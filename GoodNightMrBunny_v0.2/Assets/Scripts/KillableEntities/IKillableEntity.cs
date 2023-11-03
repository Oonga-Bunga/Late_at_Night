using UnityEngine;

public interface IKillableEntity
{
    public void TakeHit(float damage);
    public void ChangeHealth(float value, bool isDamage);
    public void Die();
}
