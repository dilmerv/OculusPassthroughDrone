using UnityEngine;

public class DroneController : MonoBehaviour
{
    private bool droneStarted = false;

    private const int speed = 20;

    private int leftDirection, rightDirection;
    private int forwardDirection, backwardDirection;
    private int upDirection, downDirection;

    void Update()
    {
        // initiate SDK
        if(OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) || OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            Logger.Instance.LogInfo("Starting drone and SDK");
            DroneClient.Instance.StartDrone(ref droneStarted);
        }

        if(!droneStarted) return;

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

        // forward
        if (OVRInput.Get(OVRInput.Button.SecondaryThumbstickUp))
        {
            forwardDirection += speed;
            forwardDirection = Mathf.Clamp(forwardDirection, -100, 100);
            DroneClient.Instance.SendCommand($"rc 0 {forwardDirection} 0 0");
        }
        if (OVRInput.GetUp(OVRInput.Button.SecondaryThumbstickUp))
        {
            forwardDirection = 0;
        }

        // back
        if (OVRInput.Get(OVRInput.Button.SecondaryThumbstickDown))
        {
            backwardDirection -= speed;
            backwardDirection = Mathf.Clamp(backwardDirection, -100, 100);
            DroneClient.Instance.SendCommand($"rc 0 {backwardDirection} 0 0");
        }
        if (OVRInput.GetUp(OVRInput.Button.SecondaryThumbstickDown))
        {
            backwardDirection = 0;
        }

        // left
        if (OVRInput.Get(OVRInput.Button.SecondaryThumbstickLeft))
        {
            leftDirection -= speed;
            leftDirection = Mathf.Clamp(leftDirection, -100, 100);
            DroneClient.Instance.SendCommand($"rc {leftDirection} 0 0 0");
        }
        if (OVRInput.GetUp(OVRInput.Button.SecondaryThumbstickLeft))
        {
            leftDirection = 0;
        }


        // right
        if (OVRInput.Get(OVRInput.Button.SecondaryThumbstickRight))
        {
            rightDirection += speed;
            rightDirection = Mathf.Clamp(rightDirection, -100, 100);
            DroneClient.Instance.SendCommand($"rc {rightDirection} 0 0 0");
        }
        if (OVRInput.GetUp(OVRInput.Button.SecondaryThumbstickRight))
        {
            rightDirection = 0;
        }

        // up
        if (OVRInput.Get(OVRInput.Button.PrimaryThumbstickUp))
        {
            upDirection += speed;
            upDirection = Mathf.Clamp(upDirection, -100, 100);
            DroneClient.Instance.SendCommand($"rc 0 0 {upDirection} 0");
        }
        if (OVRInput.GetUp(OVRInput.Button.PrimaryThumbstickUp))
        {
            upDirection = 0;
        }

        // down
        if (OVRInput.Get(OVRInput.Button.PrimaryThumbstickDown))
        {
            downDirection -= speed;
            downDirection = Mathf.Clamp(downDirection, -100, 100);
            DroneClient.Instance.SendCommand($"rc 0 0 {downDirection} 0");
        }
        if (OVRInput.GetUp(OVRInput.Button.PrimaryThumbstickDown))
        {
            downDirection = 0;
        }

        // emergency press start button
        if (OVRInput.GetDown(OVRInput.Button.Start))
        {
            DroneClient.Instance.SendCommand($"{DroneCommand.emergency}");
        }
    }
    private void HandleDirection(OVRInput.Button button, ref int direction, DirectionOption directionOption)
    {
        if (OVRInput.Get(button))
        {
            string commandFormat = string.Empty;
            switch(directionOption)
            {
                case DirectionOption.Left:
                    direction -= speed;
                    commandFormat = "rc {0} 0 0 0";
                    break;
                case DirectionOption.Backward:
                    direction -= speed;
                    commandFormat = "rc 0 {0} 0 0";
                    break;
                case DirectionOption.Down:
                    direction -= speed;
                    commandFormat = "rc 0 0 {0} 0";
                    break;
                case DirectionOption.Right:
                    direction += speed;
                    commandFormat = "rc {0} 0 0 0";
                    break;
                case DirectionOption.Forward:
                    direction += speed;
                    commandFormat = "rc 0 {0} 0 0";
                    break;
                case DirectionOption.Up:
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

    enum DirectionOption
    { 
        Left,
        Right,
        Forward,
        Backward,
        Up,
        Down
    }
}
