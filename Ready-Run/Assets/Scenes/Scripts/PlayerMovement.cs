using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float maxVel = 10;
    public float velJump = 10;
    public float JumpForce = 10;
    bool canJump = false;
    public LayerMask mask = 6;
    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sprite;

    void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        anim = transform.GetComponent<Animator>();
        sprite = transform.GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        GetAlign();

        Vector2 direction = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
        if (direction.x > 0)
        {
            sprite.flipX = false;
        }
        else if (direction.x < 0)
        {
            sprite.flipX = true;
        }
        anim.SetInteger("Velocity", (int)direction.x * (int)maxVel);
        rb.velocity += direction * maxVel * Time.deltaTime;

        if (rb.velocity.magnitude > maxVel)
        {
            rb.velocity = rb.velocity.normalized * 5;
        }
        if (canJump)
        {
        }

        //print(rb.velocity);

        if (Input.GetButton("Jump") && canJump)
        {
            rb.velocity = (rb.velocity + new Vector2(0, JumpForce)) * Time.deltaTime * velJump;
            canJump = false;
        }

    }
    void GetAlign()
    {
        Debug.DrawLine(transform.position, -Vector3.up + transform.position, Color.black, 1);
        RaycastHit2D hit = Physics2D.Linecast(transform.position, -Vector3.up + transform.position, 1);
        if (hit)
        {
            Debug.Log(hit.collider.transform.position);
        }

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == 6)
        {

            Vector3.Slerp(collider.transform.position, transform.position, 2);
            canJump = true;
        }
    }
}

