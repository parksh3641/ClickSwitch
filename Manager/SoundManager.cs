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

        PlayBGM(GameBGMType.Lobby);
    }

    void GameStart()
    {
        PlayBGM(GameBGMType.Main);
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
                sfxAudio[1].volume = 0.5f;
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

    public void StopBGM()
    {
        musicAudio.Stop();
    }

    public void PlayBGM(GameBGMType type)
    {
        int random = 0;

        musicAudio.Stop();

        switch (type)
        {
            case GameBGMType.Lobby:
                if (soundDataBase.lobbyAudioClipList.Count == 0)
                {
                    return;
                }

                random = Random.Range(0, soundDataBase.lobbyAudioClipList.Count);

                musicAudio.pitch = 1;

                musicAudio.clip = soundDataBase.lobbyAudioClipList[random];
                break;
            case GameBGMType.Main:
                if (soundDataBase.mainAudioClipList.Count == 0)
                {
                    return;
                }

                random = Random.Range(0, soundDataBase.mainAudioClipList.Count);

                musicAudio.clip = soundDataBase.mainAudioClipList[random];

                break;
            case GameBGMType.End:
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

    public void PlaySFX(GameSfxType type)
    {
        for(int i = 0; i < sfxAudio.Length; i ++)
        {
            if(sfxAudio[i].name.Equals(type.ToString()))
            {
                if(!sfxAudio[i].isPlaying) sfxAudio[i].Play();
            }
        }
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
