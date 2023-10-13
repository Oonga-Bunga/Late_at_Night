using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    private TextMeshProUGUI timerText;
    private GameManager gameManager;

    void Start()
    {
        timerText = GetComponent<TextMeshProUGUI>();
        gameManager = FindObjectOfType<GameManager>();
        gameManager.TimerEvent += UpdateTimerDisplay;
    }

    void Update()
    {

    }

    void UpdateTimerDisplay(object sender, float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
