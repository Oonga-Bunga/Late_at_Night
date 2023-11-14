using UnityEngine;

public interface IKillableEntity
{
    public enum AttackSource
    {
        Flashlight,
        ClayBall,
        Rocket,
        Zanybell,
        KitestingerTrap,
        EvilBunny,
        Cat
    }

    public void TakeHit(float damage, AttackSource source);
    public void RecoverHealth(float amount);
    public void Die();
}
