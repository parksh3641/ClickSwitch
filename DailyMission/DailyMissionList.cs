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
    public MissionType missionType = MissionType.Score;
    public int goal = 0;
    public int rewardCount = 1;
    public MissionReward[] missionRewards = new MissionReward[3];
}

public class DailyMissionJson
{
    public GamePlayType gamePlayType = GamePlayType.GameChoice1;
    public MissionType missionType = MissionType.Score;
    public int goal = 0;
    public bool isClear = false;
}

[System.Serializable]
public class MissionReward
{
    public RewardType rewardType = RewardType.Coin;
    public int rewardNumber = 0;
}

public class MissionRewardContent
{
    public GameObject obj;
    public Image icon;
    public Text rewardNumberText;
}