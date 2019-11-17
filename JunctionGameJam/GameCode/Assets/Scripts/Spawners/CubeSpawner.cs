using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    public GameObject cubePrefab;
    public Queue<GameObject> cubes = new Queue<GameObject>();
    public int cubeAmount;
    public int cubeSpawnRate;
    public float range = 20f;
    public float force = 20f;

    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (cubes.Count > cubeAmount)
        {
            DestroyImmediate(cubes.Dequeue());
        }
        else
        {
            for (var i = 0; i < cubeSpawnRate; i++)
            {
                var ranVec = new Vector3(Random.Range(-range, range), Random.Range(-range, range), Random.Range(-range, range));
                var obj = Instantiate(cubePrefab, transform.position + ranVec, Random.rotation, transform);
                var rd = Random.Range(1f, 10f);
                obj.transform.localScale = new Vector3(rd, rd, rd);
                MaterialPropertyBlock mpb = new MaterialPropertyBlock();
                var renderer = obj.GetComponentInChildren<Renderer>();
                mpb.SetColor(Shader.PropertyToID("_Color"), new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
                renderer.SetPropertyBlock(mpb);
                var rb = obj.GetComponent<Rigidbody>();
                rb.AddRelativeForce(ranVec * force);
                rb.AddRelativeTorque(ranVec * force);
                cubes.Enqueue(obj);
             }
        }
        
    }
}
