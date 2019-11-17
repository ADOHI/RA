using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum InputEnum
{
    UP,
    DOWN,
    LEFT,
    RIGHT
}

public struct GyroInput
{
    public float rotX;
    public float rotY;
    public float rotZ;
    public float attX;
    public float attY;
    public float attZ;
    public float attW;
    public float accX;
    public float accY;
    public float accZ;
}
public class InputQueueManager : Singleton<InputQueueManager>
{
    
    //BothScene
    public GyroInput gyroInput;
    public Quaternion defaultQuaternion;
    public Quaternion iq;
    public float inputX;
    public float inputY;
    public float inputZ;

    //beforStartScene
    public GameObject test;
    public bool isCalibrate;

    //MainScene
    public float timeInterval = 3f;
    public Queue<Vector3> moveVectorQueue = new Queue<Vector3>();
    public Queue<Quaternion> cameraQuaternionQueue = new Queue<Quaternion>();
    public Queue<Vector3> headButtVectorQueue = new Queue<Vector3>();
    public Vector3 mirrorImageDirection;
    public Quaternion mirrorImageQuaternion;
    public Vector3 headbuttVector;
    //public BasicWalkerController basicCharacterController;
    public BasicWalkerController mirrorImageController;
    public PushObject mirrorImagePushObject;
    public CameraController mirrorImageCameraController;
    public int score = 0;
    public int finalScore = 6;
    //public GameObject map;
    //public Transform characterTransform;
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.P))
        {
            score++;
        }

        if (score >= finalScore)
        {
            DestroyImmediate(DayChanger.Instance);
            DestroyImmediate(StoneSpawner.Instance);
            DestroyImmediate(CharacterSpawner.Instance);
            SoundManager.Instance.ChangeSound();
            SceneManager.LoadScene("EndingScene");
            
            score = 0;
        }
    }
    private void FixedUpdate()
    {
        Quaternion q = new Quaternion(gyroInput.attX, gyroInput.attY, gyroInput.attZ, gyroInput.attW);
        iq = (Quaternion.Inverse(defaultQuaternion) * q);
        //headbuttVector = characterTransform.forward * -gyroInput.rotX + characterTransform.right * gyroInput.rotY;
        inputX = -iq.y;
        inputY = iq.x;
        inputZ = -iq.z;

    }

    public void MoveMirrorImage(Vector3 inputVector)
    {
        moveVectorQueue.Enqueue(inputVector);
        StartCoroutine(MoveMirrorImageCoroutine(inputVector, mirrorImageController, timeInterval));
    }

    public void RotateMirrorImage(Quaternion inputQuaternion)
    {
        cameraQuaternionQueue.Enqueue(inputQuaternion);
        StartCoroutine(RotateMirrorImageCoroutine(inputQuaternion, mirrorImageCameraController, timeInterval));
    }

    public void MoveMirrorImage(Vector3 inputVector, BasicWalkerController mirrorImageController)
    {
        moveVectorQueue.Enqueue(inputVector);
        StartCoroutine(MoveMirrorImageCoroutine(inputVector, mirrorImageController, timeInterval));
    }

    IEnumerator MoveMirrorImageCoroutine(Vector3 inputVector, BasicWalkerController mirrorImageController, float timeInterval)
    {
        yield return YieldInstructionCache.WaitForSeconds(timeInterval);
        mirrorImageDirection = moveVectorQueue.Dequeue();
    }

    IEnumerator RotateMirrorImageCoroutine(Quaternion inputQuaternion, CameraController mirrorImageCameraController, float timeInterval)
    {
        yield return YieldInstructionCache.WaitForSeconds(timeInterval);
        mirrorImageQuaternion = cameraQuaternionQueue.Dequeue();
    }

    public void HeadButtMirrorImage(Vector3 inputVector)
    {
        headButtVectorQueue.Enqueue(inputVector);
        StartCoroutine(HeadButtMirrorImageCoroutine(inputVector, mirrorImagePushObject, timeInterval));
    }

    public void HeadButtMirrorImage(Vector3 inputVector, PushObject mirrorImagePushObject)
    {
        headButtVectorQueue.Enqueue(inputVector);
        StartCoroutine(HeadButtMirrorImageCoroutine(inputVector, mirrorImagePushObject, timeInterval));
    }

    IEnumerator HeadButtMirrorImageCoroutine(Vector3 inputVector, PushObject mirrorImagePushObject, float timeInterval)
    {
        yield return YieldInstructionCache.WaitForSeconds(timeInterval);
        mirrorImagePushObject.HeadButt(inputVector);
    }

    public void SaveQuaternion()
    {
        defaultQuaternion = new Quaternion(gyroInput.attX, gyroInput.attY, gyroInput.attZ, gyroInput.attW);
        isCalibrate = true;
    }

}
