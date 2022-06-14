using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource musicAudio;

    public AudioSource[] sfxAudio;

    public SoundDataBase soundDataBase;

    public void Awake()
    {
        GameStateManager.eMusic += OnSwitchMusic;
        GameStateManager.eSfx += OnSwitchSFX;

        if (soundDataBase == null) soundDataBase = Resources.Load("SoundDataBase") as SoundDataBase;

    }

    public void Start()
    {
        GameManager.eGameStart += this.GameStart;

        HighTimer();

        OnSwitchMusic(GameStateManager.instance.Music);
        OnSwitchSFX(GameStateManager.instance.Sfx);

        PlaySound(GameSoundType.Lobby);
    }

    void GameStart()
    {
        PlaySound(GameSoundType.Main);
    }

    private void OnApplicationQuit()
    {
        GameManager.eGameStart -= this.GameStart;
    }

    public void OnSwitchMusic(bool check)
    {
        if(check)
        {
            musicAudio.volume = 1;

            musicAudio.Stop();
            musicAudio.Play();
        }
        else
        {
            musicAudio.volume = 0;
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

    public void PlaySound(GameSoundType type)
    {
        int random = 0;

        musicAudio.Stop();

        switch (type)
        {
            case GameSoundType.Lobby:
                if (soundDataBase.lobbyAudioClipList.Count == 0)
                {
                    return;
                }

                random = Random.Range(0, soundDataBase.lobbyAudioClipList.Count);

                musicAudio.clip = soundDataBase.lobbyAudioClipList[random];
                break;
            case GameSoundType.Main:
                if (soundDataBase.mainAudioClipList.Count == 0)
                {
                    return;
                }

                random = Random.Range(0, soundDataBase.mainAudioClipList.Count);

                musicAudio.clip = soundDataBase.mainAudioClipList[random];

                break;
            case GameSoundType.End:
                if (soundDataBase.endAudioClipList.Count == 0)
                {
                    return;
                }

                random = Random.Range(0, soundDataBase.endAudioClipList.Count);

                musicAudio.clip = soundDataBase.endAudioClipList[random];

                break;
        }

        musicAudio.Play();
    }


    public void HighTimer()
    {
        musicAudio.pitch = 1;
    }
    public void LowTimer()
    {
        if(musicAudio.pitch != 1.05f)
        {
            musicAudio.pitch = 1.05f;
        }
    }

}
