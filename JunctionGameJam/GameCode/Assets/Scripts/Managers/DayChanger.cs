using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayChanger : Singleton<DayChanger>
{
    public Material[] materials;
    public int currentIndex;
    public float dayChangeTime = 5f;
    // Start is called before the first frame update
    // Update is called once per frame
    private void Start()
    {
        StartCoroutine(ChangeDay());
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            currentIndex++;
            
        }
        RenderSettings.skybox = materials[currentIndex % 2];
        StoneSpawner.Instance.ShowStones(currentIndex % 2 == 0);
    }

    IEnumerator ChangeDay()
    {
        yield return YieldInstructionCache.WaitForSeconds(dayChangeTime);
        currentIndex++;
    }
}
