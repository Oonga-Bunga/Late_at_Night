using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Baby : AKillableEntity
{
    private static Baby _instance;

    public static Baby Instance => _instance;

    public Baby(float health) : base(health)
    {
        
    }

    // Start is called before the first frame update
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        this.currentHealth = maxHealth;
        this.hitbox = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
