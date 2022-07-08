using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDataBase", menuName = "ScriptableObjects/PlayerDataBase")]
public class PlayerDataBase : ScriptableObject
{
    [Title("Money")]
    [SerializeField]
    private int coin = 0;

    [SerializeField]
    private int crystal = 0;

    [Space]
    [SerializeField]
    private int totalScore = 0;

    [SerializeField]
    private int totalCombo = 0;

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

    [SerializeField]
    private int bestTimingActionScore = 0;

    [SerializeField]
    private int bestTimingActionCombo = 0;

    [SerializeField]
    private int bestDragActionScore = 0;

    [SerializeField]
    private int bestDragActionCombo = 0;

    [Space]
    [Title("Level")]
    [SerializeField]
    private int level = 0;

    [SerializeField]
    private int experience = 0;

    [SerializeField]
    private int icon = 0;


    [Space]
    [Title("Item")]
    [SerializeField]
    private int clock = 0;
    [SerializeField]
    private int shield = 0;
    [SerializeField]
    private int combo = 0;
    [SerializeField]
    private int exp = 0;
    [SerializeField]
    private int slow = 0;

    [Space]
    [Title("Reset")]
    [SerializeField]
    private string attendanceDay = "";
    [SerializeField]
    private string gameMode = "";

    [Space]
    [Title("Purchase")]
    [SerializeField]
    private bool removeAd = false;

    [Space]
    [Title("Trophy")]
    [SerializeField]
    private List<TrophyData> trophyDataList = new List<TrophyData>();

    [Space]
    [Title("Alarm")]
    [SerializeField]
    private int newsAlarm;

    [Space]
    [Title("DailyMission")]
    [SerializeField]
    private List<DailyMission> dailyMissionList = new List<DailyMission>();

    [SerializeField]
    private List<DailyMissionReport> dailyMissionReportList = new List<DailyMissionReport>();


