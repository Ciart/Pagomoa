using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    static public Option instance = null;

    [SerializeField] public SetAudio audio;
 

    [SerializeField] public TMP_Dropdown dropdown;
    [SerializeField] public Canvas canvas;
    int currentOption = 0;
   

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        MakeDropDown();
        FirstOption();
       
        dropdown.onValueChanged.AddListener(delegate { SetDropDown(dropdown.value); });
    }
    public void MakeDropDown()
    {
        dropdown.ClearOptions();
        for (int i = 1; i <= 8; i++)
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData($"{i}"));
        }
    }
    public void FirstOption()
    {
        if (OptionDB.instance.scale - 1 == currentOption)
        {
            dropdown.value = currentOption;
            SetDropDown(currentOption);
        }
    }
    public void OnOptionUI()
    {
        bool activeOption = false;

        if (gameObject.activeSelf == false)
            activeOption = !activeOption;
        
        gameObject.SetActive(activeOption);

        if(audio != null) 
            OptionDB.instance.audioValue = audio.audioSlider.value;

        OptionDB.instance.scale = (int)canvas.scaleFactor;
    }
    public void OffOptionUI()
    {
        bool activeOption = false;
        OptionDB.instance.scale = dropdown.value + 1;
        AudioSlider.instance.SaveAudioValue();
        
        gameObject.SetActive(activeOption);
    }
    public void SetDropDown(int index)
    {
        canvas.GetComponent<CanvasScaler>().scaleFactor = index + 1;
    }
    public void SoundOptionCancle()
    {
        if (audio != null)
        {
            audio.audioMixer.SetFloat("BGM", OptionDB.instance.audioValue);
            audio.audioSlider.value = OptionDB.instance.audioValue;
        }
        OptionDB.instance.scale -= 1;
        dropdown.value = OptionDB.instance.scale;
        
        gameObject.SetActive(false);
    }
}
