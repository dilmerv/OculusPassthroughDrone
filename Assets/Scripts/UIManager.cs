using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private bool droneStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StartDroneFromUI()
    {
        DroneClient.Instance.StartDrone(ref droneStarted);
    }

    // Update is called once per frame
    void Update()
    {
        // initiate SDK
        if(OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) || OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            Logger.Instance.LogInfo("Starting drone and SDK");
            DroneClient.Instance.StartDrone(ref droneStarted);
        }

        if(!droneStarted)
        {
            return;
        }

        // initSDK
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            Logger.Instance.LogInfo("Init SDK");
            DroneClient.Instance.SendCommand($"{DroneCommand.command}"); 
        }

        // takeOff
        if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            Logger.Instance.LogInfo("TakeOff");
            DroneClient.Instance.SendCommand($"{DroneCommand.takeoff}");

        }

        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger) || OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger))
        {
            Logger.Instance.LogInfo("Landing");
            DroneClient.Instance.SendCommand($"{DroneCommand.land}");

        }
    }
}
