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
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private Canvas _optionsCanvas;
    [SerializeField] private GameObject _inGameUI;
    private bool canPause = true;

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
        if (Input.GetKeyDown(KeyCode.Escape) && canPause/*&& GameManager.Instance.IsInGame*/)
        {
            PauseGame();
        }
    }

    public bool CanPause
    {
        get { return canPause; }
        set { canPause = value; }
    }

    /// <summary>
    /// Cierra el panel de pausa y reanuda el juego
    /// </summary>
    public void ClosePauseMenu()
    {
        _isPaused = !_isPaused;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _inGameUI.gameObject.SetActive(true);
        _pausePanel.gameObject.SetActive(false);
        _optionsCanvas.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    /// <summary>
    /// Detiene el tiempo del juego
    /// </summary>
    public void PauseGame()
    {
        _isPaused = !_isPaused;
        if (_isPaused)
        {
            //_camera._enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            _pausePanel.gameObject.SetActive(true);
            _optionsCanvas.gameObject.SetActive(false);
            _inGameUI.gameObject.SetActive(false);
            Time.timeScale = 0f;
        }
        else
        {
            //_camera._enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _inGameUI.gameObject.SetActive(true);
            _pausePanel.gameObject.SetActive(false);
            _optionsCanvas.gameObject.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}
