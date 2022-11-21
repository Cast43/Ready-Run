using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : MonoBehaviour
{
    [SerializeField] private enum Mode { Idle, Hunt, Escape };
    [SerializeField] private Mode currentmode;
    [SerializeField] private float fly_distance;
    [SerializeField] private Transform shot_spawn;
    [SerializeField] private GameObject shot;
    [SerializeField] private float vel = 2f;
    [SerializeField] private float original_pos;
    [SerializeField] private bool going_right = true;
    [SerializeField] private float shot_cooldown = 3f;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private GameObject player;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        shot_spawn = GetComponentsInChildren<Transform>()[1];
        player = GameObject.Find("Player").gameObject;
        currentmode = Mode.Idle;
        original_pos = transform.position.x;
    }

    // Update is called once per frame
    void FixedUpdate()
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
        if (collision.name == "Player")
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
        if (going_right)
        {
            rb.velocity = (new Vector2(vel, 0f));

        }
        else
        {
            rb.velocity = (new Vector2(-vel, 0f));


        }

        if (transform.position.x >= (original_pos + fly_distance))
        {
            going_right = false;
            anim.SetBool("Flip", true);


        }
        else if (transform.position.x < (original_pos - fly_distance))
        {
            going_right = true;
            anim.SetBool("Flip", true);
        }
    }
    void HuntMove()
    {
        rb.velocity = (new Vector2(0f, 0f));
        transform.position = new Vector3(Mathf.Lerp(transform.position.x, player.transform.position.x, 1f * Time.deltaTime), Mathf.Lerp(transform.position.y, player.transform.position.y + 1f, 1.2f * Time.deltaTime), transform.position.z);
        Shoot();
    }

    void Shoot()
    {
        shot_cooldown -= Time.deltaTime;

        // ROTACAO NO TIRO
        Vector3 distance = player.transform.position - transform.position;
        float angle = Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg - 180;
        Quaternion ToRotate = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, ToRotate, 1.2f * Time.deltaTime);

        if (shot_cooldown <= 0f)
        {
            anim.SetTrigger("Shoot");
            shot_cooldown = 3f;
        }
    }
    void InstantiateShoot()
    {
        Vector3 distance = transform.position - player.transform.position;
        float angle = Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg - 180;
        Quaternion ToRotate = Quaternion.AngleAxis(angle, Vector3.forward);

        GameObject shot_instance = Instantiate(shot, shot_spawn.position, ToRotate);
        Destroy(shot_instance, 5);

    }

    void EscapeMove()
    {

    }
    public IEnumerator FlipAnim()
    {
        anim.SetBool("Flip", false);

        yield return new WaitForSeconds(0);
        if (going_right)
        {
            sprite.flipX = false;
        }
        else
        {
            sprite.flipX = true;
        }
    }

}
