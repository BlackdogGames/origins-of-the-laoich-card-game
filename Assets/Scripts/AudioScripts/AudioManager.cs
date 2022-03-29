using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class AudioManager : MonoBehaviour
{

	public Sound[] sounds;

	// Singleton instance.
	public static AudioManager Instance = null;

    public const string MasterKey = "MasterVolume";
    public const string MusicKey = "MusicVolume";
    public const string SFXKey = "SFXVolume";

    [SerializeField] AudioMixer AudioMixer;

	// Initialize the singleton instance.
	private void Awake()
	{
		// If there is not already an instance of AudioManager, set it to this.
		if (Instance == null)
		{
			Instance = this;
		}
		//If an instance already exists, destroy it.
		else if (Instance != this)
		{
			Destroy(gameObject);
		}

		//Set AudioManager to DontDestroyOnLoad so that it won't be destroyed when reloading the scene.
		DontDestroyOnLoad(gameObject);

		foreach (Sound s in sounds)
        {
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.Clip;

			s.source.outputAudioMixerGroup = s.AudioMixerGroup;

			s.source.volume = s.Volume;
			s.source.pitch = s.pitch;
			s.source.loop = s.loop;
        }

		LoadVolume();

	}
    
	void Start()
	{
		Play("MasterMusic");
	}

	public void Play(string name)
    {
		Sound s = Array.Find(sounds, s => s.ClipName == name);
		if (s == null)
        {
			Debug.LogWarning("Sound:" + name + " not Found!");
			return;
        }
		s.source.Play();
    }

    void LoadVolume()
    {
        float masterVolume = PlayerPrefs.GetFloat(MasterKey, 1f);
        float musicVolume = PlayerPrefs.GetFloat(MusicKey, 1f);
        float sfxVolume = PlayerPrefs.GetFloat(SFXKey, 1f);

        AudioMixer.SetFloat(VolumeController.MasterVolume, Mathf.Log10(masterVolume) * 20);
        AudioMixer.SetFloat(VolumeController.MusicVolume, Mathf.Log10(musicVolume) * 20);
        AudioMixer.SetFloat(VolumeController.SFXVolume, Mathf.Log10(sfxVolume) * 20);
    }

}
