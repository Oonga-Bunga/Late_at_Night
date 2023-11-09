using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    #region Attributes
    [SerializeField] private Button[] tabButtons;
    [SerializeField] private GameObject[] tabPanels;

    [SerializeField] private Canvas mainMenu;
    [SerializeField] private Canvas optionsMenu;
    [SerializeField] private Canvas loginMenu;
    [SerializeField] private GameObject loginButtons;
    [SerializeField] private GameObject loginAgeScrollList;
    
    private int _currentTabIndex = 0;
    private bool _ageScrollListOpen = false;
    #endregion

    #region Methods

    private void Start()
    {
        //Se muestra el raton
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        //Se configuran las escenas
        loginMenu.gameObject.SetActive(true);
        mainMenu.gameObject.SetActive(false);
        optionsMenu.gameObject.SetActive(false);
        loginAgeScrollList.SetActive(false);

        HidePanels();
        ShowTab(_currentTabIndex);

        // Asigna los métodos ShowTab a los eventos de click de los botones.
        for (int i = 0; i < tabButtons.Length; i++)
        {
            int index = i;
            tabButtons[i].onClick.AddListener(() => ShowTab(index));
        }
    }

    /// <summary>
    /// Cambia a la escena de juego
    /// </summary>
    public void PressPlayButton()
    {
        SceneManager.LoadScene("LevelScene");
    }

    /// <summary>
    /// Cambia al canvas del menú de opciones
    /// </summary>
    public void OpenOptionsMenu()
    {
        mainMenu.gameObject.SetActive(false);
        _currentTabIndex = 0;
        HidePanels();
        ShowTab(0);
        optionsMenu.gameObject.SetActive(true);
    }

    /// <summary>
    /// Cambia al canvas del menú principal
    /// </summary>
    public void OpenMainMenu()
    {
        mainMenu.gameObject.SetActive(true);
        optionsMenu.gameObject.SetActive(false);
        loginMenu.gameObject.SetActive(false);
    }
    
    /// <summary>
    /// Método para cambiar los paneles del menú de opciones
    /// </summary>
    /// <param name="index">Indice del panel a mostrar</param>
    public void ShowTab(int index)
    {
        // Desactiva el panel actual.
        tabPanels[_currentTabIndex].SetActive(false);

        // Activa el nuevo panel.
        tabPanels[index].SetActive(true);

        // Actualiza el índice actual.
        _currentTabIndex = index;
    }

    /// <summary>
    /// Desactiva todos los paneles del menú de opciones
    /// </summary>
    public void HidePanels()
    {
        for (int i = 0; i < tabPanels.Length; i++)
        {
            tabPanels[i].SetActive(false);
        }
    }
    
    /// <summary>
    /// Abre la cuenta de twitter
    /// </summary>
    public void openUrl(string urlLink)
    {
        Application.OpenURL(urlLink);
    }

    /// <summary>
    /// Activa o desactiva el ScrollList para seleccionar la edad
    /// </summary>
    public void selectAgeButton()
    {
        if (_ageScrollListOpen)
        {
            _ageScrollListOpen = !_ageScrollListOpen;
            loginAgeScrollList.SetActive(false);
            loginButtons.SetActive(true);
        }
        else
        {
            _ageScrollListOpen = !_ageScrollListOpen;
            loginAgeScrollList.SetActive(true);
            loginButtons.SetActive(false);
        }
    }
    
    /// <summary>
    /// Cambia el nivel de calidad del videojuego
    /// </summary>
    /// <param name="qualityIndex">Indice del nivel de calidad a cambiar</param>
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    #endregion
    
}
