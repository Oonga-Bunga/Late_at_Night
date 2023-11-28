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
using UnityEditor.PackageManager;
using UnityEditor.Experimental.GraphView;

public class UserData : MonoBehaviour
{
    #region Atributtes

    [SerializeField] private GameObject _startButton;
    
    private string _username;
    private string _gender;
    private int _age = -1;
    private static GameObject sampleInstance;

    private string projectId = "a0aa3f3d-2e34-4fb5-9b07-f71db6dadf34";
    private string environmentId = "5079820c-3f88-4786-a800-84d185339de1";
    private string serviceAccountKeyId = "cfc358f1-aefe-49da-94ed-e624c837326c";
    private string secretKey = "DbNZIFvQq7U-JLKyfMirM36irE221tPf";

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
        Debug.Log(UnityServices.State);
        SetupEvents();
        StartCoroutine(ExchangeToken());
    }

    private async Task SignInAnonymouslyAsync()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Sign in anonymously succeded");
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");
        }
        catch (AuthenticationException ex)
        {
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            Debug.LogException(ex);
        }
    }

    // Setup authentication event handlers if desired
    private void SetupEvents()
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

    private IEnumerator ExchangeToken()
    {
        // Construir la cadena de autorización
        string authorizationHeader = "Basic " + System.Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(serviceAccountKeyId + ":" + secretKey));

        // Construir la URL
        string url = $"https://services.api.unity.com/auth/v1/token-exchange?projectId={projectId}&environmentId={environmentId}";

        // Crear la solicitud
        UnityWebRequest request = UnityWebRequest.Post(url, "", "application/json");

        // Agregar el encabezado de autorización
        request.SetRequestHeader("Authorization", authorizationHeader);

        // Enviar la solicitud y esperar la respuesta
        yield return request.SendWebRequest();

        // Manejar la respuesta
        if (request.result == UnityWebRequest.Result.Success)
        {
            // Acceder al token desde la respuesta
            string token = request.downloadHandler.text;
            Debug.Log("Token obtenido con éxito: " + token);
        }
        else
        {
            // Manejar el error
            Debug.LogError("Error al obtener el token: " + request.error);
        }
    }

    public int GetAge()
    {
        return _age;
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

    private async void SaveData()
    {
        var data = new Dictionary<string, object>
        {
            { "username", _username },
            { "gender", _gender },
            { "age", _age}
        };
        await CloudSaveService.Instance.Data.Player.SaveAsync(data);
        Debug.Log("Attempted to save data");
    }

    private async void LoadData()
    {
        var keysToLoad = new HashSet<string>
        {
            "username",
            "gender",
            "age"
        };
        var loadedData = await CloudSaveService.Instance.Data.Player.LoadAsync(keysToLoad);
        var loadedUsername = loadedData["username"].Value.GetAsString();
        var loadedGender = loadedData["gender"].Value.GetAsString();
        var loadedAge = loadedData["age"].Value.GetAsString();
        Debug.Log("Loaded data. Username: " + loadedUsername + ", Gender: " + loadedGender + ", Age: " + loadedAge);
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
                //SaveData();
                LoadData();
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
    #endregion
}
