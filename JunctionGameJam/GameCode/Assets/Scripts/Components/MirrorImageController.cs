using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This controller script is based on 'BasicWalkerController' and uses a 'CameraController' script to calculate the player's movement direction;
//The 'CameraController' script must be located on a child object of this gameobject;
//This script can be used for a range of different games (third-person platformers, first-person shooters,[...]);
public class MirrorImageController : BasicWalkerController
{
    //Reference to camera controls;
    CameraController cameraControls;
    private void Start()
    {
        InputQueueManager.Instance.mirrorImageController = this;
    }
    protected override void Setup()
    {
        //Search for camera controller reference in this gameobjects' children;
        cameraControls = GetComponentInChildren<CameraController>();
    }

    //Calculate movement direction based on camera controller orientation;
    protected override Vector3 CalculateMovementDirection()
    {
        return InputQueueManager.Instance.mirrorImageDirection;
    }

    //Returns reference to camera controller;
    public CameraController GetCameraController()
    {
        return cameraControls;
    }
}
