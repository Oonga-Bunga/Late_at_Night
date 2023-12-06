using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI_Scripts.Menus
{
    public class LevelMenuManager: MonoBehaviour
    {
        private static LevelMenuManager _instance;

        public static LevelMenuManager Instance => _instance;

        [SerializeField] private GameObject _pausePanel;
        [SerializeField] private Canvas _optionsCanvas;
        [SerializeField] private GameObject _instructionPanel;
        [SerializeField] private GameObject _gameUI;
        [SerializeField] private TextMeshProUGUI _loadingText;
        [SerializeField] private GameObject _loadingScreen;
        [SerializeField] private GameObject _loadingScreenCamera;
        [SerializeField] private GameObject _mobileControls;

        [SerializeField] private Button[] tabButtons;
        [SerializeField] private GameObject[] tabPanels;
        [SerializeField] private TextMeshProUGUI _optionsTabText;
        [SerializeField] private Button[] _selectedButton;
        [SerializeField] private Sprite[] _buttonsSprites;
        private int _currentTabIndex = 0;
        private int _selectedButtonIndex = -1;
        [SerializeField] private TextMeshProUGUI _canvasQualityText;
        private String[] _qualityLevels = new[] { "High", "Medium", "Low" };
        private int _qualityIntLevel = 0;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(this);
            }

            if (Application.isMobilePlatform)
            {
                _mobileControls.SetActive(true);
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                _mobileControls.SetActive(false);
            }
        }

        private void Start()
        {
            GameManager.Instance.OnGameStarted += StartGame;
            
            //PlayerPrefs
            if (PlayerPrefs.HasKey("OptionsQualityIndex"))
            {
                int _qualityIdx = PlayerPrefs.GetInt("OptionsQualityIndex");
                QualitySettings.SetQualityLevel(_qualityIdx);
                _canvasQualityText.text = _qualityLevels[_qualityIdx];
            }
        }

        public void ChangeLodingText(string text)
        {
            _loadingText.text = text;
        }

        public void StartGame()
        {
            _loadingScreen.SetActive(false);
            _loadingScreenCamera.SetActive(false);
            Destroy(_loadingScreenCamera.gameObject);
            _gameUI.SetActive(true);
            OpenInstructionsPanel();
        }

        /// <summary>
        /// Abre el menú de opciones y cierra el panel de pausa
        /// </summary>
        public void OpenOptionsCanvas()
        {
            _optionsCanvas.gameObject.SetActive(true);
            _pausePanel.gameObject.SetActive(false);
        }

        /// <summary>
        /// Abre el panel de instrucciones
        /// </summary>
        public void OpenInstructionsPanel()
        {
            FindObjectOfType<PauseManager>().CanPause = false;
            _instructionPanel.gameObject.SetActive(true);
            _optionsCanvas.gameObject.SetActive(false );
            _gameUI.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;
        }

        /// <summary>
        /// Cierra el panel de intrucciones
        /// </summary>
        public void CloseInstructionsPanel()
        {
            FindObjectOfType<PauseManager>().CanPause = true;
            _instructionPanel.gameObject.SetActive(false);
            _optionsCanvas.gameObject.SetActive(false);
            _gameUI.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f;
        }

        /// <summary>
        /// Cierra el panel del menú de opciones y abre el panel de pausa
        /// </summary>
        public void CloseOptionsCanvas()
        {
            _optionsCanvas.gameObject.SetActive(false);
            _pausePanel.gameObject.SetActive(true);
        }

        /// <summary>
        /// Cambia de escena al menú principal
        /// </summary>
        public void OpenMainMenu()
        {
            StopAllCoroutines();
            Time.timeScale = 1f;
            SceneManager.LoadScene("Main Menu");
        }

        /// <summary>
        /// Reanuda el juego cerrando los paneles
        /// </summary>
        public void ClosePauseMenu()
        {
            FindObjectOfType<PauseManager>().PauseGame();
        }
        
        /// <summary>
        /// Manejo entre tabs groups del menú de opciones
        /// </summary>
        /// <param name="index"></param>
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
        /// Ajusta la calidad del juego
        /// </summary>
        /// <param name="arrowDirection"></param>
        public void SetQuality(int arrowDirection)
        {
            _qualityIntLevel = (_qualityIntLevel + arrowDirection)%3;
            if (_qualityIntLevel < 0) _qualityIntLevel = 2;
            _canvasQualityText.text = _qualityLevels[_qualityIntLevel];
            QualitySettings.SetQualityLevel(_qualityIntLevel);
            PlayerPrefs.SetInt("OptionsQualityIndex", _qualityIntLevel);
            PlayerPrefs.Save();
        }
        
        /// <summary>
        /// Indica el tab seleccionado del menú de opciones
        /// </summary>
        /// <param name="buttonIndex"></param>
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
        
        /// <summary>
        /// Abre la cuenta de twitter
        /// </summary>
        public void openUrl(string urlLink)
        {
            Application.OpenURL(urlLink);
        }
    }
}