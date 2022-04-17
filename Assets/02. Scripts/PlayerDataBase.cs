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

    [SerializeField]
    private int bestCombo = 0;

    [SerializeField]
    private int bestMoleCatchScore = 0;

    [SerializeField]
    private int bestMoleCatchCombo = 0;

    public void Initialize()
    {
        gold = 0;
        crystal = 0;
        bestScore = 0;
        bestCombo = 0;
        bestMoleCatchScore = 0;
        bestMoleCatchCombo = 0;
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

    public int BestCombo
    {
        get
        {
            return bestCombo;
        }
        set
        {
            bestCombo = value;
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

}
