using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length, start_pos;
    public GameObject cam;
    public float para_amount;
    // Start is called before the first frame update
    void Start()
    {
        start_pos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        float temp = cam.transform.position.x * (1 - para_amount);

        float dist = cam.transform.position.x * para_amount;
        transform.position = new Vector3(start_pos + dist, transform.position.y, transform.position.z);

        if(temp > start_pos + length)
        {
            start_pos += length;
        }
        else if(temp < start_pos - length)
        { 
            start_pos -= length;
        }
    }
}
