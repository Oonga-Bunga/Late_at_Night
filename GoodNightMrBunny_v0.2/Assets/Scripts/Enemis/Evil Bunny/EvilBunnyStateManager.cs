using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EvilBunny
{
    public class EvilBunnyStateManager : MonoBehaviour
    {
        [SerializeField] private State currentState;
        [SerializeField] private AMonster enemy;
        private GameObject baby;
        public GameObject Baby
        {
            get { return baby; }
            set { baby = value; }
        }

        void Update()
        {
            RunStateMachine();
        }
        private void RunStateMachine()
        {
            State nextState = currentState?.RunCurrentState(enemy);
            if(nextState != null)
            {
                SwitchToTheNextState(nextState);
            }
        }
        private void SwitchToTheNextState(State nextState)
        {
            currentState = nextState;
        }
    }
}

