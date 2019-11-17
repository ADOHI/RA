using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    public GameObject[] treePrefab;
    public int treeAmount;
    public float height = 3f;

    // Start is called before the first frame update
    void Start()
    {
        for (var i = 0; i < treeAmount; i++)
        {
            var rd = Random.Range(0, 3);
            var rv = new Vector3(Random.Range(4500f, 5500f), height, Random.Range(4500f, 5500f));

            var obj = Instantiate(treePrefab[rd]);
            obj.transform.position = rv;
            var rd2 = Random.Range(200f, 1000f);
            obj.transform.localScale = new Vector3(rd2, rd2, rd2);
            obj.transform.parent = transform;
            var rt = obj.GetComponent<Rotation>();
            rt.speed = Random.Range(0.5f, 1f);

        }
        
    }


}
