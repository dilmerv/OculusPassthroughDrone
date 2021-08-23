using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class UDPNetworkClient : MonoBehaviour
{
    [SerializeField]
    private string hostOrIPAddress;

    [SerializeField]
    private int port;

    [SerializeField]
    private string rawCommand;

    private UdpClient UdpClient { set; get; }

    private void Awake()
    {
        UdpClient = new UdpClient();
        InitializeUDPConnection();
    }

    public void InitializeUDPConnection()
    {
        UdpClient.Connect(hostOrIPAddress, port);

        byte[] message = Encoding.ASCII.GetBytes(rawCommand);

        UdpClient.Send(message, message.Length);

        //IPEndPoint object will allow us to read datagrams sent from any source.
        IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

        // Blocks until a message returns on this socket from a remote host.
        byte[] receiveBytes = UdpClient.Receive(ref RemoteIpEndPoint);

        string returnData = Encoding.ASCII.GetString(receiveBytes);
    }

    private void OnDestroy()
    {
        if(UdpClient != null)
        {
            UdpClient.Close();
        }
    }
}