    public void Initialize()
    {
        coin = 0;
        crystal = 0;

        totalScore = 0;
        totalCombo = 0;

        bestSpeedTouchScore = 0;
        bestSpeedTouchCombo = 0;

        bestMoleCatchScore = 0;
        bestMoleCatchCombo = 0;

        bestFilpCardScore = 0;
        bestFilpCardCombo = 0;

        bestButtonActionScore = 0;
        bestButtonActionCombo = 0;

        bestTimingActionScore = 0;
        bestTimingActionCombo = 0;

        bestDragActionScore = 0;
        bestDragActionCombo = 0;

        level = 0;
        experience = 0;
        icon = 0;

        clock = 0;
        shield = 0;
        combo = 0;
        exp = 0;
        slow = 0;

        attendanceDay = "";
        gameMode = "";

        removeAd = false;

        trophyDataList.Clear();

        newsAlarm = 0;

        dailyMissionList.Clear();

        for(int i = 0; i < 3; i ++)
        {
            DailyMission dailyMission = new DailyMission();
            dailyMissionList.Add(dailyMission);
        }

        if(dailyMissionReportList.Count < System.Enum.GetValues(typeof(GamePlayType)).Length)
        {
            OnResetDailyMissionReport();
        }
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

    public int TotalCombo
    {
        get
        {
            return totalCombo;
        }
        set
        {
            totalCombo = value;
        }
    }

    public int Coin
    {
        get
        {
            return coin;
        }
        set
        {
            coin = value;
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

    public int BestTimingActionScore
    {
        get
        {
            return bestTimingActionScore;
        }
        set
        {
            bestTimingActionScore = value;
        }
    }

    public int BestTimingActionCombo
    {
        get
        {
            return bestTimingActionCombo;
        }
        set
        {
            bestTimingActionCombo = value;
        }
    }

    public int BestDragActionScore
    {
        get
        {
            return bestDragActionScore;
        }
        set
        {
            bestDragActionScore = value;
        }
    }

    public int BestDragActionCombo
    {
        get
        {
            return bestDragActionCombo;
        }
        set
        {
            bestDragActionCombo = value;
        }
    }

    public int Level
    {
        get
        {
            return level;
        }
        set
        {
            level = value;
        }
    }

    public int Experience
    {
        get
        {
            return experience;
        }
        set
        {
            experience = value;
        }
    }

    public int Icon
    {
        get
        {
            return icon;
        }
        set
        {
            icon = value;
        }
    }

    public int Clock
    {
        get
        {
            return clock;
        }
        set
        {
            clock = value;
        }
    }

    public int Shield
    {
        get
        {
            return shield;
        }
        set
        {
            shield = value;
        }
    }

    public int Combo
    {
        get
        {
            return combo;
        }
        set
        {
            combo = value;
        }
    }

    public int Exp
    {
        get
        {
            return exp;
        }
        set
        {
            exp = value;
        }
    }

    public int Slow
    {
        get
        {
            return slow;
        }
        set
        {
            slow = value;
        }
    }

    public string AttendanceDay
    {
        get
        {
            return attendanceDay;
        }
        set
        {
            attendanceDay = value;
        }
    }

    public string GameMode
    {
        get
        {
            return gameMode;
        }
        set
        {
            gameMode = value;
        }
    }

    public int GetItemCount(ItemType type)
    {
        int count = 0;

        switch (type)
        {
            case ItemType.Clock:
                count = Clock;
                break;
            case ItemType.Shield:
                count = Shield;
                break;
            case ItemType.Combo:
                count = Combo;
                break;
            case ItemType.Exp:
                count = Exp;
                break;
            case ItemType.Slow:
                count = Slow;
                break;
        }

        return count;
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

    public int NewsAlarm
    {
        get
        {
            return newsAlarm;
        }
        set
        {
            newsAlarm = value;
        }
    }

    public void SetTrophyData(TrophyData data)
    {
        trophyDataList.Add(data);
    }

    public bool GetTrophyIsAcive(GamePlayType type)
    {
        bool check = false;

        for (int i = 0; i < trophyDataList.Count; i++)
        {
            if (trophyDataList[i].gamePlayType.Equals(type))
            {
                check = trophyDataList[i].isActive;
            }
        }

        return check;
    }

    public TrophyData GetTrophyData(GamePlayType type)
    {
        TrophyData trophyData = new TrophyData();

        for(int i = 0; i < trophyDataList.Count; i ++)
        {
            if(trophyDataList[i].gamePlayType.Equals(type))
            {
                trophyData = trophyDataList[i];
            }
        }

        return trophyData;
    }

    public void SetItemCount(ItemType type, int number)
    {
        switch (type)
        {
            case ItemType.Clock:
                Clock += number;
                break;
            case ItemType.Shield:
                Shield += number;
                break;
            case ItemType.Combo:
                Combo += number;
                break;
            case ItemType.Exp:
                Exp += number;
                break;
            case ItemType.Slow:
                Slow += number;
                break;
        }

        StateManager.instance.ChangeNumber();
    }


    #region DailyMission
    public void OnResetDailyMissionReport()
    {
        dailyMissionReportList.Clear();

        for (int i = 0; i < System.Enum.GetValues(typeof(GamePlayType)).Length; i++)
        {
            DailyMissionReport report = new DailyMissionReport();
            report.gamePlayType = GamePlayType.GameChoice1 + i;
            dailyMissionReportList.Add(report);
        }
    }
    public void SetDailyMission(DailyMission dailyMission, int number)
    {
        dailyMissionList[number] = dailyMission;
    }

    public void SetDailyMissionClear(DailyMission dailyMission)
    {
        for(int i = 0; i < dailyMissionList.Count; i ++)
        {
            if(dailyMissionList[i].gamePlayType.Equals(dailyMission.gamePlayType))
            {
                dailyMissionList[i].clear = true;
            }
        }
    }    

    public DailyMission GetDailyMission(int number)
    {
        return dailyMissionList[number];
    }

    public void SetDailyMissionReport(DailyMissionReport report)
    {
        for(int i = 0; i <dailyMissionReportList.Count; i ++)
        {
            if(dailyMissionReportList[i].gamePlayType.Equals(report.gamePlayType))
            {
                dailyMissionReportList[i] = report;
            }
        }
    }

    public DailyMissionReport GetDailyMissionReport(GamePlayType type)
    {
        return dailyMissionReportList[(int)type];
    }

    public int GetDailyMissionReportValue(int number)
    {
        int value = 0;

        for(int i = 0; i < dailyMissionReportList.Count; i ++)
        {
            if(dailyMissionList[number].gamePlayType.Equals(dailyMissionReportList[i].gamePlayType))
            {
                switch (dailyMissionList[number].missionType)
                {
                    case MissionType.QuestDoPlay:
                        value = dailyMissionReportList[i].doPlay;
                        break;
                    case MissionType.QuestScore:
                        value = dailyMissionReportList[i].getScore;
                        break;
                    case MissionType.QuestCombo:
                        value = dailyMissionReportList[i].getCombo;
                        break;
                }
                break;
            }
        }

        return value;
    }

    #endregion
}
