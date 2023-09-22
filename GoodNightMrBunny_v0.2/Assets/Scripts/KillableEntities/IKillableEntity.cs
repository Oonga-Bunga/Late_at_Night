using UnityEngine;

public interface IKillableEntity
{
    public void ChangeHealth(float value, bool isDamage);
    public void Die();
}
