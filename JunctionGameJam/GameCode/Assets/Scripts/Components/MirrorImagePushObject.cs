using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorImagePushObject : PushObject
{
    // Start is called before the first frame update
    protected override void Start()
    {
        col = GetComponent<Collider>();
        col.enabled = false;
        headButtVector = transform.localPosition;
        InputQueueManager.Instance.mirrorImagePushObject = this;
    }

    public override void HeadButt(Vector3 vector)
    {
        if (vector.magnitude >= headButtThres)
        {
            Debug.Log("HeadButt!");
            StartCoroutine(DelayHeadButt(headButtDelay));
            StartCoroutine(HeadButtAnim(vector, headButtDistance, headButtTime));
        }
    }
    public override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("ColliderObject"))
        {
            other.attachedRigidbody.AddRelativeForce(force * headButtObjectPower);
            other.attachedRigidbody.AddRelativeTorque(force * headButtObjectPower);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("NightStone"))
        {
            var obj = other.gameObject;
            StoneSpawner.Instance.dayStones.Remove(obj);
            DestroyImmediate(other.gameObject);
            InputQueueManager.Instance.score++;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("MainCharacter"))
        {
            other.attachedRigidbody.AddRelativeForce(force * headButtCharacterPower);
            other.attachedRigidbody.AddRelativeTorque(force * headButtCharacterPower);
        }
    }
}
