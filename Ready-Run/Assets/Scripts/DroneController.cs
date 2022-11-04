using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : MonoBehaviour
{
    [SerializeField] private enum Mode { Idle, Hunt, Escape};
    [SerializeField] private Mode currentmode;
    [SerializeField] private float fly_distance;
    [SerializeField] private GameObject shot;
    [SerializeField] private float vel = 2f;
    [SerializeField] private float original_pos;
    [SerializeField] private bool going_right = true;
    [SerializeField] private float shot_cooldown = 3f;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        player = GameObject.Find("Player").gameObject;
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

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            currentmode = Mode.Escape;
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
        else if(transform.position.x < (original_pos - fly_distance))
        {
            going_right = true;
        }
    }
    void HuntMove()
    {
        rb.velocity = (new Vector2(0f, 0f));
        transform.position = new Vector3(Mathf.Lerp(transform.position.x, player.transform.position.x, 0.015f), Mathf.Lerp(transform.position.y, player.transform.position.y + 1f, 0.02f), transform.position.z);
        Shoot();
    }
    
    void Shoot()
    {
        shot_cooldown -= Time.deltaTime;
        if(shot_cooldown <= 0f)
        {
            shot_cooldown = 3f;
            GameObject shot_instance = Instantiate(shot, transform.position, Quaternion.identity);
        }
    }

    void EscapeMove()
    {

    }
}
