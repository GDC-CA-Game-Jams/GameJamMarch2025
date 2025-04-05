using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    //Inspector Variables
    public GameObject player;

    public Animator playerAnim;

    public Timer gameTimer;

    public float force;
    
    public float tracking = 0;
    
    public float destroy = 5;

    public float damage = 3;

    public AudioClip playerHitSound;

    //Script Only Variables
    private Rigidbody2D rb;

    private bool followplayer = false;
    
    private float tracktimer = 0;
    
    private float dtimer = 0;

    private AudioSource audioSource;

    private string dead = "Destroy";

    Animator anim;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player");
        gameTimer = GameObject.FindGameObjectWithTag("Timer").GetComponent<Timer>();
        anim = GetComponent<Animator>();
        playerAnim = player.GetComponent<Animator>();

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
            rb.linearVelocity = Vector3.zero;
            anim.Play(dead);
            //Destroy(gameObject);
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
            //Debug.Log("player hit");

            //play sound
            audioSource.PlayOneShot(playerHitSound);

            //decreasing the time remaining
            gameTimer.Damage(damage);

            //stops projectile movement
            rb.linearVelocity = Vector3.zero;

            playerAnim.Play("Hit");

            //plays destroy animation
            anim.Play(dead);

            //destroys projectile
            //Destroy(gameObject);
        }
    }

    void OnDestroyAnimationFinish()
    {
        Destroy(gameObject);
    }
}
