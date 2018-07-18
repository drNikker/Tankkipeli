using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;



public class Options : MonoBehaviour
{

    
    public AudioMixer masterMixer;
    //public Button masterVolButton;
    //public Button musicVolButton;
    //public Button SFXVolButton;
    public Text resoText;
    public Text framerateText;
    public Text masterVolText;
    public Text musicVolText;
    public Text sfxVolText;

    private float masterVolFloat;
    private float musicVolFloat;
    private float sfxVolFloat;
    private int targetFramerate = 30;

    private string resolutionChoicePref = "ResolutionChoicePref_";
    private string masterVolumePref = "MasterVolPref_";
    private string musicVolumePref = "MusicVolPref_";
    private string sfxVolumePref = "SFXVolPref_";


    private void Start()
    {
        GetOptions();
        
    }
    
    ////SET VOLS
    //public void MasterVolumeButton(float masterLvl)
    //{
    //    masterMixer.SetFloat("MasterVol", GetDecibel(masterLvl));
    //}

    //public void MusicVolumeButton(float musicLvl)
    //{
    //    masterMixer.SetFloat("MusicVol", GetDecibel(musicLvl));
    //}

    //public void SFXVolumeButton(float sfxLvl)
    //{
    //    masterMixer.SetFloat("SFXVol", GetDecibel(sfxLvl));
    //}


    //SET AND GET

    public void SetOptions()
    {
        PlayerPrefs.SetFloat(masterVolumePref, masterVolFloat);
        PlayerPrefs.SetFloat(musicVolumePref, musicVolFloat);
        PlayerPrefs.SetFloat(sfxVolumePref, sfxVolFloat);
        //PlayerPrefs.SetInt(resolutionChoicePref, resoDropDown.value);
        PlayerPrefs.Save();
    }

    public void GetOptions()
    {
        masterVolFloat = PlayerPrefs.GetFloat(masterVolumePref, 50f);
        masterVolText.text = masterVolFloat.ToString();
        masterMixer.SetFloat("MasterVol", GetDecibel(masterVolFloat));
        musicVolFloat = PlayerPrefs.GetFloat(musicVolumePref, 50f);
        musicVolText.text = musicVolFloat.ToString();
        masterMixer.SetFloat("MusicVol", GetDecibel(musicVolFloat));
        sfxVolFloat = PlayerPrefs.GetFloat(sfxVolumePref, 50f);
        sfxVolText.text = sfxVolFloat.ToString();
        masterMixer.SetFloat("SFXVol", GetDecibel(sfxVolFloat));
        //resoDropDown.value = PlayerPrefs.GetInt(resolutionChoicePref, 0);
    }
    

    // Returns correct decibel value for linear value (0.0f - 1.0f)
    // 1.0f = 0dB
    // 0.5f = -6dB
    // 0.0f = -80dB
    private float GetDecibel(float musicVal)
    {
        float linearValue = musicVal / 100;
        linearValue = Mathf.Clamp01(linearValue);
        float decibel = -80f;
        if (linearValue != 0f)
            decibel = 20f * Mathf.Log10(linearValue);

        return decibel;
    }


    public void MasterVolumeButton()
    {
        if (masterVolFloat < 100f)
        {
            masterVolFloat += 10;
        }
        else
        {
            masterVolFloat = 0f;
        }
        masterVolText.text = masterVolFloat.ToString();
        masterMixer.SetFloat("MasterVol", GetDecibel(masterVolFloat));
        Debug.Log("MasterVol");
    }

    public void MusicVolumeButton()
    {
        if (musicVolFloat < 100f)
        {
            musicVolFloat += 10;
        }
        else
        {
            musicVolFloat = 0f;
        }
        musicVolText.text = musicVolFloat.ToString();
        masterMixer.SetFloat("MusicVol", GetDecibel(musicVolFloat));
        Debug.Log("MusicBtn");
    }

    public void SFXVolumeButton()
    {
        if (sfxVolFloat < 100f)
        {
            sfxVolFloat += 10;
        }
        else
        {
            sfxVolFloat = 0f;
        }
        sfxVolText.text = sfxVolFloat.ToString();
        masterMixer.SetFloat("SFXVol", GetDecibel(sfxVolFloat));
    }

    public void framerateButton()
    {
        
        switch(framerateText.text)
        {
            case "30":
                framerateText.text = "60";
                targetFramerate = 60;
                break;
            case "60":
                framerateText.text = "120";
                targetFramerate = 120;
                break;
            case "120":
                framerateText.text = "144";
                targetFramerate = 144;
                break;
            case "144":
                framerateText.text = "No Limit";
                targetFramerate = 600;
                break;
            case "No Limit":
                framerateText.text = "30";
                targetFramerate = 30;
                break;
        }
    }
    //public void resolutionButton()
    //{
    //    switch()
    //    {

    //    }
    //}


    public void setOptions()
    {
        //switch () RESOLUTIONS HERE
        //{
        //    case
        //    break;
        //}
        Application.targetFrameRate = targetFramerate;


    }





}

