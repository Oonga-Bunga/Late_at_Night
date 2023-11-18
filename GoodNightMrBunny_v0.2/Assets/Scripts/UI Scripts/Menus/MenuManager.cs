using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    #region Attributes
    [SerializeField] private Button[] tabButtons;
    [SerializeField] private GameObject[] tabPanels;
    [SerializeField] private GameObject[] _mobileGUI;
    [SerializeField] private GameObject[] _computerGUI;
    [SerializeField] private Button[] _selectedButton;
    [SerializeField] private Sprite[] _buttonsSprites;
    private int _selectedButtonIndex = -1;
    [SerializeField] private TextMeshProUGUI _canvasQualityText;
    private String[] _qualityLevels = new[] { "High", "Medium", "Low" };
    private int _qualityIntLevel = 0;

    [SerializeField] private Canvas mainMenu;
    [SerializeField] private Canvas optionsMenu;
    [SerializeField] private Canvas loginMenu;
    [SerializeField] private Canvas selectLevelMenu;
    [SerializeField] private TextMeshProUGUI _optionsTabText;

    [SerializeField] private GameObject loginButtons;
    [SerializeField] private GameObject loginAgeScrollList;
    [SerializeField] private AudioSource _clickSound;

    private Button[] _buttons;
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
        selectLevelMenu.gameObject.SetActive(false);
        if(FindObjectOfType<UserData>().GetAge() >= 0) OpenMainMenu();

        //Elementos de interfaz
        ShowDispositiveElements();
        HidePanels();
        ShowTab(_currentTabIndex);

        // Asigna los métodos ShowTab a los eventos de click de los botones.
        for (int i = 0; i < tabButtons.Length; i++)
        {
            int index = i;
            tabButtons[i].onClick.AddListener(() => ShowTab(index));
        }

        //Sonido de botones en la UI
        _buttons = Resources.FindObjectsOfTypeAll<Button>();
        foreach (var button in _buttons)
        {
            button.onClick.AddListener(()=>_clickSound.PlayOneShot(_clickSound.clip));
        }
    }

    private void Update()
    {
        if (optionsMenu.enabled == false)return;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenMainMenu();
        }
    }

    /// <summary>
    /// Cambia a la escena de juego
    /// </summary>
    public void openSelectLevelMenu()
    {
        selectLevelMenu.gameObject.SetActive(true);
        mainMenu.gameObject.SetActive(false);
        optionsMenu.gameObject.SetActive(false);
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
        selectLevelMenu.gameObject.SetActive(false);
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
    /// Cambia el nombre del tab en actual del menu de opciones
    /// </summary>
    /// <param name="tabName"></param>
    public void SetTabsText(string tabName)
    {
        _optionsTabText.text = tabName;
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
    /// Abre la escena del menú seleccionado
    /// </summary>
    public void selectLevel(string levelRoute)
    {
        SceneManager.LoadScene(levelRoute);
    }

    public void SetJsonOne(TextAsset jsonAsset)
    {
        FindObjectOfType<LevelJsons>().SetFirstJason(jsonAsset);
    }

    public void SetJsonTwo(TextAsset jsonAsset)
    {
        FindObjectOfType<LevelJsons>().SetSecondJason(jsonAsset);
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
    /// <param name="arrowDirection">-1 si se pulsa la flecha izquierda y +1 la derecha</param>
    public void SetQuality(int arrowDirection)
    {
        _qualityIntLevel = (_qualityIntLevel + arrowDirection)%3;
        if (_qualityIntLevel < 0) _qualityIntLevel = 2;
        _canvasQualityText.text = _qualityLevels[_qualityIntLevel];
        QualitySettings.SetQualityLevel(_qualityIntLevel);
    }

    /// <summary>
    /// Cierra la ventana del videojuego
    /// </summary>
    public void CloseApplication()
    {
        Application.Quit();
    }

    /// <summary>
    /// Muestra los elementos del dispositivo con el que se está jugando
    /// </summary>
    public void ShowDispositiveElements()
    {
        if (!Application.isMobilePlatform)
        {
            foreach (var mobileElement in _mobileGUI)
            {
                mobileElement.SetActive(false);
            }
        }
        else
        {
            foreach (var computerElemet in _computerGUI)
            {
                computerElemet.SetActive(false);
            }
        }
    }

    //Cambia el sprite del boton seleccionado
    public void ChangeSelectedButtonSprite(int buttonIndex)
    {
        //Sprite seleccionado
        _selectedButton[buttonIndex].image.sprite = _buttonsSprites[1];

        if (_selectedButtonIndex < 0)
        {
            _selectedButtonIndex = buttonIndex;
            return;
        }
        
        //DeseleccionarAnterior
        _selectedButton[_selectedButtonIndex].image.sprite = _buttonsSprites[0];
        _selectedButtonIndex = buttonIndex;
    }

    #endregion
    
}
