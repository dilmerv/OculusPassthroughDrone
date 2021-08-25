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

        // takeOff
        if (OVRInput.GetDown(OVRInput.Button.One))
        { 
            
        }

        // landing
        if (OVRInput.GetDown(OVRInput.Button.Two))
        {

        }
    }
}
