using DilmerGames.Core.Singletons;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DroneActionMapping : Singleton<DroneActionMapping>
{
    public delegate void ActionRef<T>(ref T item);

    public Dictionary<DroneAction, ActionRef<int>> MovementInputBindings = new Dictionary<DroneAction, ActionRef<int>>
    {
        { DroneAction.Forward, (ref int direction) => HandleDirection(OVRInput.Button.SecondaryThumbstickUp, ref direction, DroneAction.Forward)},
        { DroneAction.Backward, (ref int direction) => HandleDirection(OVRInput.Button.SecondaryThumbstickDown, ref direction, DroneAction.Backward)},
        { DroneAction.Left, (ref int direction) => HandleDirection(OVRInput.Button.SecondaryThumbstickLeft, ref direction, DroneAction.Left)},
        { DroneAction.Right, (ref int direction) => HandleDirection(OVRInput.Button.SecondaryThumbstickRight, ref direction, DroneAction.Right)},
        { DroneAction.Up, (ref int direction) => HandleDirection(OVRInput.Button.PrimaryThumbstickUp, ref direction, DroneAction.Up)},
        { DroneAction.Down, (ref int direction) => HandleDirection(OVRInput.Button.PrimaryThumbstickDown, ref direction, DroneAction.Down)},
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

    private static void HandleDirection(OVRInput.Button button, ref int direction, DroneAction directionOption)
    {
        if (OVRInput.Get(button))
        {
            string commandFormat = string.Empty;
            switch (directionOption)
            {
                case DroneAction.Left:
                    direction -= DroneConstants.speed;
                    commandFormat = "rc {0} 0 0 0";
                    break;
                case DroneAction.Backward:
                    direction -= DroneConstants.speed;
                    commandFormat = "rc 0 {0} 0 0";
                    break;
                case DroneAction.Down:
                    direction -= DroneConstants.speed;
                    commandFormat = "rc 0 0 {0} 0";
                    break;
                case DroneAction.Right:
                    direction += DroneConstants.speed;
                    commandFormat = "rc {0} 0 0 0";
                    break;
                case DroneAction.Forward:
                    direction += DroneConstants.speed;
                    commandFormat = "rc 0 {0} 0 0";
                    break;
                case DroneAction.Up:
                    direction += DroneConstants.speed;
                    commandFormat = "rc 0 0 {0} 0";
                    break;
            }
            direction = Mathf.Clamp(direction, -100, 100);
            DroneClient.Instance.SendCommand(string.Format(commandFormat, direction));
        }
        else if (OVRInput.GetUp(button))
        {
            direction = 0;
        }
    }
}