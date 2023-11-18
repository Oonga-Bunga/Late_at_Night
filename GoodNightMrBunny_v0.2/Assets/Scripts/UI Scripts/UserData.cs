using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class UserData : MonoBehaviour
{
    #region Atributtes

    [SerializeField] private GameObject _startButton;
    
    private string _username;
    private string _gender;
    private int _age = -1;

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
    #endregion
}
