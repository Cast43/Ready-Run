using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotController : MonoBehaviour
{
    public GameObject player;
    public Rigidbody2D player_rb;
    public Rigidbody2D rb;

    public Vector2 target;
    public float speed = 50f;

    void Start()
    {
        player = GameObject.Find("Player");
        player_rb = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        rb = GetComponent<Rigidbody2D>();

        target = new Vector2(player.transform.position.x + player_rb.velocity.x/4, player.transform.position.y);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 newPosition = Vector2.MoveTowards(transform.position, target, Time.fixedDeltaTime * speed);
        rb.MovePosition(newPosition);
    }
}
