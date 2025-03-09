using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    //Inspector Variables
    public GameObject player;
    
    public float force;
    
    public float tracking = 0;
    
    public float destroy = 5;

    //Script Only Variables
    private Rigidbody2D rb;

    private bool followplayer = false;
    
    private float tracktimer = 0;
    
    private float dtimer = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        player = GameObject.FindGameObjectWithTag("Player");

        FindPlayer();

        if (tracking > 0)
        {
            followplayer = true;
        }
    }

    void Update()
    {
        FollowPlayer();
        DeleteTimer();
    }

    //after the projectile is on screen for a certain amount of time, it will destroy itself
    void DeleteTimer()
    {
        dtimer += Time.deltaTime;
        if (dtimer > destroy)
        {
            Destroy(gameObject);
        }
    }

    //determines if the player is being followed and updates the bullets movement thusly
    void FollowPlayer()
    {
        if (followplayer == true)
        {
            if (tracktimer < tracking)
            {
                tracktimer += Time.deltaTime;
                FindPlayer();
            }
        }
    }

    //finds the player's location and moves the bullet towards the player
    void FindPlayer()
    {
        Vector3 direction = player.transform.position - transform.position;
        rb.linearVelocity = new Vector2(direction.x, direction.y).normalized * force;
        float rotation = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotation + 90);
    }

    //handles what happens when the projectile collides with the player
    void OnTriggerEnter2D(Collider2D other)
    {
        //checks to see if what the projectile is colliding with is actually the player
        if (other.gameObject.CompareTag("Player"))
        {
            //still needs line of code that accesses player timer
            
            //destroys projectile
            Destroy(gameObject);
        }
    }
}
