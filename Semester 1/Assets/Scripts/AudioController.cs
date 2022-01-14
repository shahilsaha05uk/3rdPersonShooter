using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private string volumeParam = "MasterVol";
    [SerializeField] private Slider masterSlider;
    [SerializeField] private AudioSource audioSource;

    private void Start()
    {
        masterSlider.onValueChanged.AddListener(SliderValue);

        GetComponent<AudioSource>().Play();
        masterSlider.value = PlayerPrefs.GetFloat(volumeParam, masterSlider.value);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(volumeParam, masterSlider.value);

    }

    private void SliderValue(float value)
    {
        mixer.SetFloat(volumeParam, value);
    }
}
