using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public float vel;
    Rigidbody2D rig;
    void Start()
    {
        vel = Random.Range(4,8);
        rig = transform.GetComponent<Rigidbody2D>();
        Destroy(this.gameObject,5);
    }

    void Update()
    {
        rig.velocity = Vector2.right*-vel;
    }
}
