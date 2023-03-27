using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeeklyManager : MonoBehaviour
{
    public GameObject receiveView;

    public GameObject alarm;

    [Title("Receive")]
    public ReceiveContent[] receiveContentArray;

    [Space]
    [Title("RewardBox")]
    public Image[] rewardContentImgArray;
    public Sprite[] imgArray;

    public ButtonScaleAnimation[] rewardContentAnimaton;

    public GameObject[] rewardProgressBarArray;

    public WeeklyContent weeklyContent;
    public RectTransform weeklyContentTransform;


    private int alarmIndex = 0;

    bool save = false;
    bool delay = false;

    List<WeeklyContent> weeklyContentList = new List<WeeklyContent>();

    Dictionary<string, string> weeklyMissionData = new Dictionary<string, string>();

    ShopDataBase shopDataBase;
    WeeklyMissionList weeklyMissionList;
    PlayerDataBase playerDataBase;

    private void Awake()
    {
        if (shopDataBase == null) shopDataBase = Resources.Load("ShopDataBase") as ShopDataBase;
        if (weeklyMissionList == null) weeklyMissionList = Resources.Load("WeeklyMissionList") as WeeklyMissionList;
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;

        alarm.SetActive(false);
        receiveView.SetActive(false);

        save = false;
        delay = false;

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
            rewardContentAnimaton[i].enabled = false;
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

        Initialize();

        Debug.Log("위클리 미션이 초기화되었습니다");
    }

    public void OnSetAlarm()
    {
        alarm.SetActive(true);

        alarmIndex++;
    }

    public void OnCheckAlarm()
    {
        alarmIndex--;

        if (alarmIndex == 0)
        {
            alarm.SetActive(false);
        }
    }

    public void OpenWeeklyView()
    {
        CheckWeeklyMissionKey();

        if(save)
        {
            save = false;
            delay = false;

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

        if(!weeklyMissionList.weeklyMissionReport.receive1)
        {
            if(playerDataBase.WeeklyMissionKey > 0)
            {
                rewardContentAnimaton[0].enabled = true;
                rewardContentImgArray[0].sprite = imgArray[0];
            }
        }
        else
        {
            rewardContentImgArray[0].sprite = imgArray[1];
        }


        if (!weeklyMissionList.weeklyMissionReport.receive2)
        {
            if (playerDataBase.WeeklyMissionKey > 1)
            {
                rewardContentAnimaton[1].enabled = true;
                rewardContentImgArray[1].sprite = imgArray[0];
            }
        }
        else
        {
            rewardContentImgArray[1].sprite = imgArray[1];
        }

        if (!weeklyMissionList.weeklyMissionReport.receive3)
        {
            if (playerDataBase.WeeklyMissionKey > 2)
            {
                rewardContentAnimaton[2].enabled = true;
                rewardContentImgArray[2].sprite = imgArray[0];
            }
        }
        else
        {
            rewardContentImgArray[2].sprite = imgArray[1];
        }

        if (!weeklyMissionList.weeklyMissionReport.receive4)
        {
            if (playerDataBase.WeeklyMissionKey > 3)
            {
                rewardContentAnimaton[3].enabled = true;
                rewardContentImgArray[3].sprite = imgArray[0];
            }
        }
        else
        {
            rewardContentImgArray[3].sprite = imgArray[1];
        }

        if (!weeklyMissionList.weeklyMissionReport.receive1)
        {
            if (playerDataBase.WeeklyMissionKey > 4)
            {
                rewardContentAnimaton[4].enabled = true;
                rewardContentImgArray[4].sprite = imgArray[0];
            }
        }
        else
        {
            rewardContentImgArray[4].sprite = imgArray[1];
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
            case WeeklyMissionType.DailyMissionClear:
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

    public void ReceiveWeeklyReward(int number)
    {
        switch(number)
        {
            case 0:
                if (weeklyMissionList.weeklyMissionReport.receive1 || playerDataBase.WeeklyMissionKey <= 0) return;
                break;
            case 1:
                if (weeklyMissionList.weeklyMissionReport.receive2 || playerDataBase.WeeklyMissionKey <= 1) return;
                break;
            case 2:
                if (weeklyMissionList.weeklyMissionReport.receive3 || playerDataBase.WeeklyMissionKey <= 2) return;
                break;
            case 3:
                if (weeklyMissionList.weeklyMissionReport.receive4 || playerDataBase.WeeklyMissionKey <= 3) return;
                break;
            case 4:
                if (weeklyMissionList.weeklyMissionReport.receive5 || playerDataBase.WeeklyMissionKey <= 4) return;
                break;
        }

        receiveView.SetActive(true);

        for(int i = 0; i < receiveContentArray.Length; i ++)
        {
            receiveContentArray[i].gameObject.SetActive(false);
        }

        int random1, random2, random3 = 0;

        switch(number)
        {
            case 0:
                random1 = GetRandom();

                receiveContentArray[0].Initialize(RewardType.Coin, 500);
                receiveContentArray[1].Initialize(RewardType.Clock + random1, 1);

                receiveContentArray[0].gameObject.SetActive(true);
                receiveContentArray[1].gameObject.SetActive(true);

                PlayfabManager.instance.UpdateAddCurrency(MoneyType.Coin, 500);
                PlayfabManager.instance.PurchaseItem(shopDataBase.itemList[random1], null, 1);

                weeklyMissionList.weeklyMissionReport.receive1 = true;
                break;
            case 1:
                random1 = GetRandom();
                random2 = GetRandom();

                receiveContentArray[0].Initialize(RewardType.Coin, 1000);
                receiveContentArray[1].Initialize(RewardType.Clock + random1, 1);
                receiveContentArray[2].Initialize(RewardType.Clock + random2, 1);

                receiveContentArray[0].gameObject.SetActive(true);
                receiveContentArray[1].gameObject.SetActive(true);
                receiveContentArray[2].gameObject.SetActive(true);

                PlayfabManager.instance.UpdateAddCurrency(MoneyType.Coin, 1000);
                PlayfabManager.instance.PurchaseItem(shopDataBase.itemList[random1], null, 1);
                PlayfabManager.instance.PurchaseItem(shopDataBase.itemList[random2], null, 1);

                weeklyMissionList.weeklyMissionReport.receive2 = true;
                break;
            case 2:
                receiveContentArray[0].Initialize(RewardType.Coin, 1500);
                receiveContentArray[1].Initialize(RewardType.Crystal, 80);

                receiveContentArray[0].gameObject.SetActive(true);
                receiveContentArray[1].gameObject.SetActive(true);

                PlayfabManager.instance.UpdateAddCurrency(MoneyType.Coin, 1500);
                PlayfabManager.instance.UpdateAddCurrency(MoneyType.Crystal, 80);

                weeklyMissionList.weeklyMissionReport.receive3 = true;
                break;
            case 3:
                random1 = GetRandom();
                random2 = GetRandom();
                random3 = GetRandom();

                receiveContentArray[0].Initialize(RewardType.Coin, 2000);
                receiveContentArray[1].Initialize(RewardType.Clock + random1, 1);
                receiveContentArray[2].Initialize(RewardType.Clock + random2, 1);
                receiveContentArray[3].Initialize(RewardType.Clock + random3, 1);

                receiveContentArray[0].gameObject.SetActive(true);
                receiveContentArray[1].gameObject.SetActive(true);
                receiveContentArray[2].gameObject.SetActive(true);
                receiveContentArray[3].gameObject.SetActive(true);

                PlayfabManager.instance.UpdateAddCurrency(MoneyType.Coin, 2000);
                PlayfabManager.instance.PurchaseItem(shopDataBase.itemList[random1], null, 1);
                PlayfabManager.instance.PurchaseItem(shopDataBase.itemList[random2], null, 1);
                PlayfabManager.instance.PurchaseItem(shopDataBase.itemList[random3], null, 1);

                weeklyMissionList.weeklyMissionReport.receive4 = true;
                break;
            case 4:
                receiveContentArray[0].Initialize(RewardType.Coin, 2500);
                receiveContentArray[1].Initialize(RewardType.Crystal, 150);
                receiveContentArray[2].Initialize(RewardType.IconBox, 1);

                receiveContentArray[0].gameObject.SetActive(true);
                receiveContentArray[1].gameObject.SetActive(true);
                receiveContentArray[2].gameObject.SetActive(true);

                PlayfabManager.instance.UpdateAddCurrency(MoneyType.Coin, 2500);
                PlayfabManager.instance.UpdateAddCurrency(MoneyType.Crystal, 150);

                if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdatePlayerStatisticsInsert("IconBox", 1);

                weeklyMissionList.weeklyMissionReport.receive5 = true;
                break;
        }

        OnCheckAlarm();

        SetWeeklyMissionReport();

        rewardContentAnimaton[number].StopAnim();
        rewardContentAnimaton[number].enabled = false;
        rewardContentImgArray[number].sprite = imgArray[1];

        delay = true;
        Invoke("CloseDelay", 1.5f);
    }

    void CloseDelay()
    {
        delay = false;
    }

    int GetRandom()
    {
        return Random.Range(0, 5);
    }

    public void CloseReceiveView()
    {
        if (!delay)
        {
            receiveView.SetActive(false);
        }
    }


    #region Developer
    [Button]
    public void UpdateWeeklyMissionReport1()
    {
        UpdateWeeklyMissionReport(WeeklyMissionType.GetScore, 10000);
    }

    [Button]
    public void UpdateWeeklyMissionReport2()
    {
        UpdateWeeklyMissionReport(WeeklyMissionType.GetCombo, 500);
    }

    [Button]
    public void UpdateWeeklyMissionReport3()
    {
        UpdateWeeklyMissionReport(WeeklyMissionType.UseItem, 10);
    }

    [Button]
    public void UpdateWeeklyMissionReport4()
    {
        UpdateWeeklyMissionReport(WeeklyMissionType.GamePlay, 10);
    }

    [Button]
    public void UpdateWeeklyMissionReport5()
    {
        UpdateWeeklyMissionReport(WeeklyMissionType.DailyMissionClear, 10);
    }

    [Button]
    public void UpdateWeeklyMissionReport6()
    {
        UpdateWeeklyMissionReport(WeeklyMissionType.ChallengeCoinRush, 5);
    }
    #endregion
}
