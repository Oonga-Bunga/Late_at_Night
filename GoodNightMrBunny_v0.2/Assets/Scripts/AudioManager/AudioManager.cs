using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixer masterMixer;

    public void SetMusicVolume(float musicVolume)
    {
        masterMixer.SetFloat("MusicVolume", musicVolume);
    }

    public void SetFXVolume(float fxVolume)
    {
        masterMixer.SetFloat("FXVolume", fxVolume);
    }
}
