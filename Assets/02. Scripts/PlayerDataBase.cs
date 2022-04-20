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
    private int totalScore = 0;

    [SerializeField]
    private int bestSpeedTouchScore = 0;

    [SerializeField]
    private int bestSpeedTouchCombo = 0;

    [SerializeField]
    private int bestMoleCatchScore = 0;

    [SerializeField]
    private int bestMoleCatchCombo = 0;

    [SerializeField]
    private int bestFilpCardScore = 0;

    [SerializeField]
    private int bestFilpCardCombo = 0;

    public void Initialize()
    {
        gold = 0;
        crystal = 0;
        totalScore = 0;
        bestSpeedTouchScore = 0;
        bestSpeedTouchCombo = 0;
        bestMoleCatchScore = 0;
        bestMoleCatchCombo = 0;
        bestFilpCardScore = 0;
        bestFilpCardCombo = 0;
    }

    public int TotalScore
    {
        get
        {
            return totalScore;
        }
        set
        {
            totalScore = value;
        }
    }

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

    public int BestSpeedTouchScore
    {
        get
        {
            return bestSpeedTouchScore;
        }
        set
        {
            bestSpeedTouchScore = value;
        }
    }

    public int BestSpeedTouchCombo
    {
        get
        {
            return bestSpeedTouchCombo;
        }
        set
        {
            bestSpeedTouchCombo = value;
        }
    }

    public int BestMoleCatchScore
    {
        get
        {
            return bestMoleCatchScore;
        }
        set
        {
            bestMoleCatchScore = value;
        }
    }

    public int BestMoleCatchCombo
    {
        get
        {
            return bestMoleCatchCombo;
        }
        set
        {
            bestMoleCatchCombo = value;
        }
    }

    public int BestFilpCardCombo
    {
        get
        {
            return bestFilpCardCombo;
        }
        set
        {
            bestFilpCardCombo = value;
        }
    }

    public int BestFilpCardScore
    {
        get
        {
            return bestFilpCardScore;
        }
        set
        {
            bestFilpCardScore = value;
        }
    }

}
