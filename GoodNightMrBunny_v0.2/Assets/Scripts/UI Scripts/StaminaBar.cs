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
        slider.maxValue = player.PlayerMovement.MaxStamina;
        slider.value = slider.maxValue;
        player.PlayerMovement.OnStaminaChanged += UpdateBar;
    }

    private void UpdateBar(float value)
    {
        slider.value = value;
    }
}
