using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EvilBunny
{
    public class EvilBunnyStateManager : StateManager
    {
        private GameObject baby;
        private GameObject switchGoal;

        public GameObject Baby
        {
            get { return baby; }
            set { baby = value; }
        }

        public GameObject SwitchGoal
        {
            get { return switchGoal; }
            set { switchGoal = value; }
        }
    }
}

