using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSpawner : MonoBehaviour
{
    public GameObject cubePrefab;
    public Queue<GameObject> cubes = new Queue<GameObject>();
    public int cubeAmount;
    public int cubeSpawnRate;

    // Start is called before the first frame update
    void Start()
    {
        for (var i = 0; i < cubeAmount; i++)
        {
            
            var obj = Instantiate(cubePrefab);
            
            var rd = Random.Range(1f, 10f);
            var temp = Random.Range(10f, 50f);
            obj.transform.localScale = new Vector3(Random.Range(5f, 20f), temp, Random.Range(5f, 20f));
            var ranVec = new Vector3(Random.Range(4000f, 6000f), temp, Random.Range(4000f, 6000f));
            obj.transform.position = ranVec;
            obj.transform.parent = transform;

            MaterialPropertyBlock mpb = new MaterialPropertyBlock();
            var renderer = obj.GetComponentInChildren<Renderer>();
            mpb.SetColor(Shader.PropertyToID("_Color"), new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
            renderer.SetPropertyBlock(mpb);
        }
       
    }
}
