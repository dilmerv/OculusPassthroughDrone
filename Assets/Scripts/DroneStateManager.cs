using DilmerGames.Core.Singletons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneStateManager : Singleton<DroneStateManager>
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetBattery()
    {
        if (DroneClient.Instance.Connected) 
        {
            DroneClient.Instance.SendStateCommand(DroneCommand.battery);
        }
    }
}
