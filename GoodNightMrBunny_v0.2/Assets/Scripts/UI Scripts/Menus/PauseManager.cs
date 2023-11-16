using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private static PauseManager _instance;

    public static PauseManager Instance => _instance;

    [SerializeField] private bool _startsPaused = false;
    [SerializeField] private bool _isPaused = false;
    [SerializeField] private TextMeshProUGUI _pauseText;

    public bool IsPaused => _isPaused;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        if (_startsPaused)
        {
            PauseGame();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && GameManager.Instance.IsInGame)
        {
            _isPaused = !_isPaused;
            PauseGame();
        }
    }

    /// <summary>
    /// Detiene el tiempo del juego
    /// </summary>
    public void PauseGame()
    {
        if (_isPaused)
        {
            //_camera._enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            _pauseText.gameObject.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            //_camera._enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _pauseText.gameObject.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}
