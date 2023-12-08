using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalMenuManager : MonoBehaviour
{
    private void Start()
    {
        var _userData = FindObjectOfType<UserData>();
        if (SceneManager.GetActiveScene().name == "WinScene" && _userData.currentLevelPlayed == _userData.GetProgress())
        {
            _userData.SetProgress();
            _userData.SaveData();
        }
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
