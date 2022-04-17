using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource musicAudio;

    public AudioSource[] sfxAudio;

    public void Awake()
    {
        GameStateManager.eMusic += OnSwitchMusic;
        GameStateManager.eSfx += OnSwitchSFX;
    }

    public void Start()
    {
        OnSwitchMusic(GameStateManager.instance.Music);
        OnSwitchSFX(GameStateManager.instance.Sfx);
    }


    public void OnSwitchMusic(bool check)
    {
        if(check)
        {
            musicAudio.Play();
        }
        else
        {
            musicAudio.Pause();
        }
    }

    public void OnSwitchSFX(bool check)
    {
        if (check)
        {
            for(int i = 0; i < sfxAudio.Length; i ++)
            {
                sfxAudio[i].volume = 1;
            }
        }
        else
        {
            for (int i = 0; i < sfxAudio.Length; i++)
            {
                sfxAudio[i].volume = 0;
            }
        }
    }

}
