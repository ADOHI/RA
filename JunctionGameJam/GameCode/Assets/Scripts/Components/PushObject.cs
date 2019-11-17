using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushObject : MonoBehaviour
{
    protected Collider col;
    public float headButtThres = 9f;
    public float headButtDelay = 1f;
    public float headButtTime = 0.4f;
    public Vector3 force;
    public bool isHeadButtAvailable = true;
    public float headButtDistance = 2f;
    public Transform characterTransform;
    public float headButtObjectPower = 1000f;
    public float headButtCharacterPower = 1000f;
    public Vector3 headButtVector;
    // Start is called before the first frame update
    virtual protected void Start()
    {
        col = GetComponent<Collider>();
        col.enabled = false;
        headButtVector = transform.localPosition;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.localRotation = new Quaternion(-InputQueueManager.Instance.iq.x, -InputQueueManager.Instance.iq.z, -InputQueueManager.Instance.iq.y, InputQueueManager.Instance.iq.w);
        if (isHeadButtAvailable)
        {
            //var headbuttVector = characterTransform.forward * InputQueueManager.Instance.gyroInput.accY + characterTransform.right * InputQueueManager.Instance.gyroInput.accX;
            var headbuttVector = InputQueueManager.Instance.gyroInput.accX * transform.right + -InputQueueManager.Instance.gyroInput.accZ * transform.up + InputQueueManager.Instance.gyroInput.accY * transform.forward;
            HeadButt(headbuttVector);
        }
        transform.localPosition = headButtVector;
    }

    public virtual void HeadButt(Vector3 vector)
    {
        if (vector.magnitude >= headButtThres)
        {
            
            Debug.Log("HeadButt!");
            StartCoroutine(DelayHeadButt(headButtDelay));
            StartCoroutine(HeadButtAnim(vector, headButtDistance, headButtTime));
            InputQueueManager.Instance.HeadButtMirrorImage(vector);
        }
    }

    public IEnumerator HeadButtAnim(Vector3 vector, float distance, float time)
    {
        force = vector;
        col.enabled = true;
        transform.localScale = new Vector3(2f, 2f, 2f);
        for (var f = 0f; f < time; f+= Time.fixedDeltaTime)
        {
            headButtVector += vector.normalized * distance * Time.fixedDeltaTime;
            
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }

        force = Vector3.zero;
        col.enabled = false;

        for (var f = 0f; f < time; f += Time.fixedDeltaTime)
        {
            headButtVector -= vector.normalized * distance * Time.fixedDeltaTime;
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }
        transform.localScale = new Vector3(1f, 1f, 1f);
        headButtVector = Vector3.zero;
    }

    public IEnumerator DelayHeadButt(float time)
    {
        isHeadButtAvailable = false;

        yield return YieldInstructionCache.WaitForSeconds(time);
        isHeadButtAvailable = true;
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("ColliderObject"))
        {
            other.attachedRigidbody.AddRelativeForce(force * headButtObjectPower);
            other.attachedRigidbody.AddRelativeTorque(force * headButtObjectPower);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("DayStone"))
        {
            var obj = other.gameObject;
            StoneSpawner.Instance.dayStones.Remove(obj);
            DestroyImmediate(other.gameObject);
            InputQueueManager.Instance.score++;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("MirrorImage"))
        {
            other.attachedRigidbody.AddRelativeForce(force * headButtCharacterPower);
            other.attachedRigidbody.AddRelativeTorque(force * headButtCharacterPower);
        }
    }
}
