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

    public class DroneRequest
    { 
        public DroneCommand Command { get; set; }
    }

    public class DroneReponse
    {
        public DroneCommand Command { get; set; }

        public string Response { get; set; }
    }

    [SerializeField]
    private string droneIP = "192.168.10.1";

    [SerializeField]
    private int controllerPort = 8889;
    [SerializeField]
    private int statePort = 8890;

    [SerializeField]
    private bool connectInAwake = false;

    private Queue<string> messages = new Queue<string>();

    [SerializeField]
    private DroneStats DroneStats = new DroneStats();

    private Queue<DroneCommand> states = new Queue<DroneCommand>();

    private UdpClient UdpClient { set; get; }

    private UdpClient UdpStateClient { set; get; }

    private Thread controllerReceivingThread;

    private Thread stateReceivingThread;

    public bool Connected { private set; get; }

    public void Awake()
    {
        if (!connectInAwake) return;
        StartDrone();
    }

    public void StartDrone()
    {
        UdpClient = new UdpClient();
        try
        {
            UdpClient.Connect(droneIP, controllerPort);

            if (UdpClient.Client.Connected)
            {
                UpdateLogWithLock("Connected");
                Connected = true;
            }
        }
        catch (Exception e)
        {
            UpdateLogWithLock(e.Message);
            Connected = false;
            return;
        }

        // start threads
        controllerReceivingThread = CreateThread(ControllerReceiever);
        stateReceivingThread = CreateThread(ProcessStateCommand);
    }

    private Thread CreateThread(Action action)
    {
        ThreadStart threadStart = new ThreadStart(action);
        Thread thread = new Thread(threadStart);
        thread.Start();
        return thread;
    }

    public void SendCommand(string command)
    {
        byte[] message = Encoding.ASCII.GetBytes(command);
        UdpClient.Send(message, message.Length);
    }

    public void SendStateCommand(DroneCommand command)
    {
        states.Enqueue(command);
        byte[] message = Encoding.ASCII.GetBytes($"{command}?");
        UdpClient.Send(message, message.Length);
    }

    public void ProcessStateCommand()
    {
        while (true)
        {
            lock (lockObject)
            {
                if (states.Count > 0)
                {
                    IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, controllerPort);
                    string responseData = string.Empty;

                    try
                    {
                        byte[] receiveBytes = UdpClient.Receive(ref RemoteIpEndPoint);
                        responseData = Encoding.ASCII.GetString(receiveBytes);

                        DroneStats.battery = responseData;
                        UpdateLogWithLock($"Controller Received: {responseData}");
                    }
                    catch (Exception e)
                    {
                        UpdateLogWithLock(e.Message);
                    }
                }
            }
        }
    }

    private void Update()
    {
        if (messages.Count > 0)
        {
            string message = messages.Dequeue();
            Logger.Instance.LogInfo(message);
        }
    }

    private void UpdateLogWithLock(string message)
    {
        lock (lockObject)
        {
            messages.Enqueue(message);
        }
    }

    private void ControllerReceiever()
    {
        UpdateLogWithLock("Starting controller receiver...");

        while (true)
        {
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, controllerPort);

            try
            {
                byte[] receiveBytes = UdpClient.Receive(ref RemoteIpEndPoint);
                var returnData = Encoding.ASCII.GetString(receiveBytes);

                lock (lockObject)
                {
                    UpdateLogWithLock($"Controller Received: {returnData}");
                }
            }
            catch (Exception e)
            {
                UpdateLogWithLock(e.Message);
            }
        }
    }

    private void OnDestroy()
    {
        if(UdpClient != null)
            UdpClient.Close();

        if(controllerReceivingThread != null)
            controllerReceivingThread.Abort();

        if(stateReceivingThread != null)
            stateReceivingThread.Abort();
    }
}
