using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    [SerializeField] string VolumeParameter = "MasterVolume";
    [SerializeField] AudioMixerGroup AudioMixerGroup;
    [SerializeField] AudioMixer AudioMixer;
    [SerializeField] Slider Slider;

    private void Awake()
    {
        Slider.onValueChanged.AddListener(SliderValueChanged);
    }

    private void SliderValueChanged(float value)
    {
        AudioMixer.SetFloat(VolumeParameter, value:Mathf.Log10(value) * 30f);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
