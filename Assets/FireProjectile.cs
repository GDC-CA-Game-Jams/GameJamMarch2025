using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectile : MonoBehaviour
{
    public GameObject projectile;
    public Transform projectilePos;

    [Serialized Field] public float timer;
    private float clock;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if timer <= 0
        {
            timer += timer.deltaTime;
        }
    }
}
