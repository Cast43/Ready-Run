using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public LayerMask ground_layer;
    public Rigidbody2D rb;
    public Animator anim;
    public SpriteRenderer sprite;
    public CapsuleCollider2D hitbox;

    [Header("Movement")]
    [SerializeField] private float accel;
    [SerializeField] private float max_speed;
    [SerializeField] private float linear_drag;
    [SerializeField] private bool changing_dir => (rb.velocity.x > 0f && horiz_move < 0f || rb.velocity.x < 0f && horiz_move > 0f);
    [SerializeField] private Transform ground_check;
    private float horiz_move;
    private float vert_move;

    [Header("Jump")]
    [SerializeField] private float jump_power;
    [SerializeField] private float coyote_time = 0.2f;
    [SerializeField] private float jump_buffer_time = 0.2f;
    private bool is_jumping = false;
    private float coyote_time_count;
    private float jump_buffer_counter;

    [Header("Slide")]
    [SerializeField] private float slide_power;
    [SerializeField] private float slide_time = 0.2f;
    [SerializeField] private float slide_cooldown = 1f;
    private bool can_slide = true;
    private bool is_sliding;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        hitbox = GetComponent<CapsuleCollider2D>();
    }

    private void Update()
    {
        if (is_sliding)
        {
            return;
        }

        //Flip char
        Flip();

        //Get direction
        horiz_move = GetInput().x;
        vert_move = GetInput().y;

        //Jump control
        CoyoteControl();
        BufferControl();
        JumpControl();

        //Slide control
        SlideInput();

    }

    private void SlideInput()
    {
        if (Input.GetButtonDown("Slide") && can_slide && Mathf.Abs(horiz_move) > 0f && IsGrounded())
        {
            StartCoroutine(Slide());
        }
    }

    private void JumpControl()
    {

        if (coyote_time_count > 0f && jump_buffer_counter > 0f && is_jumping == false)
        {

            //Animation Variables
            anim.SetBool("Jump", true);

            rb.velocity = new Vector2(rb.velocity.x, jump_power);

            jump_buffer_counter = 0f;

            StartCoroutine(JumpCooldown());

        }


        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);

            coyote_time_count = 0f;

        }
    }

    private void BufferControl()
    {
        if (Input.GetButtonDown("Jump"))
        {
            jump_buffer_counter = jump_buffer_time;
        }
        else
        {
            jump_buffer_counter -= Time.deltaTime;
        }
    }

    private void CoyoteControl()
    {
        if (IsGrounded())
        {
            rb.drag = 8f;
            coyote_time_count = coyote_time;
        }
        else
        {
            coyote_time_count -= Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        if (is_sliding)
        {
            return;
        }

        MoveChar();
        ApplyLinDrag();

        //Animation Variables
        anim.SetInteger("VelocityX", (int)horiz_move);
        anim.SetInteger("VelocityY", (int)rb.velocity.y);
        if (IsGrounded())
        {
            anim.SetBool("NotGrounded", false);
        }
        else
        {
            anim.SetBool("NotGrounded", true);
        }

    }

    private Vector2 GetInput()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(ground_check.position, 0.1f, ground_layer);
    }

    private IEnumerator JumpCooldown()
    {
        is_jumping = true;
        yield return new WaitForSeconds(0.4f);
        is_jumping = false;
        anim.SetBool("Jump", false);


    }

    private IEnumerator Slide()
    {
        can_slide = false;
        is_sliding = true;

        float grav = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * slide_power, 0f);

        //hitbox.size = new Vector2(hitbox.size.x, 0.2813561f);
        //hitbox.offset = new Vector2(hitbox.offset.x, -0.3264888f);
        //Animation Variables
        anim.SetBool("Slide", true);

        yield return new WaitForSeconds(slide_time);

        //hitbox.size = new Vector2(hitbox.size.x, 0.9004961f);
        //hitbox.offset = new Vector2(hitbox.offset.x, -0.04975197f);
        rb.gravityScale = grav;
        //Animation Variables
        anim.SetBool("Slide", false);

        is_sliding = false;

        yield return new WaitForSeconds(slide_cooldown);
        can_slide = true;
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
        if ((Mathf.Abs(horiz_move) < 0.1f || changing_dir) && is_jumping == false && IsGrounded())
        {
            rb.drag = linear_drag;
        }
        else
        {
            rb.drag = 1f;
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

