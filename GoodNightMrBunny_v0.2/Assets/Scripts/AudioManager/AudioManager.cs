using System;
using System.Collections;
using System.Collections.Generic;
using Player;
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
    [SerializeField] private Slider _sensitivitySlider;
    private CameraController _cameraController;
    #endregion
    
    #region Methods

    private void Start()
    {
        _cameraController = FindObjectOfType<CameraController>();
        _musicSlider.onValueChanged.AddListener(SetMusicVolume);
        _fxSlider.onValueChanged.AddListener(SetFXVolume);
        _sensitivitySlider.onValueChanged.AddListener(SetSensitivitySlider);
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
        
        if (PlayerPrefs.HasKey("CameraSensitivity"))
        {
            float _sensivityValue = PlayerPrefs.GetFloat("CameraSensitivity");
            SetSensitivitySlider(_sensivityValue);
            _sensitivitySlider.value = _sensivityValue;
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
    /// Controla el nivel de sensibilidad de la cámara mediante un slider
    /// </summary>
    /// <param name="value"></param>
    public void SetSensitivitySlider(float value)
    {
        if (_cameraController != null)
        {
            _cameraController.SensitivityX = value;
            _cameraController.SensitivityY = _cameraController.SensitivityX / 2;
        }
            
        PlayerPrefs.SetFloat("CameraSensitivity", value);
    }
    
    /// <summary>
    /// Quitamos los listeners para evitar fugas de memoria
    /// </summary>
    private void OnDestroy()
    {
        _musicSlider.onValueChanged.RemoveListener(SetMusicVolume);
        _fxSlider.onValueChanged.RemoveListener(SetFXVolume);
        _sensitivitySlider.onValueChanged.RemoveListener(SetSensitivitySlider);
    }
    #endregion
}
