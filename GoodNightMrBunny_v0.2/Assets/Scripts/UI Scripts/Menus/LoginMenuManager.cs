using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginMenuManager : MonoBehaviour
{
    #region Attributes
    [SerializeField] private GameObject loginAgeScrollList;
    [SerializeField] private GameObject _mainLoginPanel;
    [SerializeField] private Button[] _selectedButton;
    [SerializeField] private Sprite[] _buttonsSprites;
    private int _selectedButtonIndex = -1;
    private bool _ageScrollListOpen = false;
    #endregion
    
    #region Methods
    private void Start()
    {
        loginAgeScrollList.SetActive(false);
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
            _mainLoginPanel.SetActive(true);
        }
        else
        {
            _ageScrollListOpen = !_ageScrollListOpen;
            loginAgeScrollList.SetActive(true);
            _mainLoginPanel.SetActive(false);
        }
    }

    /// <summary>
    /// Cambia a la escena de menú principal
    /// </summary>
    public void OpenMenuScene()
    {
        SceneManager.LoadScene("Main Menu");
    }
    
    /// <summary>
    /// Deja marcado el botón de genero que has seleccionado
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
    #endregion
}
