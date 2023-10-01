using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScrollListGenerator : MonoBehaviour
{
    public Button buttonPrefab; // Asigna el botón prefab en el Inspector
    public RectTransform content; // Asigna el objeto de contenido en el Inspector
    
    public  void Start()
    {
        //Crea los botones del Scroll List para seleccionar la edad del jugador
        for (int i = 1; i <= 100; i++)
        {
            // Crea una instancia del botón prefab
            Button newButton = Instantiate(buttonPrefab, content);

            // Configura el texto del botón con el número
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = i.ToString();
        }
    }
}
