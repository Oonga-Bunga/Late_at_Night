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
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _fxSlider;
    #endregion
    
    #region Methods

    private void Start()
    {
        _musicSlider.onValueChanged.AddListener(SetMusicVolume);
        _fxSlider.onValueChanged.AddListener(SetFXVolume);
        if (PlayerPrefs.HasKey("Music"))
        {
            float _musicVolume = PlayerPrefs.GetFloat("Music");
            masterMixer.SetFloat("MusicVolume", _musicVolume);
            _musicSlider.value = _musicVolume;
        }

        if (PlayerPrefs.HasKey("FX"))
        {
            float _fxVolume = PlayerPrefs.GetFloat("FX");
            masterMixer.SetFloat("FXVolume", _fxVolume);
            _fxSlider.value = _fxVolume;
        }
    }

    /// <summary>
    /// Controla el volumen de la música del juego mediante un Slider
    /// </summary>
    /// <param name="musicVolume"></param>
    private void SetMusicVolume(float musicVolume)
    {
        masterMixer.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("Music", musicVolume);
    }

    /// <summary>
    /// Controla el volumen de los efectos de sonido mediante un Slider
    /// </summary>
    /// <param name="fxVolume"></param>
    private void SetFXVolume(float fxVolume)
    {
        masterMixer.SetFloat("FXVolume", fxVolume);
        PlayerPrefs.SetFloat("FX", fxVolume);
    }
    
    /// <summary>
    /// Quitamos los listeners para evitar fugas de memoria
    /// </summary>
    private void OnDestroy()
    {
        _musicSlider.onValueChanged.RemoveListener(SetMusicVolume);
        _fxSlider.onValueChanged.RemoveListener(SetFXVolume);
    }
    #endregion
}
