using DilmerGames.Core.Singletons;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DroneActionMapping : Singleton<DroneActionMapping>
{
    public delegate void ActionRef<T>(ref T item);

    public Dictionary<DroneAction, ActionRef<int>> ControllersMovementInputBindings = new Dictionary<DroneAction, ActionRef<int>>
    {
        { 
            DroneAction.Left, (ref int direction) => HandleDirection(OVRInput.Button.SecondaryThumbstickLeft, 
                ref direction, DroneCommand.rc, "{0} 0 0 0", DroneSpeedType.Negative, -100, 100)
        },
        { 
            DroneAction.Right, (ref int direction) => HandleDirection(OVRInput.Button.SecondaryThumbstickRight, 
            ref direction, DroneCommand.rc, "{0} 0 0 0", DroneSpeedType.Positive, -100, 100)
        },
        { 
            DroneAction.Forward, (ref int direction) => HandleDirection(OVRInput.Button.SecondaryThumbstickUp, 
            ref direction, DroneCommand.rc, "0 {0} 0 0", DroneSpeedType.Positive, -100, 100)
        },
        { 
            DroneAction.Backward, (ref int direction) => HandleDirection(OVRInput.Button.SecondaryThumbstickDown, 
            ref direction, DroneCommand.rc, "0 {0} 0 0", DroneSpeedType.Negative, -100, 100)
        },
        { 
            DroneAction.Up, (ref int direction) => HandleDirection(OVRInput.Button.PrimaryThumbstickUp, 
            ref direction, DroneCommand.rc, "0 0 {0} 0", DroneSpeedType.Positive, -100, 100)
        },
        { 
            DroneAction.Down, (ref int direction) => HandleDirection(OVRInput.Button.PrimaryThumbstickDown, 
            ref direction, DroneCommand.rc, "0 0 {0} 0", DroneSpeedType.Negative, -100, 100)
        },
        { 
            DroneAction.RotateLeft, (ref int direction) => HandleDirection(OVRInput.Button.PrimaryThumbstickLeft, 
            ref direction, DroneCommand.ccw, "{0}", DroneSpeedType.Positive, 1, 1000)
        },
        { 
            DroneAction.RotateRight, (ref int direction) => HandleDirection(OVRInput.Button.PrimaryThumbstickRight, 
            ref direction, DroneCommand.cw, "{0}", DroneSpeedType.Positive, 1, 1000)
        },
    };

    public Dictionary<DroneAction, Action> ControllerCoreActionInputBindings = new Dictionary<DroneAction, Action>
    {
        {
            DroneAction.Connect, () => HandleCoreAction(OVRInput.Button.SecondaryIndexTrigger, () => DroneClient.Instance.StartDrone())
        },
        {
            DroneAction.InitializeSDK, () =>
            {
                HandleCoreAction(OVRInput.Button.One,
                    () => DroneClient.Instance.SendCommand(
                    new DroneRequest { RequestType = RequestType.ControlCommand, Command = DroneCommand.command }),
                    () => DroneStateManager.Instance.StarStats());
            }
        },
        { 
            DroneAction.TakeOff, () => HandleCoreAction(OVRInput.Button.Two, () => DroneClient.Instance.SendCommand(
                new DroneRequest { RequestType = RequestType.ControlCommand, Command = DroneCommand.takeoff }))
        },
        { 
            DroneAction.Landing, () => HandleCoreAction(OVRInput.Button.PrimaryHandTrigger, () => DroneClient.Instance.SendCommand(
                new DroneRequest { RequestType = RequestType.ControlCommand, Command = DroneCommand.land }))
        },
        { 
            DroneAction.Emergency, () => HandleCoreAction(OVRInput.Button.Start, () => DroneClient.Instance.SendCommand(
                new DroneRequest { RequestType = RequestType.ControlCommand, Command = DroneCommand.emergency }))
        }
    };

    public Dictionary<DroneAction, Action> HandsCoreActionInputBindings = new Dictionary<DroneAction, Action>
    {
        {
            DroneAction.Connect, () => HandleCoreAction(OVRHand.Hand.HandLeft, OVRHand.HandFinger.Index, DroneConstants.HAND_TRACKING_MIN_PINCH, 
                () => DroneClient.Instance.StartDrone())
        },
        {
            DroneAction.InitializeSDK, () =>
            {
                HandleCoreAction(OVRHand.Hand.HandLeft, OVRHand.HandFinger.Middle, DroneConstants.HAND_TRACKING_MIN_PINCH,
                    () => DroneClient.Instance.SendCommand(
                    new DroneRequest { RequestType = RequestType.ControlCommand, Command = DroneCommand.command }),
                    () => DroneStateManager.Instance.StarStats());
            }
        },
        {
            DroneAction.TakeOff, () => HandleCoreAction(OVRHand.Hand.HandRight, OVRHand.HandFinger.Index, DroneConstants.HAND_TRACKING_MIN_PINCH,
                () => DroneClient.Instance.SendCommand(new DroneRequest { RequestType = RequestType.ControlCommand, Command = DroneCommand.takeoff }))
        },
        {
            DroneAction.Landing, () => HandleCoreAction(OVRHand.Hand.HandRight, OVRHand.HandFinger.Middle, DroneConstants.HAND_TRACKING_MIN_PINCH,
                () => DroneClient.Instance.SendCommand(new DroneRequest { RequestType = RequestType.ControlCommand, Command = DroneCommand.land }))
        },
        {
            DroneAction.Emergency, () => HandleCoreAction(OVRHand.Hand.HandRight, OVRHand.HandFinger.Ring, DroneConstants.HAND_TRACKING_MIN_PINCH,
                () => DroneClient.Instance.SendCommand(new DroneRequest { RequestType = RequestType.ControlCommand, Command = DroneCommand.emergency }))
        }
    };


    private static void HandleCoreAction(OVRInput.Button button, params Action[] callbacks)
    {
        if (OVRInput.GetDown(button)) foreach (var callback in callbacks) callback?.Invoke();
    }

    private static void HandleCoreAction(OVRHand.Hand hand, OVRHand.HandFinger handFinger, float minPinchStrength, params Action[] callbacks)
    {
        if(DroneController.Instance.LeftHand == null && DroneController.Instance.RightHand == null)
        {
            Logger.Instance.LogInfo("Hands currently not enabled or not supported");
            return;
        }

        OVRHand currentHand = hand == OVRHand.Hand.HandLeft ? DroneController.Instance.LeftHand : 
            DroneController.Instance.RightHand;

        if (currentHand.GetFingerIsPinching(handFinger) && currentHand.GetFingerPinchStrength(handFinger) >= minPinchStrength)
        {
            Logger.Instance.LogInfo($"{hand} {handFinger} {minPinchStrength}");
            foreach (var callback in callbacks) callback?.Invoke();
        }
    }

    private static void HandleDirection(OVRInput.Button button, ref int direction, DroneCommand command, string commandFormat, 
        DroneSpeedType droneSpeedType, float min, float max)
    {
        if (OVRInput.Get(button))
        {
            direction = droneSpeedType == DroneSpeedType.Negative ? direction - DroneConstants.DRONE_SPEED : direction + DroneConstants.DRONE_SPEED;
            DroneClient.Instance.SendCommand(new DroneRequest
            {
                RequestType = RequestType.ControlCommand,
                Command = command,
                Payload = $"{command} {string.Format(commandFormat, Mathf.Clamp(direction, min, max))}"
            });
        }
        else if (OVRInput.GetUp(button)) direction = 0;
    }
}