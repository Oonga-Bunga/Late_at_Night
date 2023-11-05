using UnityEngine;

public interface IKillableEntity
{
    public enum AttackSource
    {
        Flashlight,
        ClayBall,
        Rocket,
        Shadow,
        KitestingerTrap,
        EvilBunny,
        Cat
    }

    public void TakeHit(float damage, AttackSource source);
    public void ChangeHealth(float value, bool isDamage);
    public void Die();
}
