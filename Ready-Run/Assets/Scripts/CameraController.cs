using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float smoothness;

    private Vector3 vel = Vector3.zero;

    private void Start()
    {
        target = GameObject.Find("Player").GetComponent<Transform>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 move_pos = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, move_pos, ref vel, smoothness);
    }
}
