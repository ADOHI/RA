using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class EndVedio : MonoBehaviour
{
    public VideoPlayer vp;

    // Update is called once per frame
    void Update()
    {
        if (vp.time > 45f)
        {
            vp.Pause();
            SceneManager.LoadScene("MainScene");
        }
        
    }
}
