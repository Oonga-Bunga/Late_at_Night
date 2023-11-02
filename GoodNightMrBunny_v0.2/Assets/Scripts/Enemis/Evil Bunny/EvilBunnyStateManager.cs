using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EvilBunny
{
    public class EvilBunnyStateManager : StateManager
    {
        private GameObject baby;
        public GameObject Baby
        {
            get { return baby; }
            set { baby = value; }
        }
    }
}

