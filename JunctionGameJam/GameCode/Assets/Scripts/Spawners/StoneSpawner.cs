using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoneSpawner : Singleton<StoneSpawner>
{
    public GameObject dayStone;
    public GameObject nightStone;
    public List<GameObject> dayStones = new List<GameObject>();
    public List<GameObject> nightStones = new List<GameObject>();
    public Text text;
    public int stoneAmount = 10;
    public float height = 3f;

    //public int cubeSpawnRate;
    //public float range = 20f;
    //public float force = 20f;

    // Start is called before the first frame update
    void Start()
    {
        for (var i = 0; i < stoneAmount / 2; i++)
        {
            var rv = new Vector3(Random.Range(4500f, 5500f), height, Random.Range(4500f, 5500f));
            var obj = Instantiate(dayStone, rv, new Quaternion(0f, Random.Range(-1f, 1f), 0f, 0f), transform);
            var rd = Random.Range(20f, 30f);
            obj.transform.localScale = new Vector3(rd, rd, rd);
            MaterialPropertyBlock mpb = new MaterialPropertyBlock();
            var renderer = obj.GetComponentInChildren<Renderer>();
            mpb.SetColor(Shader.PropertyToID("_Color"), new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
            renderer.SetPropertyBlock(mpb);
            dayStones.Add(obj);
        }

        for (var i = 0; i < stoneAmount / 2; i++)
        {
            var rv = new Vector3(Random.Range(4500f, 5500f), height, Random.Range(4500f, 5500f));
            var obj = Instantiate(nightStone, rv, new Quaternion(0f, Random.Range(-1f, 1f), 0f, 0f), transform);
            var rd = Random.Range(80f, 160f);
            obj.transform.localScale = new Vector3(rd, rd, rd);
            MaterialPropertyBlock mpb = new MaterialPropertyBlock();
            var renderer = obj.GetComponentInChildren<Renderer>();
            mpb.SetColor(Shader.PropertyToID("_Color"), new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
            renderer.SetPropertyBlock(mpb);
            obj.SetActive(false);
            nightStones.Add(obj);
        }
    }
    public void Update()
    {
        text.text = InputQueueManager.Instance.score + " / " + InputQueueManager.Instance.finalScore;
    }
    public void ShowStones(bool day)
    {
        foreach (var obj in dayStones)
        {
            obj.SetActive(day);
        }
        foreach (var obj in nightStones)
        {
            obj.SetActive(!day);
        }
    }
}
