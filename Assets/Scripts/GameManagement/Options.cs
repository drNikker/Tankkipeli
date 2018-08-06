using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;



public class Options : MonoBehaviour
{

    
    public AudioMixer masterMixer;

    public Animator anim;

    //Settings menu textboxes
    public Text resoText;
    public Text framerateText;
    public Text windowMode;
    public Text masterVolText;
    public Text musicVolText;
    public Text sfxVolText;
    public Text upText;
    public Text turnRightText;
    public Text turnLeftText;
    public Text downText;

    //Controls menu textboxes
    public Text playerText;
    public Text moveDirText;
    public Text turnDirText;

    private float masterVolFloat;
    private float musicVolFloat;
    private float sfxVolFloat;
    private int targetFramerate = 30;
    private int resoVar;
    private bool fullScreen = true;

    private string resolutionChoicePref = "ResolutionChoicePref_";
    private string masterVolumePref = "MasterVolPref_";
    private string musicVolumePref = "MusicVolPref_";
    private string sfxVolumePref = "SFXVolPref_";
    private string frameratePref = "FrameratePref_";
    private string moveDirPref = "_MoveDirPref_";
    private string turnDirPref = "_TurnDirPref_";


    private void Start()
    {
        playerText.text = "P1 Red";
        GetOptions();
    }
    
    //SET AND GET options

    public void SetOptions()
    {
        PlayerPrefs.SetFloat(masterVolumePref, masterVolFloat);
        PlayerPrefs.SetFloat(musicVolumePref, musicVolFloat);
        PlayerPrefs.SetFloat(sfxVolumePref, sfxVolFloat);
        PlayerPrefs.SetInt(frameratePref, targetFramerate);
        PlayerPrefs.SetInt(resolutionChoicePref, resoVar);

        SetReso();
        Application.targetFrameRate = targetFramerate;

        PlayerPrefs.Save();

    }

    public void GetOptions()
    {
        masterVolFloat = PlayerPrefs.GetFloat(masterVolumePref, 50f);
        masterMixer.SetFloat("MasterVol", GetDecibel(masterVolFloat));

        musicVolFloat = PlayerPrefs.GetFloat(musicVolumePref, 50f);
        masterMixer.SetFloat("MusicVol", GetDecibel(musicVolFloat));

        sfxVolFloat = PlayerPrefs.GetFloat(sfxVolumePref, 50f);
        masterMixer.SetFloat("SFXVol", GetDecibel(sfxVolFloat));

        resoVar = PlayerPrefs.GetInt(resolutionChoicePref, 2);
        targetFramerate = PlayerPrefs.GetInt(frameratePref, 30);

        SetTextBoxes();
        SetControlOptions();

    }

    // set options textboxes
    private void SetTextBoxes()
    {
        SetResoText();
        SetFramerateText();
        SetScreenMode();
        masterVolText.text = masterVolFloat.ToString();
        musicVolText.text = musicVolFloat.ToString();
        sfxVolText.text = sfxVolFloat.ToString();

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

    //Volume buttons. Increases values and immediately sets and saves values to playerprefs

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
        PlayerPrefs.SetFloat(masterVolumePref, masterVolFloat);
        
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
        PlayerPrefs.SetFloat(musicVolumePref, musicVolFloat);
        
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
        PlayerPrefs.SetFloat(sfxVolumePref, sfxVolFloat);
    }

    //Framerate setting
    //updates framerate variable
    public void FramerateButton()
    {
        switch(targetFramerate)
        {
            case 30:
                targetFramerate = 60;
                break;
            case 60:
                targetFramerate = 120;
                break;
            case 120:
                targetFramerate = 144;
                break;
            case 144:
                targetFramerate = 600;
                break;
            case 600:
                targetFramerate = 30;
                break;

        }

        SetFramerateText();
    }
    //updates framerate textbox
    private void SetFramerateText()
    {
        switch (targetFramerate)
        {
            case 30:
                framerateText.text = "30";
                break;
            case 60:
                framerateText.text = "60";
                break;
            case 120:
                framerateText.text = "120";
                break;
            case 144:
                framerateText.text = "144";
                break;
            case 600:
                framerateText.text = "No Limit";
                break;
        }
    }


    //Resolution and ScreenMode settings
    public void ResoButton()
    {
        if (resoVar < 6)
        {
            resoVar += 1;
        }
        else
        {
            resoVar = 0;
        }
        SetResoText();
        Debug.Log(resoVar);
    }

