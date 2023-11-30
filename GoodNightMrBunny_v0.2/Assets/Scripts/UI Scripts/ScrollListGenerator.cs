using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScrollListGenerator : MonoBehaviour
{
    #region Attributes
    [SerializeField]private Button buttonPrefab;
    [SerializeField]private TextMeshProUGUI _textMenuAgeButton;
    [SerializeField]private RectTransform content; 
    [SerializeField]private LoginMenuManager loginMenuManager;
    private UserData userData;
    #endregion
    
    #region Methods
    private void Awake()
    {
        userData = FindObjectOfType<UserData>();
    }

    public  void Start()
    {
        //Crea los botones del Scroll List para seleccionar la edad del jugador
        for (int i = 1; i <= 100; i++)
        {
            // Crea una instancia del botón prefab
            Button newButton = Instantiate(buttonPrefab, content);

            // Configura el texto del botón con el número
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = i.ToString();

            newButton.onClick.AddListener(() =>
            {
                userData.setAge(int.Parse(newButton.GetComponentInChildren<TextMeshProUGUI>().text));
                _textMenuAgeButton.text = newButton.GetComponentInChildren<TextMeshProUGUI>().text; 
            });
            newButton.onClick.AddListener(()=>loginMenuManager.selectAgeButton());
        }
    }
    #endregion
}
