using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Image _staminaBar;

    private void Start()
    {
        PlayerController player = PlayerController.Instance;
        slider.maxValue = player.MaxStamina;
        slider.value = slider.maxValue;
        player.OnStaminaChanged += UpdateBar;

        _staminaBar.fillAmount = 0.5f;
    }

    private void UpdateBar(object sender, float value)
    {
        slider.value = value;
        _staminaBar.fillAmount = Mathf.Clamp(value / (slider.maxValue*2), 0f, 0.5f);
    }

    private void UpdateRadialBar()
    {
        
    }
}
