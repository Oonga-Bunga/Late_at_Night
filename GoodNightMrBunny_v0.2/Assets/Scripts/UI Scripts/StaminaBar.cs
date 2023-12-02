using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    [SerializeField] private Image _staminaBar;

    private void Start()
    {
        PlayerController player = PlayerController.Instance;
        player.PlayerMovement.OnStaminaChanged += UpdateBar;
        _staminaBar.fillAmount = 0.5f;
    }

    private void UpdateBar(float value)
    {
        _staminaBar.fillAmount = Mathf.Clamp(value / (200), 0f, 0.5f);
    }

    private void UpdateRadialBar()
    {

    }
}
