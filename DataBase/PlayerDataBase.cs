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

    [Space]
    [SerializeField]
    private int bestSpeedTouchScore = 0;
    [SerializeField]
    private int bestSpeedTouchCombo = 0;

    [Space]
    [SerializeField]
    private int bestMoleCatchScore = 0;
    [SerializeField]
    private int bestMoleCatchCombo = 0;

    [Space]
    [SerializeField]
    private int bestFilpCardScore = 0;
    [SerializeField]
    private int bestFilpCardCombo = 0;

    [Space]
    [SerializeField]
    private int bestButtonActionScore = 0;
    [SerializeField]
    private int bestButtonActionCombo = 0;

    [Space]
    [SerializeField]
    private int bestTimingActionScore = 0;
    [SerializeField]
    private int bestTimingActionCombo = 0;

    [Space]
    [SerializeField]
    private int bestDragActionScore = 0;
    [SerializeField]
    private int bestDragActionCombo = 0;

    [Space]
    [SerializeField]
    private int bestLeftRightScore = 0;
    [SerializeField]
    private int bestLeftRightCombo = 0;

    [Space]
    [SerializeField]
    private int bestCoinRushScore = 0;
    [SerializeField]
    private int bestCoinRushCombo = 0;

    [Title("GameMode")]
    [SerializeField]
    public List<GameModeLevel> gameModeLevelList = new List<GameModeLevel>();

    [Space]
    [Title("Player")]
    [SerializeField]
    private int level = 0;
    [SerializeField]
    private int experience = 0;
    [SerializeField]
    private int icon = 0;
    [SerializeField]
    private int banner = 0;
    [SerializeField]
    private int accessDate = 0;

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
    [SerializeField]
    private int iconBox = 0;

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
    [SerializeField]
    private bool paidProgress = false;
    [SerializeField]
    private bool coinX2 = false;
    [SerializeField]
    private bool expX2 = false;

    [Space]
    [Title("OneTimeDouble")]
    [SerializeField]
    private bool crystal100 = false;
    [SerializeField]
    private bool crystal200 = false;
    [SerializeField]
    private bool crystal300 = false;
    [SerializeField]
    private bool crystal400 = false;
    [SerializeField]
    private bool crystal500 = false;
    [SerializeField]
    private bool crystal600 = false;

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
    private int dailyMissionCount = 0;
    [SerializeField]
    private bool dailyMissionClear = false;
    [Space]
    [SerializeField]
    private List<DailyMission> dailyMissionList = new List<DailyMission>();
    [SerializeField]
    private List<DailyMissionReport> dailyMissionReportList = new List<DailyMissionReport>();


    [Space]
    [Title("Upgrade")]
    [SerializeField]
    private int startTimeLevel = 0;
    [SerializeField]
    private int criticalLevel = 0;
    [SerializeField]
    private int burningLevel = 0;
    [SerializeField]
    private int addExpLevel = 0;
    [SerializeField]
    private int addGoldLevel = 0;
    [SerializeField]
    private int comboTimeLevel = 0;
    [SerializeField]
    private int comboCriticalLevel = 0;
    [SerializeField]
    private int addScoreLevel = 0;

    [Space]
    [Title("Progress")]
    [SerializeField]
    private string freeProgressData = "";
    [SerializeField]
    private string paidProgressData = "";

    [Space]
    [Title("Tutorial")]
    [SerializeField]
    private int lockTutorial = 0;

    public delegate void BoxEvent();
    public static event BoxEvent eGetBox;


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

        bestLeftRightScore = 0;
        bestLeftRightCombo = 0;

        bestCoinRushScore = 0;
        bestCoinRushCombo = 0;

        level = 0;
        experience = 0;
        icon = 0;
        banner = 0;
        accessDate = 0;

        clock = 0;
        shield = 0;
        combo = 0;
        exp = 0;
        slow = 0;
        iconBox = 0;

        attendanceDay = "";
        gameMode = "";

        removeAd = false;
        paidProgress = false;
        coinX2 = false;
        expX2 = false;
        crystal100 = false;
        crystal200 = false;
        crystal300 = false;
        crystal400 = false;
        crystal500 = false;
        crystal600 = false;

        lockTutorial = 0;

        trophyDataList.Clear();

        newsAlarm = 0;

        dailyMissionCount = 0;
        dailyMissionClear = false;

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

        startTimeLevel = 0;
        criticalLevel = 0;
        burningLevel = 0;
        addExpLevel = 0;
        addGoldLevel = 0;
        comboTimeLevel = 0;
        comboCriticalLevel = 0;
        addScoreLevel = 0;

        gameModeLevelList.Clear();

        for (int i = 0; i < System.Enum.GetValues(typeof(GamePlayType)).Length; i++)
        {
            GameModeLevel level = new GameModeLevel();
            level.gamePlayType = GamePlayType.GameChoice1 + i;
            gameModeLevelList.Add(level);
        }

        freeProgressData = "000000000000000000000000000000";
        paidProgressData = "000000000000000000000000000000";
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

    public int BestLeftRightScore
    {
        get
        {
            return bestLeftRightScore;
        }
        set
        {
            bestLeftRightScore = value;
        }
    }

    public int BestLeftRightCombo
    {
        get
        {
            return bestLeftRightCombo;
        }
        set
        {
            bestLeftRightCombo = value;
        }
    }

    public int BestCoinRushScore
    {
        get
        {
            return bestCoinRushScore;
        }
        set
        {
            bestCoinRushScore = value;
        }
    }

    public int BestCoinRushCombo
    {
        get
        {
            return bestCoinRushCombo;
        }
        set
        {
            bestCoinRushCombo = value;
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

    public int Banner
    {
        get
        {
            return banner;
        }
        set
        {
            banner = value;
        }
    }

    public int AccessDate
    {
        get
        {
            return accessDate;
        }
        set
        {
            accessDate = value;
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

    public int IconBox
    {
        get
        {
            return iconBox;
        }
        set
        {
            iconBox = value;

            if(iconBox > 0)
            {
                eGetBox();
            }
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
    public bool PaidProgress
    {
        get
        {
            return paidProgress;
        }
        set
        {
            paidProgress = value;
        }
    }

    public bool CoinX2
    {
        get
        {
            return coinX2;
        }
        set
        {
            coinX2 = value;
        }
    }

    public bool ExpX2
    {
        get
        {
            return expX2;
        }
        set
        {
            expX2 = value;
        }
    }

    public bool Crystal100
    {
        get
        {
            return crystal100;
        }
        set
        {
            crystal100 = value;
        }
    }

    public bool Crystal200
    {
        get
        {
            return crystal200;
        }
        set
        {
            crystal200 = value;
        }
    }

    public bool Crystal300
    {
        get
        {
            return crystal300;
        }
        set
        {
            crystal300 = value;
        }
    }

    public bool Crystal400
    {
        get
        {
            return crystal400;
        }
        set
        {
            crystal400 = value;
        }
    }

    public bool Crystal500
    {
        get
        {
            return crystal500;
        }
        set
        {
            crystal500 = value;
        }
    }

    public bool Crystal600
    {
        get
        {
            return crystal600;
        }
        set
        {
            crystal600 = value;
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

    public string FreeProgressData
    {
        get
        {
            return freeProgressData;
        }
        set
        {
            freeProgressData = value;
        }
    }

    public string PaidProgressData
    {
        get
        {
            return paidProgressData;
        }
        set
        {
            paidProgressData = value;
        }
    }

    public int LockTutorial
    {
        get
        {
            return lockTutorial;
        }
        set
        {
            lockTutorial = value;
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

    public int GetTrophyHoldNumber()
    {
        return trophyDataList.Count;
    }

    public int GetActiveTrophyNumber()
    {
        int number = 0;

        for (int i = 0; i < trophyDataList.Count; i++)
        {
            if (trophyDataList[i].isActive)
            {
                number++;
            }
        }

        return number;
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

    public int DailyMissionCount
    {
        get
        {
            return dailyMissionCount;
        }
        set
        {
            dailyMissionCount = value;
        }
    }

    public bool DailyMissionClear
    {
        get
        {
            return dailyMissionClear;
        }
        set
        {
            dailyMissionClear = value;
        }
    }
    public void OnResetDailyMissionReport()
    {
        DailyMissionCount = 0;
        DailyMissionClear = false;

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

    #region Upgrade
    public int StartTimeLevel
    {
        get
        {
            return startTimeLevel;
        }
        set
        {
            startTimeLevel = value;
        }
    }

    public int CriticalLevel
    {
        get
        {
            return criticalLevel;
        }
        set
        {
            criticalLevel = value;
        }
    }

    public int BurningLevel
    {
        get
        {
            return burningLevel;
        }
        set
        {
            burningLevel = value;
        }
    }

    public int AddExpLevel
    {
        get
        {
            return addExpLevel;
        }
        set
        {
            addExpLevel = value;
        }
    }

    public int AddGoldLevel
    {
        get
        {
            return addGoldLevel;
        }
        set
        {
            addGoldLevel = value;
        }
    }

    public int ComboTimeLevel
    {
        get
        {
            return comboTimeLevel;
        }
        set
        {
            comboTimeLevel = value;
        }
    }

    public int ComboCriticalLevel
    {
        get
        {
            return comboCriticalLevel;
        }
        set
        {
            comboCriticalLevel = value;
        }
    }

    public int AddScoreLevel
    {
        get
        {
            return addScoreLevel;
        }
        set
        {
            addScoreLevel = value;
        }
    }

    public int GetLevel(UpgradeType type)
    {
        int level = 0;

        switch (type)
        {
            case UpgradeType.StartTime:
                level = StartTimeLevel;
                break;
            case UpgradeType.Critical:
                level = CriticalLevel;
                break;
            case UpgradeType.Burning:
                level = BurningLevel;
                break;
            case UpgradeType.AddExp:
                level = AddExpLevel;
                break;
            case UpgradeType.AddGold:
                level = AddGoldLevel;
                break;
            case UpgradeType.ComboTime:
                level = ComboTimeLevel;
                break;
            case UpgradeType.ComboCritical:
                level = ComboCriticalLevel;
                break;
            case UpgradeType.AddScore:
                level = AddScoreLevel;
                break;
        }

        return level;
    }

    #endregion

    public void SetGameMode(GamePlayType type, GameModeLevel level)
    {
        for (int i = 0; i < gameModeLevelList.Count; i++)
        {
            if (gameModeLevelList[i].gamePlayType.Equals(type))
            {
                gameModeLevelList[i] = level;
                break;
            }
        }
    }

    public GameModeLevel GetGameMode(GamePlayType type)
    {
        GameModeLevel level = new GameModeLevel();

        for (int i = 0; i < gameModeLevelList.Count; i++)
        {
            if (gameModeLevelList[i].gamePlayType.Equals(type))
            {
                level = gameModeLevelList[i];
                break;
            }
        }

        return level;
    }

    public void ChangeGameMode(GamePlayType type, GameModeType mode)
    {
        for (int i = 0; i < gameModeLevelList.Count; i++)
        {
            if (gameModeLevelList[i].gamePlayType.Equals(type))
            {
                gameModeLevelList[i].gameModeType = mode;
                break;
            }
        }
    }

    #region Progress
    public void SetProgress(RewardReceiveType type, string str)
    {
        switch (type)
        {
            case RewardReceiveType.Free:
                freeProgressData = str;
                break;
            case RewardReceiveType.Paid:
                paidProgressData = str;
                break;
        }
    }

    public bool GetProgress(RewardReceiveType type, int number)
    {
        bool check = false;
        switch (type)
        {
            case RewardReceiveType.Free:
                if(FreeProgressData.Substring(number, 1).Equals("1")) check = true;
                break;
            case RewardReceiveType.Paid:
                if (PaidProgressData.Substring(number, 1).Equals("1")) check = true;
                break;
        }
        return check;
    }

    public void UpdateProgress(RewardReceiveType type, int number)
    {
        switch (type)
        {
            case RewardReceiveType.Free:
                FreeProgressData = FreeProgressData.ReplaceAt(number, char.Parse("1"));
                break;
            case RewardReceiveType.Paid:
                paidProgressData = paidProgressData.ReplaceAt(number, char.Parse("1"));
                break;
        }
    }
    #endregion
}