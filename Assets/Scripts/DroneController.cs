using System;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : MonoBehaviour
{
    private bool droneStarted = false;
    private const int speed = 20;

    Dictionary<DroneDirection, Action> MovementBindings = new Dictionary<DroneDirection, Action>
    {
        { DroneDirection.Forward, () => HandleDirection(OVRInput.Button.SecondaryThumbstickUp, DroneDirection.Forward)},
        { DroneDirection.Backward, () => HandleDirection(OVRInput.Button.SecondaryThumbstickDown, DroneDirection.Backward)},
        { DroneDirection.Left, () => HandleDirection(OVRInput.Button.SecondaryThumbstickLeft, DroneDirection.Left)},
        { DroneDirection.Right, () => HandleDirection(OVRInput.Button.SecondaryThumbstickRight, DroneDirection.Right)},
        { DroneDirection.Up, () => HandleDirection(OVRInput.Button.PrimaryThumbstickUp, DroneDirection.Up)},
        { DroneDirection.Down, () => HandleDirection(OVRInput.Button.PrimaryThumbstickDown, DroneDirection.Down)},
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

        MovementBindings[DroneDirection.Forward]();
        MovementBindings[DroneDirection.Backward]();
        MovementBindings[DroneDirection.Left]();
        MovementBindings[DroneDirection.Right]();
        MovementBindings[DroneDirection.Up]();
        MovementBindings[DroneDirection.Down]();

        #endregion
    }
    private static void HandleDirection(OVRInput.Button button, DroneDirection directionOption)
    {
        int direction = 0;

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
