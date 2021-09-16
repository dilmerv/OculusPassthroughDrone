using DilmerGames.Core.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
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

    public Dictionary<DroneAction, ActionRef<bool>> HandsCoreActionInputBindings = new Dictionary<DroneAction, ActionRef<bool>>
    {
        // TODO: When OpenXR supports GetFingerIsPinching we can use the code below:
        //{
        //    DroneAction.Connect, () => HandleCoreAction(OVRHand.Hand.HandLeft, OVRHand.HandFinger.Index, DroneConstants.HAND_TRACKING_MIN_PINCH, 
        //        () => DroneClient.Instance.StartDrone())
        //},
        {
            DroneAction.Connect, (ref bool pinchDown) => HandleCoreAction(OVRHand.Hand.HandLeft, OVRSkeleton.BoneId.Hand_IndexTip,
                DroneConstants.HAND_TRACKING_MIN_FINGER_PINCH_DOWN, DroneConstants.HAND_TRACKING_MIN_FINGER_PINCH_RELEASE, ref pinchDown,
                () => DroneClient.Instance.StartDrone())
        },
        {
            DroneAction.InitializeSDK, (ref bool pinchDown) =>
            {
                HandleCoreAction(OVRHand.Hand.HandLeft,  OVRSkeleton.BoneId.Hand_MiddleTip,
                    DroneConstants.HAND_TRACKING_MIN_FINGER_PINCH_DOWN, DroneConstants.HAND_TRACKING_MIN_FINGER_PINCH_RELEASE, ref pinchDown,
                    () => DroneClient.Instance.SendCommand(
                    new DroneRequest { RequestType = RequestType.ControlCommand, Command = DroneCommand.command }),
                    () => DroneStateManager.Instance.StarStats());
            }
        },
        {
            DroneAction.TakeOff, (ref bool pinchDown) => HandleCoreAction(OVRHand.Hand.HandRight, OVRSkeleton.BoneId.Hand_IndexTip, 
                DroneConstants.HAND_TRACKING_MIN_FINGER_PINCH_DOWN, DroneConstants.HAND_TRACKING_MIN_FINGER_PINCH_RELEASE, ref pinchDown,
                () => DroneClient.Instance.SendCommand(new DroneRequest { RequestType = RequestType.ControlCommand, Command = DroneCommand.takeoff }))
        },
        {
            DroneAction.Landing, (ref bool pinchDown) => HandleCoreAction(OVRHand.Hand.HandRight, OVRSkeleton.BoneId.Hand_MiddleTip,
                DroneConstants.HAND_TRACKING_MIN_FINGER_PINCH_DOWN, DroneConstants.HAND_TRACKING_MIN_FINGER_PINCH_RELEASE, ref pinchDown,
                () => DroneClient.Instance.SendCommand(new DroneRequest { RequestType = RequestType.ControlCommand, Command = DroneCommand.land }))
        },
        {
            DroneAction.Emergency, (ref bool pinchDown) => HandleCoreAction(OVRHand.Hand.HandRight, OVRSkeleton.BoneId.Hand_RingTip,
                DroneConstants.HAND_TRACKING_MIN_FINGER_PINCH_DOWN, DroneConstants.HAND_TRACKING_MIN_FINGER_PINCH_RELEASE, ref pinchDown,
                () => DroneClient.Instance.SendCommand(new DroneRequest { RequestType = RequestType.ControlCommand, Command = DroneCommand.emergency }))
        }
    };


    private static void HandleCoreAction(OVRInput.Button button, params Action[] callbacks)
    {
        if (OVRInput.GetDown(button)) foreach (var callback in callbacks) callback?.Invoke();
    }

    private static void HandleCoreAction(OVRHand.Hand hand, OVRSkeleton.BoneId boneId, float minPinchDown, float minPinchRelease, ref bool pinchDown, params Action[] callbacks)
    {
        if(DroneController.Instance.LeftHand == null && DroneController.Instance.RightHand == null)
        {
            Logger.Instance.LogInfo("Hands currently not enabled or not supported");
            return;
        }

        OVRSkeleton handSkeleton = hand == OVRHand.Hand.HandLeft ? DroneController.Instance.LeftHandSkeleton : 
            DroneController.Instance.RightHandSkeleton;

        var thumbTip = handSkeleton.Bones.SingleOrDefault(b => b.Id == OVRSkeleton.BoneId.Hand_ThumbTip);
        var finger = handSkeleton.Bones.SingleOrDefault(b => b.Id == boneId);

        if (thumbTip != null && finger != null)
        {
            if (Vector2.Distance(thumbTip.Transform.position, finger.Transform.position) <= minPinchDown && !pinchDown)
            {
                Logger.Instance.LogInfo($"{hand} - pinch down with boneId {boneId}: {Vector2.Distance(thumbTip.Transform.position, finger.Transform.position)}");
                pinchDown = true;
                foreach (var callback in callbacks) callback?.Invoke();
            }
            else if (pinchDown && Vector2.Distance(thumbTip.Transform.position, finger.Transform.position) >= minPinchRelease)
            {
                Logger.Instance.LogInfo($"{hand} - pinch released with boneId {boneId}: {Vector2.Distance(thumbTip.Transform.position, finger.Transform.position)}");
                pinchDown = false;
            }
        }

        // OpenXR doesn't GetFingerIsPinching or Strength so until it is fixed we can't use the following code:
        /* if (currentHand.GetFingerIsPinching(handFinger) && currentHand.GetFingerPinchStrength(handFinger) >= minPinchStrength)
        {
            Logger.Instance.LogInfo($"{hand} {handFinger} {minPinchStrength}");
            foreach (var callback in callbacks) callback?.Invoke();
        }
        */
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