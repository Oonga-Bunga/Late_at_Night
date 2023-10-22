using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baby : AKillableEntity
{
    public Baby(float health) : base(health)
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
        this.currentHealth = maxHealth;
        this.hitbox = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void Die(){}
}
