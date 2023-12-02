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

    public int _currentLevelPlayed { get; set; } = 0;

    #endregion

    #region Methods
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        _startButton.SetActive(false);

        SetupAndSignIn();
    }

    private async void SetupAndSignIn()
    {
        await UnityServices.InitializeAsync();
        SetupEvents();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        var loadedData = await CloudSaveService.Instance.Data.Player.LoadAllAsync();
        if (loadedData.ContainsKey("username") && loadedData["username"].Value.ToString().Any(c => char.IsLetterOrDigit(c)))
        {
            //skip filling information
            Debug.Log("Loaded Game as "+loadedData["username"].Value.GetAsString());

            Dictionary<string, Item> data = await LoadData();
            _progress = int.Parse(data["progress"].ToString());

            SceneManager.LoadScene("Main Menu");
            
        }
        else
        {
            //ask for filling information
        }
    }

    void SetupEvents()
    {
        AuthenticationService.Instance.SignedIn += () => {
            // Shows how to get a playerID
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");

            // Shows how to get an access token
            Debug.Log($"Access Token: {AuthenticationService.Instance.AccessToken}");

        };

        AuthenticationService.Instance.SignInFailed += (err) => {
            Debug.LogError(err);
        };

        AuthenticationService.Instance.SignedOut += () => {
            Debug.Log("Player signed out.");
        };

        AuthenticationService.Instance.Expired += () =>
        {
            Debug.Log("Player session could not be refreshed and expired.");
        };
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
        var data = new Dictionary<string, object>
        {
            { "username", _username },
            { "gender", _gender },
            { "age", _age},
            {"progress", _progress}
        };
        await CloudSaveService.Instance.Data.Player.SaveAsync(data);
        Debug.Log("Attempted to save data");
    }

    public async Task<Dictionary<string, Item>> LoadData()
    {
        var data = await CloudSaveService.Instance.Data.Player.LoadAllAsync();
        Debug.Log("Attempted to load data");
        return data;
    }

    /// <summary>
    /// Comprueba si los datos introducidos en el login son correctos
    /// </summary>
    public void CanStartGame()
    {
        if (_username != null && _gender != null && _age > 0)
        {
            if (_username.Any(c => char.IsLetterOrDigit(c)))
            {
                _startButton.SetActive(true);
            }
            else
            {
                _startButton.SetActive(false);
            }
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
