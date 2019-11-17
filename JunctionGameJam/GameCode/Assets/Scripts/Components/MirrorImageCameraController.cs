using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorImageCameraController : CameraController
{
    void Update()
    {
        transform.localRotation = Quaternion.LerpUnclamped(transform.localRotation, InputQueueManager.Instance.mirrorImageQuaternion, Time.deltaTime * 3f);
        //transform.localRotation = new Quaternion(InputQueueManager.Instance.iq.x, -InputQueueManager.Instance.iq.z, -InputQueueManager.Instance.iq.y, InputQueueManager.Instance.iq.w);
        //HandleCameraRotation();
    }
}
