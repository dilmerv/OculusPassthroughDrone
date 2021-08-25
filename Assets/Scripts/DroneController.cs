using System;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : MonoBehaviour
{
    private bool droneStarted = false;
    private const int speed = 20;

    private int forwardDirection = 0;
    private int backwardDirection = 0;
    private int leftDirection = 0;
    private int rightDirection = 0;
    private int upDirection = 0;
    private int downDirection = 0;

    Dictionary<DroneDirection, Action<int>> MovementBindings = new Dictionary<DroneDirection, Action<int>>
    {
        { DroneDirection.Forward, (int direction) => HandleDirection(OVRInput.Button.SecondaryThumbstickUp, ref direction, DroneDirection.Forward)},
        { DroneDirection.Backward, (int direction) => HandleDirection(OVRInput.Button.SecondaryThumbstickDown, ref direction, DroneDirection.Backward)},
        { DroneDirection.Left, (int direction) => HandleDirection(OVRInput.Button.SecondaryThumbstickLeft, ref direction, DroneDirection.Left)},
        { DroneDirection.Right, (int direction) => HandleDirection(OVRInput.Button.SecondaryThumbstickRight, ref direction, DroneDirection.Right)},
        { DroneDirection.Up, (int direction) => HandleDirection(OVRInput.Button.PrimaryThumbstickUp, ref direction, DroneDirection.Up)},
        { DroneDirection.Down, (int direction) => HandleDirection(OVRInput.Button.PrimaryThumbstickDown, ref direction, DroneDirection.Down)},
    };

    void Update()
    {
        // Start Drone Threads
        if(OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            Logger.Instance.LogInfo("Connecting to drone and starting listeners");
            DroneClient.Instance.StartDrone(ref droneStarted);
        }

        if(!droneStarted) return;

        #region Main Commands

        // initSDK - A
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            Logger.Instance.LogInfo("Init SDK");
            DroneClient.Instance.SendCommand($"{DroneCommand.command}");
        }

        // takeOff - B
        if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            Logger.Instance.LogInfo("TakeOff");
            DroneClient.Instance.SendCommand($"{DroneCommand.takeoff}");
        }

        // land - Hand trigger
        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger))
        {
            Logger.Instance.LogInfo("Landing");
            DroneClient.Instance.SendCommand($"{DroneCommand.land}");
        }

        // emergency press start button
        if (OVRInput.GetDown(OVRInput.Button.Start))
        {
            DroneClient.Instance.SendCommand($"{DroneCommand.emergency}");
        }

        #endregion

        #region Handle Movement

        MovementBindings[DroneDirection.Forward](forwardDirection);
        MovementBindings[DroneDirection.Backward](backwardDirection);
        MovementBindings[DroneDirection.Left](leftDirection);
        MovementBindings[DroneDirection.Right](rightDirection);
        MovementBindings[DroneDirection.Up](upDirection);
        MovementBindings[DroneDirection.Down](downDirection);

        #endregion
    }
    private static void HandleDirection(OVRInput.Button button, ref int direction, DroneDirection directionOption)
    {
        if (OVRInput.Get(button))
        {
            string commandFormat = string.Empty;
            switch(directionOption)
            {
                case DroneDirection.Left:
                    direction -= speed;
                    commandFormat = "rc {0} 0 0 0";
                    break;
                case DroneDirection.Backward:
                    direction -= speed;
                    commandFormat = "rc 0 {0} 0 0";
                    break;
                case DroneDirection.Down:
                    direction -= speed;
                    commandFormat = "rc 0 0 {0} 0";
                    break;
                case DroneDirection.Right:
                    direction += speed;
                    commandFormat = "rc {0} 0 0 0";
                    break;
                case DroneDirection.Forward:
                    direction += speed;
                    commandFormat = "rc 0 {0} 0 0";
                    break;
                case DroneDirection.Up:
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
