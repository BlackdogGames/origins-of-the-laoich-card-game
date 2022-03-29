using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    [SerializeField] AudioMixer AudioMixer;
    [SerializeField] Slider MasterSlider;
    [SerializeField] Slider MusicSlider;
    [SerializeField] Slider SFXSlider;

    public const string MasterVolume = "MasterVolume";
    public const string MusicVolume = "MusicVolume";
    public const string SFXVolume = "SFXVolume";

    private void Awake()
    {
        MasterSlider.onValueChanged.AddListener(SetMasterVolume);
        MusicSlider.onValueChanged.AddListener(SetMusicVolume);
        SFXSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    private void Start()
    {
        MasterSlider.value = PlayerPrefs.GetFloat(AudioManager.MasterKey, 1f);
        MusicSlider.value = PlayerPrefs.GetFloat(AudioManager.MusicKey, 1f);
        SFXSlider.value = PlayerPrefs.GetFloat(AudioManager.SFXKey, 1f);
    }

    //save preference key
    private void OnDisable()
    {
        PlayerPrefs.SetFloat(AudioManager.MasterKey, MasterSlider.value);
        PlayerPrefs.SetFloat(AudioManager.MusicKey, MusicSlider.value);
        PlayerPrefs.SetFloat(AudioManager.SFXKey, SFXSlider.value);
    }


    void SetMasterVolume(float volume)
    {
        AudioMixer.SetFloat(MasterVolume, Mathf.Log10(volume) * 20);
    }

    void SetMusicVolume(float volume)
    {
        AudioMixer.SetFloat(MusicVolume, Mathf.Log10(volume) * 20);
    }

    void SetSFXVolume(float volume)
    {
        AudioMixer.SetFloat(SFXVolume, Mathf.Log10(volume) * 20);
    }

}
