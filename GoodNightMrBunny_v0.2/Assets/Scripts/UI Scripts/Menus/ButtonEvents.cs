using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonEvents : MonoBehaviour
{
    private TextMeshProUGUI _buttonText;
    private Color colorOriginal;

    private void Start()
    {
        _buttonText = GetComponentInChildren<TextMeshProUGUI>();
        colorOriginal = _buttonText.color;
    }
    
    public void OnButtonPressed()
    {
        _buttonText.color = Color.white;
    }

    public void OnButtonReleased()
    {
        _buttonText.color = colorOriginal;
    }
}
