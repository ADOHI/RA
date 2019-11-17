using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    public AudioSource clip;

    public AudioSource clip1;
    public AudioSource clip2;

    public void ChangeSound()
    {
        clip.Pause();
        clip1.Play();
        clip2.Play();
    }

}
