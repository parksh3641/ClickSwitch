using Sirenix.OdinInspector;
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

    [SerializeField]
    private int bestButtonActionScore = 0;

    [SerializeField]
    private int bestButtonActionCombo = 0;

    [Title("Purchase")]
    public bool removeAd = false;

    [Title("Achievement")]
    [ShowInInspector]
    public List<AchievementData> achievementDataList = new List<AchievementData>();

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
        bestButtonActionScore = 0;
        bestButtonActionCombo = 0;

        removeAd = false;

        achievementDataList.Clear();
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

    public int BestButtonActionScore
    {
        get
        {
            return bestButtonActionScore;
        }
        set
        {
            bestButtonActionScore = value;
        }
    }

    public int BestButtonActionCombo
    {
        get
        {
            return bestButtonActionCombo;
        }
        set
        {
            bestButtonActionCombo = value;
        }
    }

    public bool RemoveAd
    {
        get
        {
            return removeAd;
        }
        set
        {
            removeAd = value;
        }
    }


    public void OnSetAchievementContent(AchievementData content)
    {
        achievementDataList.Add(content);
    }

    public bool GetPerfectMode(GamePlayType type)
    {
        int index = 0;
        bool check = false;

        for(int i = 0; i < achievementDataList.Count; i ++)
        {
            if(achievementDataList[i].achievementType.Equals(type))
            {
                index = achievementDataList[i].achievementList[0];        
            }
        }

        if (index == 0) check = false;
        else check = true;

        return check;

    }
}
