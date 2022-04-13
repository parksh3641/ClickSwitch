using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDataBase", menuName = "ScriptableObjects/PlayerDataBase")]
public class PlayerDataBase : ScriptableObject
{
    [SerializeField]
    private int gold = 0;

    [SerializeField]
    private int crystal = 0;

    [SerializeField]
    private int bestScore = 0;

    public int Gold
    {
        get
        {
            return gold;
        }
        set
        {
            gold = value;
        }
    }

    public int Crystal
    {
        get
        {
            return crystal;
        }
        set
        {
            crystal = value;
        }
    }

    public int BestScore
    {
        get
        {
            return bestScore;
        }
        set
        {
            bestScore = value;
        }
    }

}
