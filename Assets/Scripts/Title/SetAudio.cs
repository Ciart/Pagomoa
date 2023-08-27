using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Drawing.Inspector.PropertyDrawers;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetAudio : MonoBehaviour
{
    public static SetAudio instance = null; 

    [SerializeField] public AudioMixer audioMixer;
    [SerializeField] public Slider audioSlider;

    private void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
        LoadAudioOption();
    }
    public void AudioControl()
    {
        float sound = audioSlider.value;

        if (sound == -40f)
            audioMixer.SetFloat("MyExposedParam", -80);
        else
            audioMixer.SetFloat("MyExposedParam", sound);
    }
    public void LoadAudioOption()
    {
        float sound = OptionDB.instance.audioValue;
        if (sound == -40f)
        {
            audioMixer.SetFloat("MyExposedParam", -80);
            audioSlider.value = sound;
        }
        else
        {
            audioMixer.SetFloat("MyExposedParam", sound);
            audioSlider.value = sound;
        }
    }
}
