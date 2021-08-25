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
    private string sendAndReceiveIP = "192.168.10.1";

    [SerializeField]
    private int sendAndReceivePort = 8889;

    [SerializeField]
    private bool connectInAwake = false;

    private UdpClient UdpClient { set; get; }

    private Queue<string> commands = new Queue<string>();

    private Queue<string> messages = new Queue<string>();

    private Thread receivingThread;

    private Thread sendingThread;

    public void Awake()
    {
        if (!connectInAwake) return;
        bool startDrone = false;
        StartDrone(ref startDrone);
    }

    public void StartDrone(ref bool droneStarted)
    {
        UdpClient = new UdpClient();
        try
        {
            UdpClient.Connect(sendAndReceiveIP, sendAndReceivePort);
            if (UdpClient.Client.Connected)
            {
                UpdateLogWithLock("Connected");
                droneStarted = true;
            }
        }
        catch (Exception e)
        {
            messages.Enqueue(e.Message);
            droneStarted = false;
            return;
        }

        receivingThread = CreateThread(Receieve);
        sendingThread = CreateThread(SendCommands);
    }

    private Thread CreateThread(Action action)
    {
        ThreadStart threadStart = new ThreadStart(action);
        Thread thread = new Thread(threadStart);
        thread.Start();

        return thread;
    }

    private void Update()
    {
        lock (lockObject)
        {
            if (messages.Count > 0)
            {
                var message = messages.Dequeue();
                Logger.Instance.LogInfo(message);
            }
        }
    }

    public void SendCommand(string command)
    {
        lock (lockObject)
        {
            commands.Enqueue(command);
            string[] commandCombination = command.Split(' ');
            DroneCommand droneCommand;
            //if (commandCombination.Length > 1 && Enum.TryParse(commandCombination[0], out droneCommand))
            //    commands.Enqueue($"{droneCommand} {commandCombination[1]}");
            //else if (commandCombination.Length == 1 && Enum.TryParse(command, out droneCommand))
                //commands.Enqueue($"{droneCommand}");
            //else
              //  messages.Enqueue($"Invalid command: {command}");
        }
    }

    private void UpdateLogWithLock(string message)
    {
        lock (lockObject)
        {
            messages.Enqueue(message);
        }
    }

    private void Receieve()
    {
        UpdateLogWithLock("Starting receieving thread...");

        while (true)
        {
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

            try
            {
                byte[] receiveBytes = UdpClient.Receive(ref RemoteIpEndPoint);
                var returnData = Encoding.ASCII.GetString(receiveBytes);

                lock (lockObject)
                {
                    //if (!string.IsNullOrEmpty(returnData))
                    //{
                        UpdateLogWithLock($"Received: {returnData}");
                        messages.Enqueue(returnData);
                    //}
                }
            }
            catch (Exception e)
            {
                UpdateLogWithLock(e.Message);
            }
        }
    }

    private void SendCommands()
    {
        UpdateLogWithLock("Starting sending thread...");

        while (true)
        {
            lock (lockObject)
            {
                // process commands
                if (commands.Count > 0)
                {
                    var command = commands.Dequeue();

                    UpdateLogWithLock($"Sending: {command}");

                    byte[] message = Encoding.ASCII.GetBytes(command);
                    UdpClient.Send(message, message.Length);
                }
            }
        }
    }

    private void OnDestroy()
    {
        if(UdpClient != null)
        {
            UdpClient.Close();
        }

        if(receivingThread != null)
            receivingThread.Abort();

        if(sendingThread != null)
            sendingThread.Abort();
    }
}
