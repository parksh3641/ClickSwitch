using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "DailyMissionList", menuName = "ScriptableObjects/DailyMissionList")]
public class DailyMissionList : ScriptableObject
{
    public DailyMission[] dailyMissions;
}

[System.Serializable]
public class DailyMission
{
    public GamePlayType gamePlayType = GamePlayType.GameChoice1;
    public MissionType missionType = MissionType.QuestDoPlay;
    public int goal = 0;
    public bool clear = false;
}

[System.Serializable]
public class DailyMissionReport
{
    public GamePlayType gamePlayType = GamePlayType.GameChoice1;
    public int doPlay = 0;
    public int getScore = 0;
    public int getCombo = 0;
}