using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSpawner : MonoBehaviour
{
    public GameObject buttonPrefab;
    public Queue<GameObject> cubes = new Queue<GameObject>();
    public int buttonAmount;
    public float height = 3f;

    //public int cubeSpawnRate;
    //public float range = 20f;
    //public float force = 20f;

    // Start is called before the first frame update
    void Start()
    {
        for (var i = 0; i < buttonAmount; i++)
        {
            var rv = new Vector3(Random.Range(4500f, 5500f), height, Random.Range(4500f, 5500f));
            var obj = Instantiate(buttonPrefab, rv, new Quaternion(0f, Random.Range(-1f, 1f), 0f, 0f), transform);
            var rd = Random.Range(1f, 10f);
            obj.transform.localScale = new Vector3(rd, rd, rd);
            MaterialPropertyBlock mpb = new MaterialPropertyBlock();
            var renderer = obj.GetComponentInChildren<Renderer>();
            mpb.SetColor(Shader.PropertyToID("_Color"), new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
            renderer.SetPropertyBlock(mpb);
        }
    }

    // Update is called once per frame
}
