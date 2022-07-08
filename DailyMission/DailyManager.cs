using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyManager : MonoBehaviour
{
    public GameObject dailyView;

    public DailyContent[] dailyContents;


    List<int> missionIndexs = new List<int>();
    Dictionary<string, string> dailyMissionData = new Dictionary<string, string>();


    DailyMissionList dailyMissionList;
    PlayerDataBase playerDataBase;


    private void Awake()
    {
        if (dailyMissionList == null) dailyMissionList = Resources.Load("DailyMissionList") as DailyMissionList;
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;

        dailyView.SetActive(false);
    }

    private void OnEnable()
    {
        GameManager.eGameEnd += UpdateDailyMission;
    }
    private void OnDisable()
    {
        GameManager.eGameEnd -= UpdateDailyMission;
    }

    public void OpenDaily()
    {
        if (!dailyView.activeSelf)
        {
            dailyView.SetActive(true);

            CheckDailyMission();
        }
        else
        {
            dailyView.SetActive(false);
        }
    }

    public void CheckDailyMission()
    {
        int number = PlayerPrefs.GetInt(System.DateTime.Today.ToString());

        if (number == 0)
        {
            Debug.Log("New DailyMission Appeard!");

            RandomDailyMission();
        }
        else
        {
            Debug.Log("Now DailyMission");

            LoadDailyMission();
        }
    }

    void RandomDailyMission()
    {
        playerDataBase.OnResetDailyMissionReport();

        PlayerPrefs.SetInt(System.DateTime.Today.ToString(), 1);

        UnDuplicateRandom(0, dailyMissionList.dailyMissions.Length);
    }

    void UnDuplicateRandom(int min, int max)
    {
        int currentNumber = Random.Range(min, max);
        missionIndexs.Clear();

        for (int i = 0; i < 3;)
        {
            if (missionIndexs.Contains(currentNumber))
            {
                currentNumber = Random.Range(min, max);
            }
            else
            {
                missionIndexs.Add(currentNumber);
                i++;
            }
        }

        for (int i = 0; i < missionIndexs.Count; i++)
        {
            dailyContents[i].Initialize(dailyMissionList.dailyMissions[missionIndexs[i]], i, this);
            playerDataBase.SetDailyMission(dailyMissionList.dailyMissions[missionIndexs[i]],i);


            DailyMission dailyMission = new DailyMission();

            dailyMission.gamePlayType = dailyMissionList.dailyMissions[missionIndexs[i]].gamePlayType;
            dailyMission.missionType = dailyMissionList.dailyMissions[missionIndexs[i]].missionType;
            dailyMission.goal = dailyMissionList.dailyMissions[missionIndexs[i]].goal;
            dailyMission.clear = false;
 
            dailyMissionData.Clear();
            dailyMissionData.Add("DailyMission_" + i, JsonUtility.ToJson(dailyMission));

            if(PlayfabManager.instance.isActive) PlayfabManager.instance.SetPlayerData(dailyMissionData);
        }

        UpdateDailyMission();
    }


    void LoadDailyMission() //데이터베이스에서 로드
    {
        for(int i = 0; i < dailyContents.Length; i ++)
        {
            dailyContents[i].Initialize(playerDataBase.GetDailyMission(i), i, this);
        }

        UpdateDailyMission();
    }

    [Button]
    public void UpdateDailyMission() //게임 끝날때 마다 체크
    {
        Debug.Log("일일 미션 상황 체크");

        for(int i = 0; i < dailyContents.Length; i ++)
        {
            dailyContents[i].UpdateState(playerDataBase.GetDailyMissionReportValue(i));
        }
    }

    public void Received(DailyMission dailyMission, int number)
    {
        if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdateAddCurrency(MoneyType.Coin, 100);

        NotionManager.instance.UseNotion(NotionType.ReceiveNotion);

        playerDataBase.SetDailyMissionClear(dailyMission);

        dailyMission.clear = true;

        dailyMissionData.Clear();
        dailyMissionData.Add("DailyMission_" + number, JsonUtility.ToJson(dailyMission));

        if (PlayfabManager.instance.isActive) PlayfabManager.instance.SetPlayerData(dailyMissionData);
    }
}
