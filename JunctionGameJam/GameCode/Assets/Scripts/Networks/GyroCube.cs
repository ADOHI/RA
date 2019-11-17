using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroCube : MonoBehaviour
{
    void Update()
    {
        transform.rotation = new Quaternion(InputQueueManager.Instance.iq.x, -InputQueueManager.Instance.iq.z, InputQueueManager.Instance.iq.y, InputQueueManager.Instance.iq.w);
    }
}
