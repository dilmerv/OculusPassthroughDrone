using DilmerGames.Core.Singletons;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DroneActionMapping : Singleton<DroneActionMapping>
{
    public delegate void ActionRef<T>(ref T item);

    public Dictionary<DroneAction, ActionRef<int>> MovementInputBindings = new Dictionary<DroneAction, ActionRef<int>>
    {
        { DroneAction.Left, (ref int direction) => HandleDirection(OVRInput.Button.SecondaryThumbstickLeft, ref direction,      "rc {0} 0 0 0", DroneSpeedType.Negative)},
        { DroneAction.Right, (ref int direction) => HandleDirection(OVRInput.Button.SecondaryThumbstickRight, ref direction,    "rc {0} 0 0 0", DroneSpeedType.Positive)},
        { DroneAction.Forward, (ref int direction) => HandleDirection(OVRInput.Button.SecondaryThumbstickUp, ref direction,     "rc 0 {0} 0 0", DroneSpeedType.Positive)},
        { DroneAction.Backward, (ref int direction) => HandleDirection(OVRInput.Button.SecondaryThumbstickDown, ref direction,  "rc 0 {0} 0 0", DroneSpeedType.Negative)},
        { DroneAction.Up, (ref int direction) => HandleDirection(OVRInput.Button.PrimaryThumbstickUp, ref direction,            "rc 0 0 {0} 0", DroneSpeedType.Positive)},
        { DroneAction.Down, (ref int direction) => HandleDirection(OVRInput.Button.PrimaryThumbstickDown, ref direction,        "rc 0 0 {0} 0", DroneSpeedType.Negative)},
        { DroneAction.YawLeft, (ref int direction) => HandleDirection(OVRInput.Button.PrimaryThumbstickLeft, ref direction,     "rc 0 0 0 {0}", DroneSpeedType.Negative)},
        { DroneAction.YawRight, (ref int direction) => HandleDirection(OVRInput.Button.PrimaryThumbstickRight, ref direction,   "rc 0 0 0 {0}", DroneSpeedType.Positive)},
    };

    public Dictionary<DroneAction, Action> CoreActionInputBindings = new Dictionary<DroneAction, Action>
    {
        { DroneAction.Connect, () => HandleCoreAction(OVRInput.Button.SecondaryIndexTrigger, () => DroneClient.Instance.StartDrone())},
        { DroneAction.InitializeSDK, () => HandleCoreAction(OVRInput.Button.One, () => DroneClient.Instance.SendCommand($"{DroneCommand.command}"))},
        { DroneAction.TakeOff, () => HandleCoreAction(OVRInput.Button.Two, () => DroneClient.Instance.SendCommand($"{DroneCommand.takeoff}"))},
        { DroneAction.Landing, () => HandleCoreAction(OVRInput.Button.PrimaryHandTrigger, () => DroneClient.Instance.SendCommand($"{DroneCommand.land}"))},
        { DroneAction.Emergency, () => HandleCoreAction(OVRInput.Button.Start, () => DroneClient.Instance.SendCommand($"{DroneCommand.emergency}"))},
    };

    private static void HandleCoreAction(OVRInput.Button button, Action callback)
    {   
        if (OVRInput.GetDown(button)) callback?.Invoke();
    }

    private static void HandleDirection(OVRInput.Button button, ref int direction, string commandFormat, DroneSpeedType droneSpeedType)
    {
        if (OVRInput.Get(button))
        {
            direction = droneSpeedType == DroneSpeedType.Negative ? direction - DroneConstants.speed : direction + DroneConstants.speed;
            DroneClient.Instance.SendCommand(string.Format(commandFormat, Mathf.Clamp(direction, -100, 100)));
        }
        else if (OVRInput.GetUp(button)) direction = 0;
    }
}