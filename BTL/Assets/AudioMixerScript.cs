using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioMixerScript : MonoBehaviour
{

    public AudioMixer audioMixer;
    public Slider volumeSlider;

    public void Awake()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("sliderVolume");
    }
    private void Start()
    {
        audioMixer.SetFloat("volume", PlayerPrefs.GetFloat("sliderVolume"));
    }

    public void Update()
    {
        PlayerPrefs.SetFloat("sliderVolume", volumeSlider.value);
    }
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", Mathf.Log10(volume) * 20);
    }
}
