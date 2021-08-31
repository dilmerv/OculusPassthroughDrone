using DilmerGames.Core.Singletons;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class DroneClient : Singleton<DroneClient>
{
    private static readonly object lockObject = new object();

    [SerializeField]
    private string droneIP = "192.168.10.1";

    [SerializeField]
    private int controllerPort = 8889;

    [SerializeField]
    private int statePort = 8890;

    [SerializeField]
    private bool connectInAwake = false;

    private Queue<DroneRequest> droneRequests = new Queue<DroneRequest>();

    private UdpClient UdpClient { set; get; }

    private Thread receivingThread;

    // Connected: is set once UDP Connection is successful
    public bool Connected { private set; get; }

    // SDKInitialized: is required before sending flying commands the drone
    // this is pretty much saying let's add SDK connectivity
    public bool SDKInitialized { private set; get; }

    public DroneStats DroneStats { get; set; } = new DroneStats();

    // logging

    // Use messages to queued up log entries
    // normally needed when using a secondary thread
    public Queue<string> LogMessages { get; set; } = new Queue<string>();

    void Update()
    {
        if (LogMessages.Count > 0)
        {
            string message = LogMessages.Dequeue();
            Logger.Instance.LogInfo(message);
        }
    }

    public void Awake()
    {
        if (!connectInAwake) return;
        StartDrone();
    }

    public void StartDrone()
    {
        try
        {
            UdpClient = new UdpClient();
            UdpClient.Connect(droneIP, controllerPort);
            Connected = UdpClient.Client.Connected;
            if(Connected) UpdateLogWithLock("Connected");
        }
        catch (Exception e)
        {
            UpdateLogWithLock(e.Message);
            Connected = false;
            return;
        }

        // start threads
        receivingThread = CreateThread(ProcessCommand);
    }

    private Thread CreateThread(Action action)
    {
        ThreadStart threadStart = new ThreadStart(action);
        Thread thread = new Thread(threadStart);
        thread.Start();
        return thread;
    }

    // This method should only be used for control type commands
    // use DroneRequest parameter to read and control commands
    public void SendCommand(string command)
    {
        Enum.TryParse(command, out DroneCommand droneCommand);

        var droneRequest = new DroneRequest
        {
            RequestType = RequestType.ControlCommand,
            Command = droneCommand
        };

        droneRequests.Enqueue(droneRequest);

        byte[] message = Encoding.ASCII.GetBytes($"{droneRequest.Payload}");
        UdpClient.Send(message, message.Length);
    }

    public void SendCommand(DroneRequest droneRequest)
    {
        droneRequests.Enqueue(droneRequest);
        byte[] message = Encoding.ASCII.GetBytes($"{droneRequest.Payload}");
        UdpClient.Send(message, message.Length);
    }

    public void ProcessCommand()
    {
        while (true)
        {
            lock (lockObject)
            {
                if (droneRequests.Count > 0)
                {
                    IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Parse(droneIP), statePort);

                    try
                    {
                        byte[] receiveBytes = UdpClient.Receive(ref RemoteIpEndPoint);
                        string responseData = Encoding.ASCII.GetString(receiveBytes);

                        DroneRequest droneRequest = droneRequests.Dequeue();
                        
                        DroneStats.UpdateStats(new DroneReponse
                        {
                            Command = droneRequest.Command,
                            Response = responseData
                        });

                        // only happens once
                        if(!SDKInitialized) SDKInitialized = droneRequest.Command == DroneCommand.command;

                        UpdateLogWithLock($"Drone Request Type: {droneRequest.RequestType}");
                        UpdateLogWithLock($"Drone Request Command: {droneRequest.Command}");
                        UpdateLogWithLock($"Drone Request Payload: {droneRequest.Payload}");
                        UpdateLogWithLock($"\nServer Received: {responseData}");
                    }
                    catch (Exception e)
                    {
                        UpdateLogWithLock(e.Message);
                    }
                }
            }
        }
    }

    private void UpdateLogWithLock(string message)
    {
        lock (lockObject)
        {
            LogMessages.Enqueue(message);
        }
    }

    private void OnDestroy()
    {
        if (UdpClient != null)
        {
            UdpClient.Close();
            UdpClient.Dispose();
        }

        if (receivingThread != null)
            receivingThread.Abort();
    }
}
