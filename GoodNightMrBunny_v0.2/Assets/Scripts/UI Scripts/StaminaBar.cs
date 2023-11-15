using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    [SerializeField] private Slider slider;

    private void Start()
    {
        PlayerController player = PlayerController.Instance;
        slider.maxValue = player.MaxStamina;
        slider.value = slider.maxValue;
        player.OnStaminaChanged += UpdateBar;
    }

    private void UpdateBar(object sender, float value)
    {
        slider.value = value;
    }
}
