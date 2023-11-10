using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    #region Atributes
    [SerializeField] private AudioMixer masterMixer;
    #endregion
    
    #region Methods
    /// <summary>
    /// Controla el volumen de la música del juego mediante un Slider
    /// </summary>
    /// <param name="musicVolume"></param>
    public void SetMusicVolume(float musicVolume)
    {
        masterMixer.SetFloat("MusicVolume", musicVolume);
    }

    /// <summary>
    /// Controla el volumen de los efectos de sonido mediante un Slider
    /// </summary>
    /// <param name="fxVolume"></param>
    public void SetFXVolume(float fxVolume)
    {
        masterMixer.SetFloat("FXVolume", fxVolume);
    }
    #endregion
}