    //sets resolution text boxes according to resoVar
    private void SetResoText()
    {
        switch (resoVar)
        {
            case 0:
                resoText.text = "3840x2160";
                break;
            case 1:
                resoText.text = "2560x1440";
                break;
            case 2:
                resoText.text = "1920x1080";
                break;
            case 3:
                resoText.text = "1600x900";
                break;
            case 4:
                resoText.text = "1366x768";
                break;
            case 5:
                resoText.text = "1280x720";
                break;
            case 6:
                resoText.text = "1024x576";
                break;
            default:
                resoText.text = "-";
                Debug.Log("This shouldn't happen");
                break;
        }
    }

    //actually sets resolution and window mode
    private void SetReso()
    {
        switch (resoVar)
        {
            case 0:
                Screen.SetResolution(3840, 2160, fullScreen);
                break;
            case 1:
                Screen.SetResolution(2560, 1440, fullScreen);
                break;
            case 2:
                Screen.SetResolution(1920, 1080, fullScreen);
                break;
            case 3:
                Screen.SetResolution(1600, 900, fullScreen);
                break;
            case 4:
                Screen.SetResolution(1366, 768, fullScreen);
                break;
            case 5:
                Screen.SetResolution(1280, 720, fullScreen);
                break;
            case 6:
                Screen.SetResolution(1024, 576, fullScreen);
                break;
            default:
                Screen.SetResolution(1920, 1080, fullScreen);
                break;
        }
    }


    public void ScreenModeButton()
    {
        if (fullScreen == false)
        {
            fullScreen = true;
        }
        else
        {
            fullScreen = false;
        }
        SetScreenMode();
    }

    private void SetScreenMode()
    {
        if (fullScreen == false)
        {
            windowMode.text = "Windowed";
        }
        else
        {
            windowMode.text = "FullScreen";
        }
    }

    // PLAYER SETTINGS
    
        
    // REMEMBER!!! player names are used in calling playerfPrefs in PhysicMovement1 Script aswell to get movement
    // directions, so remember to change them there as well if you change player texts here.

    private void SetControlOptions()
    {
        
        switch (playerText.text)
        {
            case "P1 Red":
                if (PlayerPrefs.HasKey(playerText.text + moveDirPref))
                {
                    if (PlayerPrefs.GetInt(playerText.text + moveDirPref) == -1)
                    {
                        moveDirText.text = "Bumpers/Triggers";
                        upText.text = "Backwards";
                        downText.text = "Forwards";
                    }
                    else
                    {
                        moveDirText.text = "Triggers/Bumpers";
                        upText.text = "Forwards";
                        downText.text = "Backwards";
                    }
                }
                else
                {
                    moveDirText.text = "Triggers/Bumpers";
                    upText.text = "Forwards";
                    downText.text = "Backwards";
                }


                if (PlayerPrefs.HasKey(playerText.text + turnDirPref))
                {
                    if (PlayerPrefs.GetInt(playerText.text + turnDirPref) == 0)
                    {
                        turnDirText.text = "Inverted";
                        turnLeftText.text = "Turn Right";
                        turnRightText.text = "Turn Left";
                    }
                    else
                    {
                        turnDirText.text = "Normal";
                        turnLeftText.text = "Turn Left";
                        turnRightText.text = "Turn Right";
                    }
                }
                else
                {
                    turnDirText.text = "Normal";
                    turnLeftText.text = "Turn Left";
                    turnRightText.text = "Turn Right";
                }
                break;


            case "P2 Blue":
                if (PlayerPrefs.HasKey(playerText.text + moveDirPref))
                {
                    if (PlayerPrefs.GetInt(playerText.text + moveDirPref) == -1)
                    {
                        moveDirText.text = "Bumpers/Triggers";
                        upText.text = "Backwards";
                        downText.text = "Forwards";
                    }
                    else
                    {
                        moveDirText.text = "Triggers/Bumpers";
                        upText.text = "Forwards";
                        downText.text = "Backwards";
                    }
                }
                else
                {
                    moveDirText.text = "Triggers/Bumpers";
                    upText.text = "Forwards";
                    downText.text = "Backwards";
                }

                if (PlayerPrefs.HasKey(playerText.text + turnDirPref))
                {
                    if (PlayerPrefs.GetInt(playerText.text + turnDirPref) == 0)
                    {
                        turnDirText.text = "Inverted";
                        turnLeftText.text = "Turn Right";
                        turnRightText.text = "Turn Left";
                    }
                    else
                    {
                        turnDirText.text = "Normal";
                        turnLeftText.text = "Turn Left";
                        turnRightText.text = "Turn Right";
                    }
                }
                else
                {
                    turnDirText.text = "Normal";
                    turnLeftText.text = "Turn Left";
                    turnRightText.text = "Turn Right";
                }
                break;


            case "P3 Cyan":
                if (PlayerPrefs.HasKey(playerText.text + moveDirPref))
                {
                    if (PlayerPrefs.GetInt(playerText.text + moveDirPref) == -1)
                    {
                        moveDirText.text = "Bumpers/Triggers";
                        upText.text = "Backwards";
                        downText.text = "Forwards";
                    }
                    else
                    {
                        moveDirText.text = "Triggers/Bumpers";
                        upText.text = "Forwards";
                        downText.text = "Backwards";
                    }
                }
                else
                {
                    moveDirText.text = "Triggers/Bumpers";
                    upText.text = "Forwards";
                    downText.text = "Backwards";
                }

                if (PlayerPrefs.HasKey(playerText.text + turnDirPref))
                {
                    if (PlayerPrefs.GetInt(playerText.text + turnDirPref) == 0)
                    {
                        turnDirText.text = "Inverted";
                        turnLeftText.text = "Turn Right";
                        turnRightText.text = "Turn Left";
                    }
                    else
                    {
                        turnDirText.text = "Normal";
                        turnLeftText.text = "Turn Left";
                        turnRightText.text = "Turn Right";
                    }
                }
                else
                {
                    turnDirText.text = "Normal";
                    turnLeftText.text = "Turn Left";
                    turnRightText.text = "Turn Right";
                }
                break;


            case "P4 Yellow":
                if (PlayerPrefs.HasKey(playerText.text + moveDirPref))
                {
                    if (PlayerPrefs.GetInt(playerText.text + moveDirPref) == -1)
                    {
                        moveDirText.text = "Bumpers/Triggers";
                        upText.text = "Backwards";
                        downText.text = "Forwards";
                    }
                    else
                    {
                        moveDirText.text = "Triggers/Bumpers";
                        upText.text = "Forwards";
                        downText.text = "Backwards";
                    }
                }
                else
                {
                    moveDirText.text = "Triggers/Bumpers";
                    upText.text = "Forwards";
                    downText.text = "Backwards";
                }

                if (PlayerPrefs.HasKey(playerText.text + turnDirPref))
                {
                    if (PlayerPrefs.GetInt(playerText.text + turnDirPref) == 0)
                    {
                        turnDirText.text = "Inverted";
                        turnLeftText.text = "Turn Right";
                        turnRightText.text = "Turn Left";
                    }
                    else
                    {
                        turnDirText.text = "Normal";
                        turnLeftText.text = "Turn Left";
                        turnRightText.text = "Turn Right";
                    }
                }
                else
                {
                    turnDirText.text = "Normal";
                    turnLeftText.text = "Turn Left";
                    turnRightText.text = "Turn Right";
                }
                break;
        }
        
    }

