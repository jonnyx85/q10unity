using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

[System.Serializable]
public class SoundManager : MonoBehaviour
{
    public AudioMixer mainMixer;
    public AudioSource BtnClick, PopSound;
    public Slider volumeSlider;

    public static SoundManager Instance;

    private void Awake()
    {
        if(Instance==null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public void Start()
    {
        mainMixer.SetFloat("Vol", PlayerPrefs.GetFloat("Vol", 0f));
        volumeSlider.value = PlayerPrefs.GetFloat("Vol", 0f);
    }

    public void ChnageVolume(float Vol)
    {
        PlayerPrefs.SetFloat("Vol", Vol);
        mainMixer.SetFloat("Vol", Vol);
        volumeSlider.value = Vol;
    }    
    public void PlayClickSound()
    {
        BtnClick.Play();
    }
    public void PlayPopSound()
    {
        PopSound.Play();
    }
}
