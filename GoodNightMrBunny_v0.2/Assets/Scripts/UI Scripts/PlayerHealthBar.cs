using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] private Image _playerHealthBar;
    private PlayerHealth _player;
    private float _playerHealth;
    private void Start()
    {
        _player = PlayerController.Instance.PlayerHealth;
        _playerHealth = _player.MaxHealth;
        _player.OnHealthChanged += UpdateBar;
        _playerHealthBar.fillAmount = 0.5f;
    }

    private void UpdateBar(float value)
    {
        _playerHealthBar.fillAmount = Mathf.Clamp((_player.CurrentHealth) / (_player.MaxHealth*2), 0f, 0.5f);
    }
}
