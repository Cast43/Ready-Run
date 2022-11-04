using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public float vel;
    public bool InRange = false;
    Rigidbody2D rig;
    void Start()
    {
        rig = transform.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (InRange)
        {
            rig.velocity = Vector2.left * vel;
            Destroy(this.gameObject, 10);

        }

    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.name == "Player")
        {
            InRange = true;
        }
    }
}
