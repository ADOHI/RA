using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public GameObject line;
    private float timeOffset;
    public int lineNum = 10;
    public float lineLength = 100.0f;
    public float speed = 1.0f;
    //public float offset = 2.0f;
    private LineRenderer[] lines;

    void Awake()
    {
        lines = new LineRenderer[lineNum];

        for (int i = 0; i < lineNum; i++)
        {
            var Obj = Instantiate(line, Vector3.zero, Quaternion.identity);
            Obj.transform.SetParent(transform);
            LineRenderer renderLine = Obj.GetComponent<LineRenderer>();
            for (int j = 0; j < 2; j++)
            {
                renderLine.SetPosition(j, new Vector3(i * (lineLength/lineNum), 0, j * lineLength));
            }
        }

        for (int i = 0; i < lineNum; i++)
        {
            var Obj = Instantiate(line, Vector3.zero, Quaternion.identity);
            Obj.transform.SetParent(transform);
            LineRenderer renderLine = Obj.GetComponent<LineRenderer>();

            for (int j = 0; j < 2; j++)
            {
                renderLine.SetPosition(j, new Vector3(j * lineLength, 0, i * (lineLength / lineNum)));
            }

            lines[i] = renderLine;
        }
    }

    private void Update()
    {
        timeOffset += Time.deltaTime * speed;

        for(int i=0; i<lineNum; i++)
        {
            for(int j=0; j<2; j++)
            {
                lines[i].SetPosition(j, new Vector3(j*lineLength, 0, (i*(lineLength/lineNum) + timeOffset)%lineLength));
            }
        }
    }
}