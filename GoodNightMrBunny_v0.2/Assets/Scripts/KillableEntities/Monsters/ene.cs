/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMonster : AKillableEntity
{
    [SerializeField] private float damage = 3f;
    [SerializeField] private float normalAttackCooldown = 2f;
    private float normalAttackTimer;

    public AMonster(float health) : base(health)
    {
        this.normalAttackTimer = 2f;
    }

    public void Update()
    {
        GetWaypoint();

        normalAttackTimer -= Time.deltaTime;

        AKillableEntity target = GetTarget();
        if (normalAttackTimer <= 0 && target != null)
        {
            normalAttackTimer = normalAttackCooldown;
            NormalAttack(target);
        }
    }

    public void NormalAttack(AKillableEntity target)
    {

    }

    public override void TakeHit(float damage)
    {
        base.TakeHit(damage);
    }

    public override void Die()
    {
        base.Die();
    }

    public Vector3 GetWaypoint()
    {
        return Vector3.zero;
    }

    public AKillableEntity GetTarget()
    {
        return this;
    }
}
*/