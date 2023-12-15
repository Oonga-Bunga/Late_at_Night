using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    private static PauseManager _instance;

    public static PauseManager Instance => _instance;

    [SerializeField] private bool _startsPaused = false;
    [SerializeField] private bool _isPaused = false;
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private Canvas _optionsCanvas;
    [SerializeField] private GameObject _inGameUI;
    private bool _canPause = false;
    [SerializeField] private InputActionReference _lookAction;
    [SerializeField] private GameObject _instructionPanel;

    public bool IsPaused => _isPaused;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }

        if (_startsPaused)
        {
            PauseGame();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && GameManager.Instance.IsInGame && _canPause)
        {
            PauseGame();
        }
    }

    public bool CanPause
    {
        get { return _canPause; }
        set { _canPause = value; }
    }

    /// <summary>
    /// Detiene el tiempo del juego
    /// </summary>
    public void PauseGame()
    {
        _isPaused = !_isPaused;
        if (_isPaused)
        {
            _lookAction.action.Disable();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;
            if (_instructionPanel.gameObject.activeSelf) return;
            _pausePanel.gameObject.SetActive(true);
            _optionsCanvas.gameObject.SetActive(false);
            _inGameUI.gameObject.SetActive(false);
        }
        else
        {
            _lookAction.action.Enable();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f;
            if (_instructionPanel.gameObject.activeSelf) return;
            _inGameUI.gameObject.SetActive(true);
            _pausePanel.gameObject.SetActive(false);
            _optionsCanvas.gameObject.SetActive(false);
        }
    }
}
