using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI_Scripts.Menus
{
    public class LevelMenuManager: MonoBehaviour
    {
        [SerializeField] private GameObject _pausePanel;
        [SerializeField] private Canvas _optionsCanvas;
        
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
        

        /// <summary>
        /// Abre el menú de opciones y cierra el panel de pausa
        /// </summary>
        public void OpenOptionsCanvas()
        {
            _optionsCanvas.gameObject.SetActive(true);
            _pausePanel.gameObject.SetActive(false);
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