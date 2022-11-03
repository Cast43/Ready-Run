using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : MonoBehaviour
{
    [SerializeField] private enum Mode { Idle, Hunt, Escape};
    [SerializeField] private Mode currentmode;
    [SerializeField] private float fly_distance;
    private Rigidbody2D rb;
    [SerializeField] private float vel = 2f;
    [SerializeField] private float original_pos;
    [SerializeField] private bool going_right = true;
    private SpriteRenderer sprite;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        currentmode = Mode.Idle;
        original_pos = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        CheckMode();
    }

    void CheckMode()
    {
        switch (currentmode)
        {
            case Mode.Idle:
                IdleMove();
                break;
            case Mode.Hunt:
                HuntMove();
                break;
            case Mode.Escape:
                EscapeMove();
                break;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "Player")
        {
            currentmode = Mode.Hunt;
        }
    }
    void IdleMove()
    {
        if(going_right)
        {
            rb.velocity = (new Vector2(vel, 0f));
            sprite.flipX = false;
        }
        else
        {
            rb.velocity = (new Vector2(-vel, 0f));
            sprite.flipX = true;
        }

        if (transform.position.x >= (original_pos + fly_distance))
        {
            going_right = false;
        }
        else if(transform.position.x < original_pos - fly_distance)
        {
            going_right = true;
        }
    }
    void HuntMove()
    {

    }

    void EscapeMove()
    {

    }
}
