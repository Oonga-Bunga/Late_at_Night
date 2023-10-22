using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BabyBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    private bool babyFound = false;

    private void Start()
    {

    }

    private void Update()
    {
        if (babyFound) return;

        Baby baby = FindObjectOfType<Baby>();
        slider.maxValue = baby.MaxHealth;
        slider.value = slider.maxValue;
        baby.HealthChanged += UpdateBar;
    }

    private void UpdateBar(object sender, float value)
    {
        slider.value = value;
    }
}
