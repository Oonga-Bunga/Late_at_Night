using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using UnityEngine.Networking;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Unity.Services.CloudSave.Models;
using UnityEngine.SceneManagement;

public class UserData : MonoBehaviour
{
    #region Atributtes

    [SerializeField] private GameObject _startButton;

    private string _username;
    private string _gender;
    private int _age = -1;
    private int _progress = 1; //Levels available
    private static GameObject sampleInstance;
    [SerializeField] private DatabaseManager _databaseManager;

    public int currentLevelPlayed { get; set; } = 1;

    #endregion

    #region Methods
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        _startButton.SetActive(false);
    }

    public int GetAge()
    {
        return _age;
    }

    /// <summary>
    /// Actualiza el último nivel pasado por el jugador
    /// </summary>
    public void SetProgress()
    {
        _progress += 1;
    }
    
    public int GetProgress()
    {
        return _progress;
    }


    /// <summary>
    /// Actualiza el nombre de usuario del jugador
    /// </summary>
    /// <param name="input">nombre de usuario nuevo</param>
    public void setUsername(TextMeshProUGUI input)
    {
        _username = input.text;
        CanStartGame();
    }

    /// <summary>
    /// Actualiza el género del usuario
    /// </summary>
    /// <param name="selectedGender">género del usuario</param>
    public void setGender(string selectedGender)
    {
        _gender = selectedGender;
        CanStartGame();
    }

    /// <summary>
    /// Actualiza la edad del usuario
    /// </summary>
    /// <param name="selectedAge">Edad nueva a actualizar</param>
    public void setAge(int selectedAge)
    {
        _age = selectedAge;
        CanStartGame();
    }
    
    

    public async void SaveData()
    {
        Debug.Log("saving data");

        StartCoroutine(_databaseManager.SendPostRequest(_username, _gender, _age, _progress));
    }

    /// <summary>
    /// Comprueba si los datos introducidos en el login son correctos
    /// </summary>
    public void CanStartGame()
    {
        if (/*_username != null && */_gender != null && _age > 0)
        {
            _startButton.SetActive(true);
            /*
            if (_username.Any(c => char.IsLetterOrDigit(c)))
            {
                _startButton.SetActive(true);
            }
            else
            {
                _startButton.SetActive(false);
            }*/
        }
        else
        {
            _startButton.SetActive(false);
        }
    }

    public void openMenu()
    {
        SaveData();
        SceneManager.LoadScene("Main Menu");
    }
    
    #endregion
}
