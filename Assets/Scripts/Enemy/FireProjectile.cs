using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectile : MonoBehaviour
{
    //holds gameobject from inspector that will later be added to the game
    public GameObject projectile;

    //gives the enemy a way to get info about the player
    private GameObject player;

    //holds the transform/location based on gameobject passed from inspector. This will be where the projectile is spawned in
    public Transform projectilePos;

    //how long before the 
    public float startTimer = 0;

    //can set the frequency the enemy fires/shoots the projectiles at the player from the inspector
    public float fireInterval = 0;

    //the set distance the player can be from the enemy before it starts shooting
    public float playerdistance = 0;

    //used to make sure the playerdistance is being used for the current enemy
    private bool trackdistance = false;

    //used to check how much time is passed before next projectile is fired
    private float projectiletimer = 0;

    //used to check if the enemy can start firing projectiles
    private bool startFire = false;

    //used to track how much time has passed before enemy can start firing projectiles
    private float timer = 0;

    void Start()
    {
        //Once the enemy spawns in, it gets access to the player GameObject through its tag set in Unity
        player = GameObject.FindGameObjectWithTag("Player");

        //checks if the player distance is being used and if the enemy should be tracking how far away the player is
        if (playerdistance > 0)
        {
            trackdistance = true;
        }
    }

    void Update()
    {
        FireStart();
    }

    void FireStart()
    {
        if (startFire == false)
        {
            timer += Time.deltaTime;
            if (timer >= startTimer)
            {
                startFire = true;
                FireStart();
            }
            else
            {
                return;
            }
        }
        else if (startFire == true)
        {
            FireDistance();
        }
        
        
    }
    void FireDistance()
    {
        //if tracking the player's location is being used, then the player's distance from enemy will be determined. If not, the enemy will fire normally
        if (trackdistance == true)
        {
            //figures out the distance between the player 
            float distance = Vector2.Distance(transform.position, player.transform.position);

            //checks if the player is within distance to fire
            if (distance >= playerdistance)
            {
                FireInterval();
            }
        }
        else
        {
            FireInterval();
        }
    }

    //function that handles the interval at which projectiles are fired 
    void FireInterval()
    {
        //checks if the fireInterval has been set to make sure whether the enemy should be firing at the player at this time
        if (fireInterval > 0)
        {
            //keeps track of how much time has passed
            projectiletimer += Time.deltaTime;

            //checks if enough time has passed before the enemy fires another projectile
            if (projectiletimer > fireInterval)
            {
                //resets the timer, so it can start tracking when the next projectile should be fired
                projectiletimer = 0;

                //calls the function that adds the projectile to the game
                Fire();
            }
        }
    }

    //Function that add a new projectile
    void Fire()
    {
        //adds the projectile to the game
        Instantiate(projectile, projectilePos.position, Quaternion.identity);
    }

    void StartDistance()
    {
        //figures out the distance between the player 
        float distance = Vector2.Distance(transform.position, player.transform.position);

        //checks if the player is within distance to fire
        if (distance >= playerdistance)
        {
            Fire();
        }
    }
}
