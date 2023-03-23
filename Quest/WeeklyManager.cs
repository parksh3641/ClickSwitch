using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeeklyManager : MonoBehaviour
{
    [Title("Reward")]
    public Image[] rewardContentImgArray;
    public Sprite[] imgArray;

    public GameObject[] rewardProgressBarArray;

    public WeeklyContent weeklyContent;
    public RectTransform weeklyContentTransform;

    bool save = false;

    List<WeeklyContent> weeklyContentList = new List<WeeklyContent>();

    Dictionary<string, string> weeklyMissionData = new Dictionary<string, string>();

    WeeklyMissionList weeklyMissionList;
    PlayerDataBase playerDataBase;

    private void Awake()
    {
        if (weeklyMissionList == null) weeklyMissionList = Resources.Load("WeeklyMissionList") as WeeklyMissionList;
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;

        save = false;

        for (int i = 0; i < System.Enum.GetValues(typeof(WeeklyMissionType)).Length; i ++)
        {
            WeeklyContent content = Instantiate(weeklyContent);
            content.transform.parent = weeklyContentTransform;
            content.transform.position = Vector3.zero;
            content.transform.localScale = Vector3.one;
            content.gameObject.SetActive(false);

            weeklyContentList.Add(content);
        }

        for (int i = 0; i < rewardProgressBarArray.Length; i++)
        {
            rewardProgressBarArray[i].gameObject.SetActive(false);
        }

        weeklyContentTransform.anchoredPosition = new Vector2(0, -999);
    }

    public void Initialize()
    {
        for(int i = 0; i < weeklyContentList.Count; i ++)
        {
            weeklyContentList[i].gameObject.SetActive(true);
            weeklyContentList[i].Initialize(WeeklyMissionType.GetScore + i, this);
        }
    }

    public void InitializeMission()
    {
        playerDataBase.WeeklyMissionKey = 0;

        PlayfabManager.instance.UpdatePlayerStatisticsInsert("WeeklyMissionKey", 0);

        weeklyMissionList.Initialize();

        SetWeeklyMissionReport();

        weeklyMissionList.OnResetWeeklyMission();

        for(int i = 0; i < System.Enum.GetValues(typeof(WeeklyMissionType)).Length; i ++)
        {
            SetWeeklyMisson(WeeklyMissionType.GetScore + i);
        }

        Debug.Log("위클리 미션이 초기화되었습니다");
    }

    public void OpenWeeklyView()
    {
        CheckWeeklyMissionKey();

        if(save)
        {
            save = false;

            SetWeeklyMissionReport();
        }
    }


    public void CheckWeeklyMissionKey()
    {
        for (int i = 0; i < rewardProgressBarArray.Length; i++)
        {
            if (playerDataBase.WeeklyMissionKey >= i + 1)
            {
                rewardProgressBarArray[i].gameObject.SetActive(true);
            }
        }
    }

    public void SetWeeklyMissionReport()
    {
        weeklyMissionData.Clear();
        weeklyMissionData.Add("WeeklyMissionReport", JsonUtility.ToJson(weeklyMissionList.weeklyMissionReport));

        if (PlayfabManager.instance.isActive) PlayfabManager.instance.SetPlayerData(weeklyMissionData);

        Debug.Log("WeeklyMissionReport 서버 저장");
    }

    public void SetWeeklyMisson(WeeklyMissionType type)
    {
        weeklyMissionData.Clear();
        weeklyMissionData.Add("WeeklyMission_" + (int)type, JsonUtility.ToJson(weeklyMissionList.weeklyMissions[(int)type]));

        if (PlayfabManager.instance.isActive) PlayfabManager.instance.SetPlayerData(weeklyMissionData);

        Debug.Log("WeeklyMission_" + (int)type + "서버 저장");
    }

    public void RecevieButton(WeeklyMissionType type)
    {
        playerDataBase.WeeklyMissionKey += 1;
        PlayfabManager.instance.UpdatePlayerStatisticsInsert("WeeklyMissionKey", playerDataBase.WeeklyMissionKey);

        weeklyMissionList.weeklyMissions[(int)type].clear = true;
        SetWeeklyMisson(type);

        SetWeeklyMissionReport();

        CheckWeeklyMissionKey();

        NotionManager.instance.UseNotion(NotionType.ReceiveNotion);
    }

    public void UpdateWeeklyMissionReport(WeeklyMissionType type, int number)
    {
        save = true;

        switch (type)
        {
            case WeeklyMissionType.GetScore:
                weeklyMissionList.weeklyMissionReport.getScore += number;
                break;
            case WeeklyMissionType.GetCombo:
                weeklyMissionList.weeklyMissionReport.getCombo += number;
                break;
            case WeeklyMissionType.UseItem:
                weeklyMissionList.weeklyMissionReport.useItem += number;
                break;
            case WeeklyMissionType.GamePlay:
                weeklyMissionList.weeklyMissionReport.gamePlay += number;
                break;
            case WeeklyMissionType.DailyMissonClear:
                weeklyMissionList.weeklyMissionReport.dailyMissonClear += number;
                break;
            case WeeklyMissionType.ChallengeCoinRush:
                weeklyMissionList.weeklyMissionReport.challengeCoinRush += number;
                break;
        }

        for(int i = 0; i < weeklyContentList.Count; i ++)
        {
            weeklyContentList[i].UpdateValue();
        }
    }

    public void ReceiveButton(int number) //열쇠로 상자 받기
    {
        switch(number)
        {
            case 0:
                if(!weeklyMissionList.weeklyMissionReport.receive1)
                {
                    weeklyMissionList.weeklyMissionReport.receive1 = true;
                }

                break;
        }

        SetWeeklyMissionReport();
    }




    #region Developer
    [Button]
    public void UpdateWeeklyMissionReport1()
    {
        UpdateWeeklyMissionReport(WeeklyMissionType.GetScore, 100);
    }

    [Button]
    public void UpdateWeeklyMissionReport2()
    {
        UpdateWeeklyMissionReport(WeeklyMissionType.GetCombo, 10);
    }

    [Button]
    public void UpdateWeeklyMissionReport3()
    {
        UpdateWeeklyMissionReport(WeeklyMissionType.UseItem, 1);
    }

    [Button]
    public void UpdateWeeklyMissionReport4()
    {
        UpdateWeeklyMissionReport(WeeklyMissionType.GamePlay, 1);
    }

    [Button]
    public void UpdateWeeklyMissionReport5()
    {
        UpdateWeeklyMissionReport(WeeklyMissionType.DailyMissonClear, 1);
    }

    [Button]
    public void UpdateWeeklyMissionReport6()
    {
        UpdateWeeklyMissionReport(WeeklyMissionType.ChallengeCoinRush, 1);
    }
    #endregion
}
