using System;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : MonoBehaviour
{
    private const int speed = 20;
    private int forwardDirection = 0;
    private int backwardDirection = 0;
    private int leftDirection = 0;
    private int rightDirection = 0;
    private int upDirection = 0;
    private int downDirection = 0;

    Dictionary<DroneAction, Action<int>> MovementInputBindings = new Dictionary<DroneAction, Action<int>>
    {
        { DroneAction.Forward, (int direction) => HandleDirection(OVRInput.Button.SecondaryThumbstickUp, ref direction, DroneAction.Forward)},
        { DroneAction.Backward, (int direction) => HandleDirection(OVRInput.Button.SecondaryThumbstickDown, ref direction, DroneAction.Backward)},
        { DroneAction.Left, (int direction) => HandleDirection(OVRInput.Button.SecondaryThumbstickLeft, ref direction, DroneAction.Left)},
        { DroneAction.Right, (int direction) => HandleDirection(OVRInput.Button.SecondaryThumbstickRight, ref direction, DroneAction.Right)},
        { DroneAction.Up, (int direction) => HandleDirection(OVRInput.Button.PrimaryThumbstickUp, ref direction, DroneAction.Up)},
        { DroneAction.Down, (int direction) => HandleDirection(OVRInput.Button.PrimaryThumbstickDown, ref direction, DroneAction.Down)},
    };

    Dictionary<DroneAction, Action> CoreActionInputBindings = new Dictionary<DroneAction, Action>
    {
        { DroneAction.Connect, () => HandleCoreAction(OVRInput.Button.SecondaryIndexTrigger, () => DroneClient.Instance.StartDrone())},
        { DroneAction.InitializeSDK, () => HandleCoreAction(OVRInput.Button.One, () => DroneClient.Instance.SendCommand($"{DroneCommand.command}"))},
        { DroneAction.TakeOff, () => HandleCoreAction(OVRInput.Button.Two, () => DroneClient.Instance.SendCommand($"{DroneCommand.takeoff}"))},
        { DroneAction.Landing, () => HandleCoreAction(OVRInput.Button.PrimaryHandTrigger, () => DroneClient.Instance.SendCommand($"{DroneCommand.land}"))},
        { DroneAction.Emergency, () => HandleCoreAction(OVRInput.Button.Start, () => DroneClient.Instance.SendCommand($"{DroneCommand.emergency}"))},
    };

    void Update()
    {
        CoreActionInputBindings[DroneAction.Connect]();

        if (!DroneClient.Instance.Connected) return;

        #region Main Commands
        CoreActionInputBindings[DroneAction.InitializeSDK]();
        CoreActionInputBindings[DroneAction.TakeOff]();
        CoreActionInputBindings[DroneAction.Landing]();
        CoreActionInputBindings[DroneAction.Emergency]();
        #endregion

        #region Handle Movement
        MovementInputBindings[DroneAction.Forward](forwardDirection);
        MovementInputBindings[DroneAction.Backward](backwardDirection);
        MovementInputBindings[DroneAction.Left](leftDirection);
        MovementInputBindings[DroneAction.Right](rightDirection);
        MovementInputBindings[DroneAction.Up](upDirection);
        MovementInputBindings[DroneAction.Down](downDirection);
        #endregion
    }

    private static void HandleCoreAction(OVRInput.Button button, Action callback)
    {
        if (OVRInput.GetDown(button)) callback?.Invoke();
    }

    private static void HandleDirection(OVRInput.Button button, ref int direction, DroneAction directionOption)
    {
        if (OVRInput.Get(button))
        {
            string commandFormat = string.Empty;
            switch(directionOption)
            {
                case DroneAction.Left:
                    direction -= speed;
                    commandFormat = "rc {0} 0 0 0";
                    break;
                case DroneAction.Backward:
                    direction -= speed;
                    commandFormat = "rc 0 {0} 0 0";
                    break;
                case DroneAction.Down:
                    direction -= speed;
                    commandFormat = "rc 0 0 {0} 0";
                    break;
                case DroneAction.Right:
                    direction += speed;
                    commandFormat = "rc {0} 0 0 0";
                    break;
                case DroneAction.Forward:
                    direction += speed;
                    commandFormat = "rc 0 {0} 0 0";
                    break;
                case DroneAction.Up:
                    direction += speed;
                    commandFormat = "rc 0 0 {0} 0";
                    break;
            }
            direction = Mathf.Clamp(direction, -100, 100);
            DroneClient.Instance.SendCommand(string.Format(commandFormat, direction));
        }
        if (OVRInput.GetUp(button))
        {
            direction = 0;
        }
    }
}
