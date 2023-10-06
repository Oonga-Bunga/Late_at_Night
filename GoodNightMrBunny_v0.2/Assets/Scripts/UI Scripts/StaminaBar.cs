using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    [SerializeField] private Slider slider;

    private void Start()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        slider.maxValue = player.GetMaxStamina();
        slider.value = slider.maxValue;
        player.StaminaChanged += UpdateBar;
    }

    private void UpdateBar(object sender, float value)
    {
        slider.value = value;
    }
}
