using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    #region Attributes
    public Button[] tabButtons;
    public GameObject[] tabPanels;

    [SerializeField] private Canvas mainMenu;
    [SerializeField] private Canvas optionsMenu;
    [SerializeField] private Canvas loginMenu;
    [SerializeField] private GameObject loginButtons;
    [SerializeField] private GameObject loginAgeScrollList;
    
    private int currentTabIndex = 0;
    private bool ageScrollListOpen = false;
    #endregion

    #region Methods

    private void Start()
    {
        loginMenu.enabled = true;
        mainMenu.enabled = false;
        optionsMenu.enabled = false;
        loginAgeScrollList.SetActive(false);

        HidePanels();
        ShowTab(currentTabIndex);

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
        mainMenu.enabled = false;
        currentTabIndex = 0;
        HidePanels();
        ShowTab(0);
        optionsMenu.enabled = true;
    }

    /// <summary>
    /// Cambia al canvas del menú principal
    /// </summary>
    public void OpenMainMenu()
    {
        mainMenu.enabled = true;
        optionsMenu.enabled = false;
        loginMenu.enabled = false;
    }
    
    /// <summary>
    /// Método para cambiar los paneles del menú de opciones
    /// </summary>
    /// <param name="index">Indice del panel a mostrar</param>
    public void ShowTab(int index)
    {
        // Desactiva el panel actual.
        tabPanels[currentTabIndex].SetActive(false);

        // Activa el nuevo panel.
        tabPanels[index].SetActive(true);

        // Actualiza el índice actual.
        currentTabIndex = index;
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
    public void openTwitterURL()
    {
        Application.OpenURL("https://twitter.com/OongaBungaGames");
    }

    /// <summary>
    /// Activa o desactiva el ScrollList para seleccionar la edad
    /// </summary>
    public void selectAgeButton()
    {
        if (ageScrollListOpen)
        {
            ageScrollListOpen = !ageScrollListOpen;
            loginAgeScrollList.SetActive(false);
            loginButtons.SetActive(true);
        }
        else
        {
            ageScrollListOpen = !ageScrollListOpen;
            loginAgeScrollList.SetActive(true);
            loginButtons.SetActive(false);
        }
    }

    #endregion
    
}
