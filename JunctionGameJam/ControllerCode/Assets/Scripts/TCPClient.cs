using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class TCPClient : MonoBehaviour
{
    public string serverIp;
    public int port;
    public InputField inputField;
    public Text connectionText;
    public bool isConnection;
    #region private members 	
    private TcpClient socketConnection;
    private Thread clientReceiveThread;
    Gyroscope m_Gyro;
    #endregion
    // Use this for initialization 	
    void Start()
    {
        //ConnectToTcpServer();
        m_Gyro = Input.gyro;
        m_Gyro.enabled = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (isConnection) connectionText.text = "Connect!";
        else connectionText.text = "DisConnect";
        SendMessage();
    }
    /// <summary> 	
    /// Setup socket connection. 	
    /// </summary> 	
    public void SetServerIP(string ip)
    {
        serverIp = ip;
    }

    public void ConnectToTcpServer()
    {
        try
        {
            clientReceiveThread = new Thread(new ThreadStart(ListenForData));
            clientReceiveThread.IsBackground = true;
            clientReceiveThread.Start();
            
        }
        catch (Exception e)
        {
            Debug.Log("On client connect exception " + e);
        }
    }
    /// <summary> 	
    /// Runs in background clientReceiveThread; Listens for incomming data. 	
    /// </summary>     
    private void ListenForData()
    {
        try
        {
            socketConnection = new TcpClient(serverIp, port);
            if (socketConnection != null)
            {
                isConnection = true;
            }
            else
            {
                isConnection = false;
            }

            Byte[] bytes = new Byte[1024];
            while (true)
            {
                // Get a stream object for reading 				
                using (NetworkStream stream = socketConnection.GetStream())
                {
                    int length;
                    // Read incomming stream into byte arrary. 					
                    while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        var incommingData = new byte[length];
                        Array.Copy(bytes, 0, incommingData, 0, length);
                        // Convert byte array to string message. 						
                        string serverMessage = Encoding.ASCII.GetString(incommingData);
                        //Debug.Log("server message received as: " + serverMessage);
                    }
                }
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("Socket exception: " + socketException);
        }
    }
    /// <summary> 	
    /// Send message to server using socket connection. 	
    /// </summary> 	
    public void SendMessage()
    {
        if (socketConnection == null)
        {
            return;
        }
        try
        {
            // Get a stream object for writing. 			
            NetworkStream stream = socketConnection.GetStream();
            if (stream.CanWrite)
            {
                string clientMessage = m_Gyro.rotationRate.x.ToString("0.000") + ' ' + m_Gyro.rotationRate.y.ToString("0.000") + ' ' + (-m_Gyro.rotationRate.z).ToString("0.000") + ' ' + m_Gyro.attitude.x.ToString("0.000") + ' ' + m_Gyro.attitude.y.ToString("0.000") + ' ' + (-m_Gyro.attitude.z).ToString("0.000") + ' ' + (-m_Gyro.attitude.w).ToString("0.000") + ' ' + m_Gyro.userAcceleration.x.ToString("0.000") + ' ' + m_Gyro.userAcceleration.y.ToString("0.000") + ' ' + m_Gyro.userAcceleration.z.ToString("0.000") + " strend";
                
                byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(clientMessage);
                // Write byte array to socketConnection stream.                 
                stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("Socket exception: " + socketException);
        }
    }
}