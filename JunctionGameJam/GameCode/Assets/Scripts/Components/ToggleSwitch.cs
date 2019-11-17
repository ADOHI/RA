using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleSwitch : MonoBehaviour
{
    public Material[] skyboxex;
    public bool isPush = false;
    public bool isDay = true;
    Collider col;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Toggle()
    {
        isDay = !isDay;
        if (isDay)
        {
            RenderSettings.skybox = skyboxex[0];
        }
        else
        {
            RenderSettings.skybox = skyboxex[1];
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        isPush = true;
        Toggle();
    }

    private void OnTriggerStay(Collider other)
    {
        transform.localPosition = new Vector3(0f, -0.02f, 0f);
    }

    private void OnTriggerExit(Collider other)
    {
        isPush = false;
    }


}
