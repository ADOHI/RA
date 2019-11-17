using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServerUIController : MonoBehaviour
{
    public Text serverStatusText;
    public Text serverIPText;
    public Text controllerConnectionText;
    public GameObject caliButton;
    public GameObject startButton;
    // Start is called before the first frame update
    void Start()
    {
        serverIPText.text = "Server IP Address: " + IPManager.GetIP(ADDRESSFAM.IPv4);
    }

    // Update is called once per frame
    void Update()
    {
        if (TCPTestServer.Instance.isServerOn)
        {
            serverStatusText.text = "ServerStatus: " + "ON";
        }
        else
        {
            serverStatusText.text = "ServerStatus: " + "OFF";
        }

        if (TCPTestServer.Instance.isControllerOn)
        {
            controllerConnectionText.text = "Controller Connection: " + "Connect!";
            caliButton.SetActive(true);
        }
        else
        {
            controllerConnectionText.text = "Controller Connection: " + "Disconnect";
            caliButton.SetActive(false);
        }
        startButton.SetActive(InputQueueManager.Instance.isCalibrate && TCPTestServer.Instance.isControllerOn);
    }
}
