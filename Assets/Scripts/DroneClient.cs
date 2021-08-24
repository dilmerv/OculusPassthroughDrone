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
    private string hostOrIPAddress = "192.168.0.1";

    [SerializeField]
    private int port = 8889;

    private UdpClient UdpClient { set; get; }

    private Queue<string> commands = new Queue<string>();

    private Queue<string> messages = new Queue<string>();

    private Thread connectAndReceiveThread;

    private Thread sendThread;

    private void Awake()
    {
        UdpClient = new UdpClient();
        connectAndReceiveThread = CreateThread(ConnectAndReceieve);
        sendThread = CreateThread(SendCommands);
    }

    private Thread CreateThread(Action action)
    {
        ThreadStart threadStart = new ThreadStart(action);
        Thread thread = new Thread(threadStart);
        thread.IsBackground = true;
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

    private void SendCommand(string command)
    {
        lock (commands)
        {
            commands.Enqueue(command);
        }
    }

    private void UpdateLogWithLock(string message)
    {
        lock (lockObject)
        {
            messages.Enqueue(message);
        }
    }

    private void ConnectAndReceieve()
    {
        try
        {
            UdpClient.Connect(hostOrIPAddress, port);
            if (UdpClient.Client.Connected) UpdateLogWithLock("Connected");
        }
        catch(Exception e)
        {
            messages.Enqueue(e.Message);
        }

        while (true)
        {
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

            try
            {
                byte[] receiveBytes = UdpClient.Receive(ref RemoteIpEndPoint);
                var returnData = Encoding.ASCII.GetString(receiveBytes);

                lock (lockObject)
                {
                    if (!string.IsNullOrEmpty(returnData))
                    {
                        UpdateLogWithLock($"Received: {returnData}");
                        messages.Enqueue(returnData);
                    }
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

        connectAndReceiveThread.Abort();
        sendThread.Abort();
    }
}
