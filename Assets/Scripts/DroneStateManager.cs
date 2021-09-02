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
    private TextMeshProUGUI pitchText;
    
    [SerializeField]
    private TextMeshProUGUI yawText;
    
    [SerializeField]
    private TextMeshProUGUI rollText;
    
    [SerializeField]
    private TextMeshProUGUI batteryText;

    [SerializeField]
    private TextMeshProUGUI timeText;

    [SerializeField]
    private TextMeshProUGUI tofText;

    [SerializeField]
    private TextMeshProUGUI heightText;

    [SerializeField]
    private TextMeshProUGUI baroText;
    
    [SerializeField]
    private TextMeshProUGUI templText;

    [SerializeField]
    private TextMeshProUGUI temphText;

    [SerializeField]
    private TextMeshProUGUI agxText;

    [SerializeField]
    private TextMeshProUGUI agyText;
    
    [SerializeField]
    private TextMeshProUGUI agzText;

    [SerializeField]
    private TextMeshProUGUI lastUpdated;

    [SerializeField]
    private TextMeshProUGUI stateText;

    private Coroutine statsCoroutine;

    public void StarStats() => statsCoroutine = StartCoroutine(StartStatsUpdate());

    public IEnumerator StartStatsUpdate()
    {
        while (true) 
        {
            yield return new WaitForSeconds(updateFrequency);

            if (!DroneClient.Instance.SDKInitialized)
            {
                Logger.Instance.LogWarning("SDK is not initialized yet");
                stateText.text = $"State: <color=red>offline</color>";
                continue;
            }

            stateText.text = $"State: <color=green>online</color>";
            pitchText.text = $"Pitch: {DroneClient.Instance.DroneStats.pitch}";
            yawText.text = $"Yaw: {DroneClient.Instance.DroneStats.yaw}";
            rollText.text = $"Yaw: {DroneClient.Instance.DroneStats.roll}";
            batteryText.text = $"Battery: {DroneClient.Instance.DroneStats.battery}";
            timeText.text = $"Time: {DroneClient.Instance.DroneStats.time}";
            tofText.text = $"Tof: {DroneClient.Instance.DroneStats.tof}";
            heightText.text = $"Height: {DroneClient.Instance.DroneStats.height}";
            baroText.text = $"Baro: {DroneClient.Instance.DroneStats.baro}";
            templText.text = $"Templ: {DroneClient.Instance.DroneStats.templ}";
            temphText.text = $"Temph: {DroneClient.Instance.DroneStats.temph}";
            agxText.text = $"Temph: {DroneClient.Instance.DroneStats.agx}";
            agyText.text = $"Temph: {DroneClient.Instance.DroneStats.agy}";
            agzText.text = $"Temph: {DroneClient.Instance.DroneStats.agz}";

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
