using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BabyBar : MonoBehaviour
{
    [SerializeField] private Slider slider;

    private void Start()
    {
        Baby baby = Baby.Instance;
        slider.maxValue = baby.MaxHealth;
        slider.value = slider.maxValue;
        baby.OnHealthChanged += UpdateBar;
    }
    
    private void UpdateBar(float value)
    {
        slider.value = value;
    }
}
