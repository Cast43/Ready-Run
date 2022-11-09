using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotController : MonoBehaviour
{
    public GameObject player;
    public Rigidbody2D player_rb;
    public Rigidbody2D rb;
    public Vector2 target;
    public Transform point_pos;
    public float speed;
    public float detect_dist;
    void Start()
    {
        player = GameObject.Find("Player");
        player_rb = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        rb = GetComponent<Rigidbody2D>();
        point_pos = GetComponentInChildren<Transform>();

        target = new Vector2(player.transform.position.x + player_rb.velocity.x / 4, player.transform.position.y);

        Vector2 distance = target - rb.position;
        rb.velocity = distance.normalized * speed;
    }

    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(point_pos.position, rb.position - target, detect_dist);
        if (hit.transform.tag == "Player")
        {
            Debug.Log("Poggers");
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

}
