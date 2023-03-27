using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeeklyMission
{
    public WeeklyMissionType weeklyMissonType = WeeklyMissionType.GetScore;
    public int goal = 0;
    public bool clear = false;
}

[System.Serializable]
public class WeeklyMissionReport
{
    public int getScore = 0;
    public int getCombo = 0;
    public int useItem = 0;
    public int gamePlay = 0;
    public int dailyMissonClear = 0;
    public int challengeCoinRush = 0;

    [Title("Receive")]
    public bool receive1 = false;
    public bool receive2 = false;
    public bool receive3 = false;
    public bool receive4 = false;
    public bool receive5 = false;
}

[CreateAssetMenu(fileName = "WeeklyMissionList", menuName = "ScriptableObjects/WeeklyMissionList")]
public class WeeklyMissionList : ScriptableObject
{
    public WeeklyMission[] weeklyMissions;

    public WeeklyMissionReport weeklyMissionReport = new WeeklyMissionReport();



    public void Initialize()
    {
        weeklyMissionReport.getScore = 0;
        weeklyMissionReport.getCombo = 0;
        weeklyMissionReport.useItem = 0;
        weeklyMissionReport.gamePlay = 0;
        weeklyMissionReport.dailyMissonClear = 0;
        weeklyMissionReport.challengeCoinRush = 0;

        weeklyMissionReport.receive1 = false;
        weeklyMissionReport.receive2 = false;
        weeklyMissionReport.receive3 = false;
        weeklyMissionReport.receive4 = false;
        weeklyMissionReport.receive5 = false;
    }

    public void OnResetWeeklyMission()
    {
        for (int i = 0; i < weeklyMissions.Length; i++)
        {
            weeklyMissions[i].clear = false;

        }
    }

    public void SetWeeklyMission(WeeklyMission mission, int number)
    {
        weeklyMissions[number] = mission;
    }

    public WeeklyMission GetWeeklyMission(WeeklyMissionType type)
    {
        WeeklyMission mission = new WeeklyMission();

        for(int i = 0; i < weeklyMissions.Length; i ++)
        {
            if(weeklyMissions[i].weeklyMissonType.Equals(type))
            {
                mission = weeklyMissions[i];
                break;
            }    
        }

        return mission;
    }

    public void SetWeeklyMissionReport(WeeklyMissionReport report)
    {
        weeklyMissionReport = report;
    }

    public WeeklyMissionReport GetWeeklyMissonReport()
    {
        return weeklyMissionReport;
    }

    public int GetWeeklyData(WeeklyMissionType type)
    {
        int number = 0;
        switch (type)
        {
            case WeeklyMissionType.GetScore:
                number = weeklyMissionReport.getScore;
                break;
            case WeeklyMissionType.GetCombo:
                number = weeklyMissionReport.getCombo;
                break;
            case WeeklyMissionType.UseItem:
                number = weeklyMissionReport.useItem;
                break;
            case WeeklyMissionType.GamePlay:
                number = weeklyMissionReport.gamePlay;
                break;
            case WeeklyMissionType.DailyMissionClear:
                number = weeklyMissionReport.dailyMissonClear;
                break;
            case WeeklyMissionType.ChallengeCoinRush:
                number = weeklyMissionReport.challengeCoinRush;
                break;
        }
        return number;
    }
}
