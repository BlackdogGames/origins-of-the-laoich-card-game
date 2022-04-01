using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string ClipName;
    public AudioClip Clip;

    public AudioMixerGroup AudioMixerGroup;

    [Range(0f, 1f)]
    public float Volume;
    [Range(0.1f, 3f)]
    public float pitch;

    public bool loop;

    [HideInInspector]
    public AudioSource source;
}
