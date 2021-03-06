using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Entities;

public class VoxelInfo : MonoBehaviour
{
    private Text title;
    private Text color;
    private Text value;
    private Text annotations;

    void Start()
    {
        title       = transform.Find("Title").GetComponent<Text>();
        color       = transform.Find("Color").GetComponent<Text>();
        value       = transform.Find("Value").GetComponent<Text>();
        annotations = transform.Find("Annotations").GetComponent<Text>();
    }

    public void FillInfo(Vector3 pos, float4 c, float val, int4 id) // DynamicBuffer<BufferInt> ids)
    {
        title.text = pos.ToString();
        color.text = "Color : " + (255 * c).ToString();
        value.text = "Value : " + val.ToString();

        annotations.text = "";
        for (int i = 0; i < 4; i++)
        {
            if (id[i] >= 0)
            {
                annotations.text += DictationEngine.GetRecorded(id[i]) + "\n";
            }
        }
    }
}
