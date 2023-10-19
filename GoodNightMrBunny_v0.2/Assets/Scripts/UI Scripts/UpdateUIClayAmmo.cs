using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateUIClayAmmo : MonoBehaviour
{
    #region Attributes
    [SerializeField]private int maxBallNumber;
    [SerializeField]private int currentBallNumber;
    [SerializeField] private TextMeshProUGUI ammoText;
    #endregion
    
    #region Methods
    void Start()
    {
        currentBallNumber = maxBallNumber;
    }
    
    public void setMaxBallNumber(int maxAmmo)
    {
        maxBallNumber = maxAmmo;
    }

    /// <summary>
    /// Método que actualiza la munición de plastilina en la UI del nivel
    /// </summary>
    /// <param name="currentAmmo">Numero de bolas restantes</param>
    public void UpdateClayText(int currentAmmo)
    {
        currentBallNumber = currentAmmo;
        ammoText.text = $"{currentBallNumber}/{maxBallNumber}";
    }
    #endregion
}
