using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public LayerMask mask = 6;
    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sprite;

    [Header("Movement")]
    [SerializeField] private float accel;
    [SerializeField] private float max_speed;
    [SerializeField] private float linear_drag;
    [SerializeField] private bool changing_dir => (rb.velocity.x > 0f && horiz_move < 0f || rb.velocity.x < 0f && horiz_move > 0f);
    private float horiz_move;
    private float vert_move;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Flip();

        horiz_move = GetInput().x;
        vert_move = GetInput().y;   


    }

    private Vector2 GetInput()
    {
        return new Vector2 (Input.GetAxisRaw("Horizontal") , Input.GetAxisRaw("Vertical"));
    }



    void FixedUpdate()
    {
        MoveChar();
        ApplyLinDrag();
    }

    private void MoveChar()
    {
        rb.AddForce(new Vector2(horiz_move, 0f) * accel);

        if (Mathf.Abs(rb.velocity.x) > max_speed)
        {
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * max_speed, rb.velocity.y);
        }
    }

    private void ApplyLinDrag()
    {
        if(Mathf.Abs(horiz_move) < 0.4f || changing_dir)
        {
            rb.drag = linear_drag;
        }
        else
        {
            rb.drag = 0f;
        }
    }

    private void Flip()
    {
        if (horiz_move > 0f)
        {
            transform.localScale = new Vector3(1f, transform.localScale.y, transform.localScale.z);
        }
        else if (horiz_move < 0f)
        {
            transform.localScale = new Vector3(-1f, transform.localScale.y, transform.localScale.z);
        }
    }
}

