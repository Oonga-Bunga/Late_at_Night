using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Shadow
{
    public class Shadow : AMonster
    {
        private bool _isAvoiding = false;
        private bool _isBeingLit = false;
        private bool _isStunned = false;

        private void Update()
        {
            if (_isAvoiding)
            {
                //steering avoidance
            }
        }

        public override void TakeHit(float damage, IKillableEntity.AttackSource source)
        {
            switch (source)
            {
                case IKillableEntity.AttackSource.Flashlight:
                    ChangeHealth(damage, true);
                    break;
                case IKillableEntity.AttackSource.ClayBall:
                case IKillableEntity.AttackSource.Rocket:
                    Stunned();
                    break;
            }
        }

        public void PlayAttackAnimation()
        {

        }

        public void PlaySuccAnimation()
        {

        }

        public void Stunned()
        {
            //play stunned animation
        }

        public void StartAvoiding()
        {
            _isAvoiding = true;
        }

        public void StopAvoiding()
        {
            _isAvoiding = false;
        }

        public override void Die()
        {

        }
    }
}

    
