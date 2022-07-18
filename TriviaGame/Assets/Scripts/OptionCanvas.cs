using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class OptionCanvas : MonoBehaviour
{
    private static OptionCanvas instance = null;
    public AudioSource music;
    public Slider MusicVolumeBar;
    public Slider SFxVolumeBar;
    public TMP_InputField ClockSoundStartAtInput;
    public AudioMixer audioMixer;
    
    public bool TglDynamicBackground;
    public bool TglMusic;
    public bool TglSFx;

    private DynamicBackground Background;



    public static OptionCanvas Instance { get { return instance; } }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(transform.gameObject);


        TglDynamicBackground = true;
        TglMusic = true;
        TglSFx = true;

    }

    public void OptionToMainMenu()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)  // If already at main menu
        {
            gameObject.GetComponent<Canvas>().enabled = false;  // Just disable the Option Canvas
        }
        else        // In GameScene find GameManager and call ReturnMenuBtn() function
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().ReturnMenuBtn();
            gameObject.GetComponent<Canvas>().enabled = false;  // and disable the Option Canvas
        }
    }

    public void ToggleDynamicBackground()
    {
        Background = GameObject.Find("DynamicBackground").GetComponent<DynamicBackground>();
        if (TglDynamicBackground == true)
        {
            TglDynamicBackground = false;
            Background.ToggledOff();
        }
        else if (TglDynamicBackground == false)
        {
            TglDynamicBackground = true;
            Background.ToggledOn();
        }
    }

    public void ToggleMusic()
    {
        if (TglMusic == true)
        {
            TglMusic = false;
            MusicVolumeBar.interactable = false;
            music.Stop();
        }
        else if (TglMusic == false)
        {
            TglMusic = true;
            MusicVolumeBar.interactable = true;
            music.Play();
        }
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
    }


    public void ToggleSFx()
    {
        if (TglSFx == true)
        {
            TglSFx = false;
            SFxVolumeBar.interactable = false;
            ClockSoundStartAtInput.interactable = false;
        }
        else if (TglSFx == false)
        {
            TglSFx = true;
            SFxVolumeBar.interactable = true;
            ClockSoundStartAtInput.interactable = true;
        }
        
        if (SceneManager.GetActiveScene().buildIndex == 1)  //If on GameScene Change values in GameManager;
        {
            GameManager GM = GameObject.Find("GameManager").GetComponent<GameManager>();
            GM.TglSFx = TglSFx;

        }

    }

    public void SetSFxVolume(float volume)
    {
        audioMixer.SetFloat("SFxVolume", volume);
    }

    public void ClockSoundStartAtInputChange()
    {
        Debug.Log("VALUE IS CHANGED");
        if (SceneManager.GetActiveScene().buildIndex == 1)  //If on GameScene Change values in GameManager;
        {
            Debug.Log("Inside the if");
            GameManager GM = GameObject.Find("GameManager").GetComponent<GameManager>();
            Debug.Log("Before: GM.TimeCounterSoundStart: " + GM.TimeCounterSoundStart);

            int.TryParse(ClockSoundStartAtInput.text, out GM.TimeCounterSoundStart);
            Debug.Log("After: GM.TimeCounterSoundStart: " + GM.TimeCounterSoundStart);
        }

    }
}