    public void ChangePlayerButton()
    {
        switch (playerText.text)
        {
            case "P1 Red":
                playerText.text = "P2 Blue";
                break;
            case "P2 Blue":
                playerText.text = "P3 Cyan";
                break;
            case "P3 Cyan":
                playerText.text = "P4 Yellow";
                break;
            case "P4 Yellow":
                playerText.text = "P1 Red";
                break;
        }
        SetControlOptions();
    }
    
    // PlayerPrefs.SetInt(resolutionChoicePref, resoVar);
    
    // move and turn dir buttons to update texts in menu and set values to playerprefs
    //move dir changes which way bumpers and triggers move the treads
    public void MoveDirButton()
    {
        if (moveDirText.text == "Triggers/Bumpers")
        {
            moveDirText.text = "Bumpers/Triggers";
            upText.text = "Backwards";
            downText.text = "Forwards";
            PlayerPrefs.SetInt(playerText.text + moveDirPref, -1);
        }
        else
        {
            moveDirText.text = "Triggers/Bumpers";
            upText.text = "Forwards";
            downText.text = "Backwards";
            PlayerPrefs.SetInt(playerText.text + moveDirPref, 1);
        }
        SetplayerControls();
    }

    //turn dir changes which side each side of triggers andd bumpers controls
    public void TurnDirButton()
    {
        if (turnDirText.text == "Normal")
        {
            turnDirText.text = "Inverted";
            turnLeftText.text = "Turn Right";
            turnRightText.text = "Turn Left";
            PlayerPrefs.SetInt(playerText.text + turnDirPref, 0);
        }
        else
        {
            turnDirText.text = "Normal";
            turnLeftText.text = "Turn Left";
            turnRightText.text = "Turn Right";
            PlayerPrefs.SetInt(playerText.text + turnDirPref, 1);
        }
        SetplayerControls();
    }
    
    //sets players controls when called.
    private void SetplayerControls()
    {
        PhysicMovement1[] PM1List = FindObjectsOfType<PhysicMovement1>();
        foreach (PhysicMovement1 item in PM1List)
        {
            item.SetControls();
        }
    }
    
    public void QuitButtonYes()
    {
#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }

    public void MenuButtonYes()
    {
        SceneManager.LoadScene("JoiningScene");
    }

 
}

