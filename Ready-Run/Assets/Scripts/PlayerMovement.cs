using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("General")]
    public LayerMask ground_layer;
    public bool is_paused = false;
    public GameObject menu_pause;
    public Vector3 vel_men;
    public float t;
    public Rigidbody2D rb;
    public Animator anim;
    public SpriteRenderer sprite;
    public CapsuleCollider2D hitbox;

    [Header("Slopes")]
    [SerializeField] private float slope_check_dist;
    [SerializeField] private float ground_check_dist;
    [SerializeField] private PhysicsMaterial2D no_fric;
    [SerializeField] private PhysicsMaterial2D fric;
    [SerializeField] private PhysicsMaterial2D semi_fric;
    private Vector2 perp;
    private float slope_ang;
    [SerializeField] private bool is_on_slope;
    [SerializeField] private bool just_entered_slope;


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
    [SerializeField] private bool is_jumping = false;
    [SerializeField] private bool jump_ground = false;
    private float coyote_time_count;
    private float jump_buffer_counter;

    [Header("Dash")]
    [SerializeField] private float dash_power;
    [SerializeField] private float max_speed_dash = 10f;
    [SerializeField] private float dash_time = 0.2f;
    [SerializeField] private float dash_cooldown = 1f;
    [SerializeField] private bool can_dash = true;
    [SerializeField] private bool is_dashing;

    [Header("Slide")]
    [SerializeField] private bool is_sliding;
    [SerializeField] private bool holding_ctrl;
    [SerializeField] private float linear_drag_slide;
    [SerializeField] private float max_speed_slide;
    [SerializeField] private float accel_slide;
    private float old_lin_drag_slide;

    [Header("Stun")]
    [SerializeField] private bool stunned;
    [SerializeField] private float timeStun;




    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        hitbox = GetComponent<CapsuleCollider2D>();
        old_lin_drag_slide = linear_drag_slide;
        menu_pause = GameObject.Find("Loader/Fade/PauseMenu");
    }

    private void Update()
    {

        //Controla o pause
        PauseControl();

        if (is_paused)
        {
            Time.timeScale = Mathf.Lerp(Time.timeScale, 0f, 0.025f);
            MenuGoUp();
        }
        else
        {
            Time.timeScale = Mathf.Lerp(Time.timeScale, 1f, 0.025f);
            MenuGoDown();
        }

        if (is_dashing || is_paused)
        {
            return;
        }

        //Flip char
        Flip();

        //Get direction
        if (!stunned)
        {
            horiz_move = GetInput().x;
            vert_move = GetInput().y;
        }
        else
        {
            horiz_move = 0;
        }

        //Jump control
        CoyoteControl();
        BufferControl();
        JumpControl();

        //Dash control
        DashInput();

        //Slide control
        SlideInput();
        SlideControl();

        //Controlar descidas anguladas
        SlopeCheck();
    }

    private void PauseControl()
    {
        if (Input.GetButtonDown("Pause"))
        {
            is_paused = !is_paused;
        }
    }

    private void MenuGoUp()
    {
        menu_pause.transform.position = Vector3.Lerp(menu_pause.transform.position, Vector3.zero, 0.05f);
    }

    private void MenuGoDown()
    {
        menu_pause.transform.position = Vector3.Lerp(menu_pause.transform.position, new Vector3(0f, -900f, 0f), 0.05f);
    }

    private void SlideControl()
    {
        if (is_sliding)
        {
            if (anim.GetBool("Slide") == false)
            {
                anim.SetTrigger("SlidePrep");

            }
            anim.SetBool("Slide", true);

        }
        else
        {
            anim.SetBool("Slide", false);
        }

    }


    private void SlideInput()
    {
        if (Input.GetButton("Slide") && (IsGrounded() || is_on_slope))
        {
            is_sliding = true;
        }
        if (Input.GetButton("Slide"))
        {
            holding_ctrl = true;
        }

        if (Input.GetButtonUp("Slide"))
        {
            is_sliding = false;
            holding_ctrl = false;
            linear_drag_slide = old_lin_drag_slide;
        }
    }

    private void DashInput()
    {
        if (Input.GetButtonDown("Dash") && can_dash && Mathf.Abs(horiz_move) > 0f && !is_on_slope && !is_sliding)
        {
            StartCoroutine(Dash());
        }
    }

    private void JumpControl()
    {
        if (coyote_time_count > 0f && jump_buffer_counter > 0f && is_jumping == false && !is_sliding)
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
        MoveChar();
        ApplyLinDrag();

        //Animation Variables
        anim.SetInteger("VelocityX", (int)horiz_move);
        anim.SetInteger("VelocityY", (int)rb.velocity.y);
        if (IsGrounded())
        {
            jump_ground = false;
            anim.SetBool("NotGrounded", false);
        }
        else
        {
            jump_ground = true;
            anim.SetBool("NotGrounded", true);
        }

    }

    private void SlopeCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, slope_check_dist, ground_layer);

        if (hit)
        {
            perp = Vector2.Perpendicular(hit.normal).normalized;

            slope_ang = Vector2.Angle(hit.normal, Vector2.up);
            is_on_slope = slope_ang != 0;
            Debug.DrawRay(hit.point, perp, Color.red);
            Debug.DrawRay(hit.point, rb.velocity / 4, Color.green);
        }

        if (!IsGrounded())
        {
            is_on_slope = false;
        }

        if (!is_sliding && is_on_slope && horiz_move == 0 && (transform.localScale.x > 0f || transform.localScale.x < 0f))
        {
            rb.sharedMaterial = fric;
        }
        else if (is_sliding && is_on_slope && horiz_move < 0)
        {
            rb.sharedMaterial = semi_fric;
        }
        else if (!is_sliding && (horiz_move < 0 || horiz_move > 0))
        {
            rb.sharedMaterial = no_fric;
        }
        else
        {
            rb.sharedMaterial = no_fric;
        }
    }

    private Vector2 GetInput()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    private bool IsGrounded()
    {
        RaycastHit2D rc = Physics2D.Raycast(ground_check.position, Vector3.down, ground_check_dist, ground_layer);
        Debug.DrawRay(rc.point, new Vector2(0f, -ground_check_dist));
        return Physics2D.Raycast(ground_check.position, Vector3.down, ground_check_dist, ground_layer);
    }

    private IEnumerator JumpCooldown()
    {
        is_jumping = true;
        yield return new WaitForSeconds(0.4f);
        is_jumping = false;
        anim.SetBool("Jump", false);
    }

    private IEnumerator Dash()
    {
        can_dash = false;
        is_dashing = true;

        float grav = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dash_power, 0f);

        anim.SetBool("Dash", true);

        yield return new WaitForSeconds(dash_time);

        anim.SetBool("Dash", false);

        rb.gravityScale = grav;

        is_dashing = false;

        yield return new WaitForSeconds(dash_cooldown);
        can_dash = true;
    }

    private void MoveChar()
    {
        if (!is_on_slope)
        {
            just_entered_slope = true;
        }

        if (holding_ctrl && IsGrounded())
        {
            is_sliding = true;
        }

        if (is_on_slope && !jump_ground && !is_jumping && is_sliding && transform.localScale.x > 0f)
        {
            StartCoroutine(JESCont());
            if (just_entered_slope)
            {
                rb.velocity = new Vector2(-horiz_move * perp.x * max_speed, -horiz_move * perp.y * max_speed);
            }
            if (horiz_move != 0)
            {
                rb.AddForce(new Vector2(-horiz_move * perp.x * accel_slide, -horiz_move * perp.y * accel_slide));
            }
            else
            {
                rb.AddForce(new Vector2(2 * perp.x * -accel_slide, 2 * perp.y * -accel_slide));
            }

            if (Mathf.Abs(rb.velocity.x) > max_speed_slide)
            {
                rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * max_speed_slide, rb.velocity.y);
            }
        }
        else if (is_on_slope && !jump_ground && !is_jumping && is_sliding && transform.localScale.x < 0f)
        {
            StartCoroutine(JESCont());
            if (just_entered_slope)
            {
                rb.velocity = new Vector2(-horiz_move * perp.x * max_speed, -horiz_move * perp.y * max_speed);
            }

            rb.AddForce(new Vector2(horiz_move * perp.x * accel_slide, horiz_move * perp.y * accel_slide));

            if (Mathf.Abs(rb.velocity.x) > max_speed_slide)
            {
                rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * max_speed_slide, rb.velocity.y);
            }
        }
        else if (is_on_slope && !jump_ground && !is_jumping)
        {
            StartCoroutine(JESCont());
            if (just_entered_slope)
            {
                rb.velocity = new Vector2(-horiz_move * perp.x * max_speed, -horiz_move * perp.y * max_speed);
            }
            rb.AddForce(new Vector2(-horiz_move * perp.x * accel, -horiz_move * perp.y * accel));
            if (Mathf.Abs(rb.velocity.x) > max_speed)
            {
                rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * max_speed, rb.velocity.y);
            }
        }
        else
        {
            rb.AddForce(new Vector2(horiz_move, 0f) * accel);
            if (Mathf.Abs(rb.velocity.x) > max_speed && !is_dashing)
            {
                rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * max_speed, rb.velocity.y);
            }
            else if (is_dashing)
            {
                rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * max_speed_dash, rb.velocity.y);
            }
        }

    }
    private IEnumerator JESCont()
    {
        yield return new WaitForSeconds(0.1f);
        just_entered_slope = false;
    }
    private void ApplyLinDrag()
    {
        if ((Mathf.Abs(horiz_move) < 0.1f || changing_dir) && is_jumping == false && IsGrounded())
        {
            rb.drag = linear_drag;
        }
        else if (is_sliding && !is_on_slope && IsGrounded())
        {
             StartCoroutine(LinDragSlide());
        }
        else if (is_sliding && !IsGrounded())
        {
            rb.drag = 1f;
        }
        else
        {
            rb.drag = 1f;
        }
    }

    private IEnumerator LinDragSlide()
    {
        rb.drag = linear_drag_slide;
        yield return new WaitForSeconds(0.1f);
        linear_drag_slide += 0.2f;
    }

    private void Flip()
    {

        if (is_on_slope && IsGrounded())
        {
            sprite.transform.eulerAngles = Vector3.forward * -slope_ang;
        }
        else
        {
            sprite.transform.eulerAngles = Vector3.zero;
        }

        if (horiz_move > 0f)
        {
            transform.localScale = new Vector3(1f, transform.localScale.y, transform.localScale.z);
        }
        else if (horiz_move < 0f)
        {
            transform.localScale = new Vector3(-1f, transform.localScale.y, transform.localScale.z);
        }
    }
    public IEnumerator Stun()
    {
        stunned = true;
        anim.SetBool("Stun", true);
        sprite.color = Color.red;
        yield return new WaitForSeconds(timeStun);
        stunned = false;
        anim.SetBool("Stun", false);
        sprite.color = Color.white;

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Drone" && is_dashing)
        {
            collision.gameObject.GetComponent<CircleCollider2D>().enabled = false;
            collision.gameObject.GetComponent<Animator>().SetBool("Death",true);
            is_dashing = false;
            Debug.Log("explode"); 
            rb.velocity = new Vector2(rb.velocity.x+2,rb.velocity.y+7);
            Destroy(collision.gameObject,5);
        }
    }
}

