using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class TCPTestServer : Singleton<TCPTestServer>
{
    #region private members 	
    /// <summary> 	
    /// TCPListener to listen for incomming TCP connection 	
    /// requests. 	
    /// </summary> 	
    private TcpListener tcpListener;
    /// <summary> 
    /// Background thread for TcpServer workload. 	
    /// </summary> 	
    private Thread tcpListenerThread;
    /// <summary> 	
    /// Create handle to connected tcp client. 	
    /// </summary> 	
    private TcpClient connectedTcpClient;
    #endregion
    public int port;
    public string ip;
    public bool isServerOn;
    public bool isControllerOn;

    // Use this for initialization
    void Start()
    {
        // Start TcpServer background thread
        ip = IPManager.GetIP(ADDRESSFAM.IPv4);
        tcpListenerThread = new Thread(new ThreadStart(ListenForIncommingRequests));
        tcpListenerThread.IsBackground = true;
        tcpListenerThread.Start();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (connectedTcpClient == null)
        {
            isControllerOn = false;
        }
        else
        {
            isControllerOn = true;
        }
    }

    /// <summary> 	
    /// Runs in background TcpServerThread; Handles incomming TcpClient requests 	
    /// </summary> 	
    private void ListenForIncommingRequests()
    {
        try
        {
            // Create listener on localhost port 8052. 			
            tcpListener = new TcpListener(IPAddress.Parse(ip), port);
            tcpListener.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            tcpListener.Start();

            Debug.Log("Server is listening");
            Byte[] bytes = new Byte[1024];
            isServerOn = true;
            while (true)
            {
                using (connectedTcpClient = tcpListener.AcceptTcpClient())
                {
                    // Get a stream object for reading 					
                    using (NetworkStream stream = connectedTcpClient.GetStream())
                    {
                        int length;
                        // Read incomming stream into byte arrary. 						
                        while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                        {
                            var incommingData = new byte[length];
                            Array.Copy(bytes, 0, incommingData, 0, length);
                            // Convert byte array to string message. 							
                            string clientMessage = Encoding.ASCII.GetString(incommingData);
                            if (clientMessage != null)
                            {
                                try
                                {
                                    var values = clientMessage.Split(' ');
                                    InputQueueManager.Instance.gyroInput.rotX = float.Parse(values[0]);
                                    InputQueueManager.Instance.gyroInput.rotY = float.Parse(values[1]);
                                    InputQueueManager.Instance.gyroInput.rotZ = float.Parse(values[2]);
                                    InputQueueManager.Instance.gyroInput.attX = float.Parse(values[3]);
                                    InputQueueManager.Instance.gyroInput.attY = float.Parse(values[4]);
                                    InputQueueManager.Instance.gyroInput.attZ = float.Parse(values[5]);
                                    InputQueueManager.Instance.gyroInput.attW = float.Parse(values[6]);
                                    InputQueueManager.Instance.gyroInput.accX = float.Parse(values[7]);
                                    InputQueueManager.Instance.gyroInput.accY = float.Parse(values[8]);
                                    InputQueueManager.Instance.gyroInput.accZ = float.Parse(values[9]);
                                }
                                catch { }
                            }
                        }
                    }
                }
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("SocketException " + socketException.ToString());
        }
        isServerOn = true;
    }
    /// <summary> 	
    /// Send message to client using socket connection. 	
    /// </summary> 	
    public void SendMessage()
    {
        if (connectedTcpClient == null)
        {
            return;
        }

        try
        {
            // Get a stream object for writing. 			
            NetworkStream stream = connectedTcpClient.GetStream();
            if (stream.CanWrite)
            {
                string serverMessage = "This is a message from your server.";
                // Convert string message to byte array.                 
                byte[] serverMessageAsByteArray = Encoding.ASCII.GetBytes(serverMessage);
                // Write byte array to socketConnection stream.               
                stream.Write(serverMessageAsByteArray, 0, serverMessageAsByteArray.Length);
                Debug.Log("Server sent his message - should be received by client");
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("Socket exception: " + socketException);
        }
    }
}