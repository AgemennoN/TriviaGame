using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine;

public class OptionCanvas : MonoBehaviour
{
    private static OptionCanvas instance = null;
    public AudioSource music;
    public GameObject musicVolumeBar;
    public AudioMixer audioMixer;
    public bool TglDynamicBackground;
    public bool TglMusic;
    public bool TglTimerSound;
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
        TglTimerSound = true;

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
        Background = GameObject.Find("DynamicBackground").GetComponent<DynamicBackground>();
        if (TglMusic == true)
        {
            TglMusic = false;
            musicVolumeBar.SetActive(false);
            music.Stop();
        }
        else if (TglMusic == false)
        {
            TglMusic = true;
            musicVolumeBar.SetActive(true);
            music.Play();
        }
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }
  

}
