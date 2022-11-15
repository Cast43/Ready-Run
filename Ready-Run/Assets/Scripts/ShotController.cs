using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotController : MonoBehaviour
{
    public GameObject player;
    public Rigidbody2D player_rb;
    public Rigidbody2D rb;
    public Vector2 target;
    public float speed;
    public float detect_dist;
    void Start()
    {
        player = GameObject.Find("Player");
        player_rb = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        rb = GetComponent<Rigidbody2D>();

        target = new Vector2(player.transform.position.x + player_rb.velocity.x / 4, player.transform.position.y);

        Vector2 distance = target - rb.position;
        rb.velocity = distance.normalized * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "Player")
        {
            Debug.Log("Pog");
            collision.GetComponent<PlayerMovement>().StartCoroutine("Stun");
            Destroy(gameObject);
        }

        if (collision.tag == "Ground")
        {
            Destroy(gameObject, 0.05f);
        }
    }
}
