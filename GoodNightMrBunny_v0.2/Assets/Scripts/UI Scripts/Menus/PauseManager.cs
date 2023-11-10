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

    public bool isPaused = false;
    public TextMeshProUGUI pauseText;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        pauseText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            PauseGame();
        }
    }

    /// <summary>
    /// Detiene el tiempo del juego
    /// </summary>
    private void PauseGame()
    {
        if (isPaused)
        {
            //_camera._enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            pauseText.gameObject.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            //_camera._enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            pauseText.gameObject.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}
