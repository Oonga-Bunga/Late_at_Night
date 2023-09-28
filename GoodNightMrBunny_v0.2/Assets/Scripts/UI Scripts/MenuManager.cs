using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Canvas mainMenu;
    [SerializeField] private Canvas optionsMenu;

    #region Methods

    private void Start()
    {
        mainMenu.enabled = true;
        optionsMenu.enabled = false;
    }

    /// <summary>
    /// Cambia a la escena de juego
    /// </summary>
    public void PressPlayButton()
    {
        SceneManager.LoadScene("SampleScene");
    }

    /// <summary>
    /// Cambia al canvas del menú de opciones
    /// </summary>
    public void OpenOptionsMenu()
    {
        mainMenu.enabled = false;
        optionsMenu.enabled = true;
    }

    /// <summary>
    /// Cambia al canvas del menú principal
    /// </summary>
    public void OpenMainMenu()
    {
        mainMenu.enabled = true;
        optionsMenu.enabled = false;
    }

    #endregion
    
}
