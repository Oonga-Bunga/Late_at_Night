using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public bool isPaused = true;
    public TextMeshProUGUI pauseText;

    private void Start()
    {
        pauseText.gameObject.SetActive(true);
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
            //camera.enabled = false;
            pauseText.gameObject.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            //camera.enabled = true;
            pauseText.gameObject.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}