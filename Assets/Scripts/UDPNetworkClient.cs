using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class UDPNetworkClient : MonoBehaviour
{
    private static readonly object lockObject = new object();

    [SerializeField]
    private string hostOrIPAddress;

    [SerializeField]
    private int port;

    [SerializeField]
    private string rawCommand;

    [SerializeField]
    private string returnData;

    private UdpClient UdpClient { set; get; }


    private Queue<string> commands = new Queue<string>();

    private void Awake()
    {
        UdpClient = new UdpClient();
        ThreadStart threadStart = new ThreadStart(InitializeUDPConnection);
        Thread thread = new Thread(threadStart);
        thread.Start();
    }

    private void Update()
    {
        if(commands.Count > 0)
        {
            lock (lockObject)
            {
                Debug.Log("Received: " + returnData);
            }
        }
    }

    public void SendCommand()
    {
        commands.Enqueue(rawCommand);
    }

    public void InitializeUDPConnection()
    {
        UdpClient.Connect(hostOrIPAddress, port);

        while (true)
        {
            //IPEndPoint object will allow us to read datagrams sent from any source.
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

            // Blocks until a message returns on this socket from a remote host.
            byte[] receiveBytes = UdpClient.Receive(ref RemoteIpEndPoint);

            lock (lockObject)
            {
                // process commands
                if (commands.Count > 0)
                {
                    byte[] message = Encoding.ASCII.GetBytes(rawCommand);
                    UdpClient.Send(message, message.Length);
                }

                returnData = Encoding.ASCII.GetString(receiveBytes);
                if (!string.IsNullOrEmpty(returnData))
                {
                    processData = true;
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
    }
}
