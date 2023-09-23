using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    /// <summary>
    /// Cambia a la escena de juego
    /// </summary>
    public void PressPlayButton()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
