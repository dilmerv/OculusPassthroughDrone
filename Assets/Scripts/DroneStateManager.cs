using DilmerGames.Core.Singletons;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class DroneStateManager : Singleton<DroneStateManager>
{
    [SerializeField]
    private float updateFrequency = 1.0f;

    [SerializeField]
    private TextMeshProUGUI batteryText;
    [SerializeField]
    private TextMeshProUGUI speedText;
    [SerializeField]
    private TextMeshProUGUI timeText;
    [SerializeField]
    private TextMeshProUGUI tempText;
    [SerializeField]
    private TextMeshProUGUI tofText;

    [SerializeField]
    private TextMeshProUGUI lastUpdated;

    private Coroutine statsCoroutine;

    public void StarStats()
    {
        statsCoroutine = StartCoroutine(StartStatsUpdate());
    }

    public IEnumerator StartStatsUpdate()
    {
        if (!DroneClient.Instance.SDKInitialized)
        {
            Logger.Instance.LogWarning("SDK is not initialized yet");
            yield return null;
        }

        while (true) 
        {
            yield return new WaitForSeconds(updateFrequency);

            DroneCommand[] commands = new DroneCommand[] { DroneCommand.battery, DroneCommand.speed, 
                DroneCommand.time, DroneCommand.temp, DroneCommand.tof };

            foreach (var command in commands)
            {
                if (DroneClient.Instance.SDKInitialized)
                {
                    DroneClient.Instance.SendCommand(new DroneRequest
                    {
                        RequestType = RequestType.ReadCommand,
                        Command = command
                    });
                }
            }

            batteryText.text = $"Battery: {DroneClient.Instance.DroneStats.battery}";
            speedText.text = $"Speed: {DroneClient.Instance.DroneStats.speed}";
            timeText.text = $"Time: {DroneClient.Instance.DroneStats.time}";
            tempText.text = $"Temp: {DroneClient.Instance.DroneStats.temp}";
            tofText.text = $"Tof: {DroneClient.Instance.DroneStats.tof}";

            lastUpdated.text = $"{DateTime.Now}";
        }
    }

    public void GetBattery()
    {
        if (DroneClient.Instance.SDKInitialized) 
        {
            DroneClient.Instance.SendCommand(new DroneRequest
            {
                RequestType = RequestType.ReadCommand,
                Command = DroneCommand.battery
            });
        }
    }

    public void GetSpeed()
    {
        if (DroneClient.Instance.SDKInitialized)
        {
            DroneClient.Instance.SendCommand(new DroneRequest
            {
                RequestType = RequestType.ReadCommand,
                Command = DroneCommand.speed
            });
        }
    }

    public void GetTemp()
    {
        if (DroneClient.Instance.SDKInitialized)
        {
            DroneClient.Instance.SendCommand(new DroneRequest
            {
                RequestType = RequestType.ReadCommand,
                Command = DroneCommand.temp
            });
        }
    }

    public void GetTime()
    {
        if (DroneClient.Instance.SDKInitialized)
        {
            DroneClient.Instance.SendCommand(new DroneRequest
            {
                RequestType = RequestType.ReadCommand,
                Command = DroneCommand.time
            });
        }
    }

    public void GetHeight()
    {
        if (DroneClient.Instance.SDKInitialized)
        {
            DroneClient.Instance.SendCommand(new DroneRequest
            {
                RequestType = RequestType.ReadCommand,
                Command = DroneCommand.height
            });
        }
    }

    public void GetAcceleration()
    {
        if (DroneClient.Instance.SDKInitialized)
        {
            DroneClient.Instance.SendCommand(new DroneRequest
            {
                RequestType = RequestType.ReadCommand,
                Command = DroneCommand.acceleration
            });
        }
    }

    public void GetToF()
    {
        if (DroneClient.Instance.SDKInitialized)
        {
            DroneClient.Instance.SendCommand(new DroneRequest
            {
                RequestType = RequestType.ReadCommand,
                Command = DroneCommand.tof
            });
        }
    }

    public void GetBaro()
    {
        if (DroneClient.Instance.SDKInitialized)
        {
            DroneClient.Instance.SendCommand(new DroneRequest
            {
                RequestType = RequestType.ReadCommand,
                Command = DroneCommand.baro
            });
        }
    }
}
