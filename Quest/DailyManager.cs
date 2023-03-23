using Firebase.Analytics;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DailyManager : MonoBehaviour
{
    public GameObject dailyView;
    public GameObject showVCView;

    public GameObject alarm;

    [Space]
    [Title("TopMenu")]
    public Image[] topMenuImgArray;
    public Sprite[] topMenuSpriteArray;

    [Title("ScrollView")]
    public GameObject[] scrollVeiwArray;

    public Text dailyMissionTimerText;
    public Text weeklyMissionTimerText;

    string localization_NextQuest = "";
    string localization_GetClearReward = "";
    string localization_Days = "";
    string localization_Hours = "";
    string localization_Minutes = "";

    public DailyContent[] dailyContents;

    public Text clearText;
    public GameObject[] lockReceiveObj;

    private int alarmIndex = 0;
    private int topNumber = 0;
    private bool open = false;

    WaitForSeconds waitForSeconds = new WaitForSeconds(1);

    List<int> missionIndexs = new List<int>();
    Dictionary<string, string> dailyMissionData = new Dictionary<string, string>();


    public WeeklyManager weeklyManager;
    DailyMissionList dailyMissionList;
    PlayerDataBase playerDataBase;


    private void Awake()
    {
        if (dailyMissionList == null) dailyMissionList = Resources.Load("DailyMissionList") as DailyMissionList;
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;

        dailyView.SetActive(false);
        showVCView.SetActive(false);
        alarm.SetActive(false);
        lockReceiveObj[0].SetActive(true);
        lockReceiveObj[1].SetActive(true);
    }

    private void Start()
    {
        StartCoroutine(DailyMissionTimer());
        StartCoroutine(WeeklyMissionTimer());

        topNumber = -1;
        ChangeTopMenu(0);
    }

    private void OnEnable()
    {
        GameManager.eGameStart += GameStart;
        GameManager.eGameEnd += UpdateDailyMission;
    }
    private void OnDisable()
    {
        GameManager.eGameStart -= GameStart;
        GameManager.eGameEnd -= UpdateDailyMission;
    }

    public void Initialize()
    {
        dailyView.SetActive(true);
        dailyView.GetComponent<RectTransform>().anchoredPosition = new Vector2(4000, 0);
    }


    void GameStart()
    {
        dailyView.SetActive(false);
    }

    public void OpenDailyView()
    {
        if (!open)
        {
            LoadDailyMission();

            open = true;
            dailyView.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);

            showVCView.SetActive(true);

            dailyMissionTimerText.text = "";
            weeklyMissionTimerText.text = "";

            localization_NextQuest = LocalizationManager.instance.GetString("NextQuest");
            localization_GetClearReward = LocalizationManager.instance.GetString("GetClearReward");
            localization_Days = LocalizationManager.instance.GetString("Days");
            localization_Hours = LocalizationManager.instance.GetString("Hours");
            localization_Minutes = LocalizationManager.instance.GetString("Minutes");

            clearText.text = localization_GetClearReward + " " + playerDataBase.DailyMissionCount + " / 3";
        }
        else
        {
            open = false;
            dailyView.GetComponent<RectTransform>().anchoredPosition = new Vector2(4000, 0);

            showVCView.SetActive(false);

            if (playerDataBase.DailyMissionCount == 0)
            {
                OnCheckAlarm();
            }
        }
    }

    [Button]
    public void OnReset()
    {
        PlayerPrefs.SetInt(System.DateTime.Today.ToString(), 0);
    }

    public void InitializeMission()
    {
        RandomDailyMission();

        OnSetAlarm();

        Debug.Log("일일 미션이 갱신되었습니다");
    }

    void RandomDailyMission()
    {
        playerDataBase.OnResetDailyMissionReport();

        PlayerPrefs.SetInt(System.DateTime.Today.ToString(), 1);

        if (PlayfabManager.instance.isActive)
        {
            PlayfabManager.instance.UpdatePlayerStatisticsInsert("DailyMissionCount", 0);
            PlayfabManager.instance.UpdatePlayerStatisticsInsert("DailyMissionClear", 0);
        }

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

            if (dailyMission.goal == 0) dailyMission.goal = 1;

            dailyMissionData.Clear();
            dailyMissionData.Add("DailyMission_" + i, JsonUtility.ToJson(dailyMission));

            if(PlayfabManager.instance.isActive) PlayfabManager.instance.SetPlayerData(dailyMissionData);
        }

        weeklyManager.UpdateWeeklyMissionReport(WeeklyMissionType.DailyMissonClear, 1);

        UpdateDailyMission();
    }


    public void LoadDailyMission()
    {
        for(int i = 0; i < dailyContents.Length; i ++)
        {
            dailyContents[i].Initialize(playerDataBase.GetDailyMission(i), i, this);
        }

        clearText.text = localization_GetClearReward + " " + playerDataBase.DailyMissionCount + " / 3";

        if (playerDataBase.DailyMissionCount >= 3 && !playerDataBase.DailyMissionClear)
        {
            lockReceiveObj[0].SetActive(false);
            lockReceiveObj[1].SetActive(false);
            OnSetAlarm();
        }

        UpdateDailyMission();
    }

    [Button]
    public void UpdateDailyMission()
    {
        dailyView.SetActive(true);

        for(int i = 0; i < dailyContents.Length; i ++)
        {
            dailyContents[i].UpdateState(playerDataBase.GetDailyMissionReportValue(i));
        }

        Debug.Log("일일 미션 업데이트 완료");
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

    public void Received(DailyMission dailyMission, int number)
    {
        NotionManager.instance.UseNotion(NotionType.ReceiveNotion);

        if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdateAddCurrency(MoneyType.Coin, 500);

        playerDataBase.SetDailyMissionClear(dailyMission);

        dailyMission.clear = true;

        dailyMissionData.Clear();
        dailyMissionData.Add("DailyMission_" + number, JsonUtility.ToJson(dailyMission));

        playerDataBase.DailyMissionCount += 1;

        if (PlayfabManager.instance.isActive)
        {
            PlayfabManager.instance.SetPlayerData(dailyMissionData);
            PlayfabManager.instance.UpdatePlayerStatisticsInsert("DailyMissionCount", playerDataBase.DailyMissionCount);
        }

        clearText.text = localization_GetClearReward + " " + playerDataBase.DailyMissionCount + " / 3";

        if (playerDataBase.DailyMissionCount >= 3)
        {
            lockReceiveObj[0].SetActive(false);
            lockReceiveObj[1].SetActive(false);

            OnSetAlarm();
        }

        OnCheckAlarm();
    }

    public void ReceiveCrystal()
    {
        if (!lockReceiveObj[0].activeInHierarchy && !playerDataBase.DailyMissionClear)
        {
            NotionManager.instance.UseNotion(NotionType.ReceiveNotion);

            if (PlayfabManager.instance.isActive)
            {
                PlayfabManager.instance.UpdateAddCurrency(MoneyType.Coin, 1500);
                PlayfabManager.instance.UpdateAddCurrency(MoneyType.Crystal, 30);
                //PlayfabManager.instance.UpdatePlayerStatisticsInsert("IconBox", 1);
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("DailyMissionClear", 1);
            }

            playerDataBase.DailyMissionClear = true;

            lockReceiveObj[0].SetActive(true);
            lockReceiveObj[1].SetActive(true);

            alarmIndex = 0;
            alarm.SetActive(false);
        }

        FirebaseAnalytics.LogEvent("DailyMission Reward");
    }

    public void SuccessWatchAd()
    {
        if (!lockReceiveObj[1].activeInHierarchy && !playerDataBase.DailyMissionClear)
        {
            NotionManager.instance.UseNotion(NotionType.ReceiveNotion);

            if (PlayfabManager.instance.isActive)
            {
                PlayfabManager.instance.UpdateAddCurrency(MoneyType.Coin, 3000);
                PlayfabManager.instance.UpdateAddCurrency(MoneyType.Crystal, 60);
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("DailyMissionClear", 1);
            }

            playerDataBase.DailyMissionClear = true;

            lockReceiveObj[0].SetActive(true);
            lockReceiveObj[1].SetActive(true);

            alarmIndex = 0;
            alarm.SetActive(false);
        }

        FirebaseAnalytics.LogEvent("DailyMission Reward X3");
    }

    IEnumerator DailyMissionTimer()
    {
        if (dailyMissionTimerText.gameObject.activeInHierarchy)
        {
            System.DateTime f = System.DateTime.Now;
            System.DateTime g = System.DateTime.Today.AddDays(1);
            System.TimeSpan h = g - f;

            dailyMissionTimerText.text = localization_NextQuest + " : " + h.Hours.ToString("D2") + localization_Hours + " " + h.Minutes.ToString("D2") + localization_Minutes;
        }

        yield return waitForSeconds;
        StartCoroutine(DailyMissionTimer());
    }

    IEnumerator WeeklyMissionTimer()
    {
        if (weeklyMissionTimerText.gameObject.activeInHierarchy)
        {
            System.DateTime f = System.DateTime.Now;
            System.DateTime g = DateTime.Today.AddDays(((int)DayOfWeek.Monday - (int)DateTime.Today.DayOfWeek + 7) % 7);
            System.TimeSpan h = g - f;

            if (h.Days > 0)
            {

                weeklyMissionTimerText.text = localization_NextQuest + " : " + h.Days.ToString("D2") + localization_Days + " " + h.Hours.ToString("D2") + localization_Hours;
            }
            else
            {
                weeklyMissionTimerText.text = localization_NextQuest + " : " + h.Hours.ToString("D2") + localization_Hours + " " + h.Minutes.ToString("D2") + localization_Minutes;
            }
        }

        yield return waitForSeconds;
        StartCoroutine(WeeklyMissionTimer());
    }

    public void ChangeTopMenu(int number)
    {
        if (topNumber != number)
        {
            topNumber = number;

            for (int i = 0; i < topMenuImgArray.Length; i++)
            {
                topMenuImgArray[i].sprite = topMenuSpriteArray[0];
            }
            topMenuImgArray[number].sprite = topMenuSpriteArray[1];

            for (int i = 0; i < scrollVeiwArray.Length; i++)
            {
                scrollVeiwArray[i].SetActive(false);
            }

            if(number == 0)
            {
                dailyMissionTimerText.gameObject.SetActive(true);
                weeklyMissionTimerText.gameObject.SetActive(false);
            }
            else if(number == 1)
            {
                dailyMissionTimerText.gameObject.SetActive(false);
                weeklyMissionTimerText.gameObject.SetActive(true);

                weeklyManager.OpenWeeklyView();
            }
            else
            {
                dailyMissionTimerText.gameObject.SetActive(false);
                weeklyMissionTimerText.gameObject.SetActive(false);
            }

            scrollVeiwArray[number].SetActive(true);
        }
    }
}
