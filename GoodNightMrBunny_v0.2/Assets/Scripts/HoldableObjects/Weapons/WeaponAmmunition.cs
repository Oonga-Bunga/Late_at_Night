using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAmmunition : MonoBehaviour
{
    public float currentAmmunition = 0f;

    public void setCurrentAmmunition(float ammunition)
    {
        currentAmmunition = ammunition;
    }
}
