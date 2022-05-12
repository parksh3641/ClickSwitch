using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundDataBase", menuName = "ScriptableObjects/SoundDataBase")]
public class SoundDataBase : ScriptableObject
{
    [Title("Lobby")]
    public List<AudioClip> lobbyAudioClipList = new List<AudioClip>();

    [Space]
    [Title("Main")]
    public List<AudioClip> mainAudioClipList = new List<AudioClip>();

    [Space]
    [Title("End")]
    public List<AudioClip> endAudioClipList = new List<AudioClip>();


    private void Awake()
    {
        
    }
}
