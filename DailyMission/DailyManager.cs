using Firebase.Analytics;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyManager : MonoBehaviour
{
    public GameObject dailyView;
    public GameObject showVCView;

    public GameObject alarm;

    public Text timerText;

    string localization = "";
    string localization2 = "";

    public DailyContent[] dailyContents;

    public Text clearText;
    public GameObject lockReceiveObj;

    private int alarmIndex = 0;
    private bool open = false;

    List<int> missionIndexs = new List<int>();
    Dictionary<string, string> dailyMissionData = new Dictionary<string, string>();


    DailyMissionList dailyMissionList;
    PlayerDataBase playerDataBase;


    private void Awake()
    {
        if (dailyMissionList == null) dailyMissionList = Resources.Load("DailyMissionList") as DailyMissionList;
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;

        dailyView.SetActive(false);
        showVCView.SetActive(false);
        alarm.SetActive(false);
        lockReceiveObj.SetActive(true);
    }

    public void Initialize()
    {
        dailyView.SetActive(true);
        dailyView.GetComponent<RectTransform>().anchoredPosition = new Vector2(4000, 0);

        CheckDailyMission();
    }

    private void Start()
    {
        StartCoroutine(DailyMissionTimer());
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

    void GameStart()
    {
        dailyView.SetActive(false);
    }

    public void OpenDaily()
    {
        if (!open)
        {
            open = true;
            dailyView.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);

            showVCView.SetActive(true);

            timerText.text = "";

            localization = LocalizationManager.instance.GetString("NextQuest");
            localization2 = LocalizationManager.instance.GetString("GetClearReward");

            clearText.text = localization2 + " " + playerDataBase.DailyMissionCount + " / 3";
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

    public void CheckDailyMission()
    {
        int number = PlayerPrefs.GetInt(System.DateTime.Today.ToString());

        if (number == 0)
        {
            Debug.Log("New DailyMission Appeard!");

            RandomDailyMission();

            OnSetAlarm();
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

        UpdateDailyMission();
    }


    void LoadDailyMission()
    {
        for(int i = 0; i < dailyContents.Length; i ++)
        {
            dailyContents[i].Initialize(playerDataBase.GetDailyMission(i), i, this);
        }

        clearText.text = localization2 + " " + playerDataBase.DailyMissionCount + " / 3";

        if (playerDataBase.DailyMissionCount >= 3 && !playerDataBase.DailyMissionClear)
        {
            lockReceiveObj.SetActive(false);
            OnSetAlarm();
        }

        UpdateDailyMission();
    }

    [Button]
    public void UpdateDailyMission()
    {
        dailyView.SetActive(true);

        Debug.Log("Update DailyMission");

        for(int i = 0; i < dailyContents.Length; i ++)
        {
            dailyContents[i].UpdateState(playerDataBase.GetDailyMissionReportValue(i));
        }
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

        if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdateAddCurrency(MoneyType.Coin, 300);

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

        clearText.text = localization2 + " " + playerDataBase.DailyMissionCount + " / 3";

        if (playerDataBase.DailyMissionCount >= 3)
        {
            lockReceiveObj.SetActive(false);

            OnSetAlarm();
        }

        OnCheckAlarm();
    }

    public void ReceiveCrystal()
    {
        if (!lockReceiveObj.activeInHierarchy && !playerDataBase.DailyMissionClear)
        {
            NotionManager.instance.UseNotion(NotionType.ReceiveNotion);

            if (PlayfabManager.instance.isActive)
            {
                PlayfabManager.instance.UpdateAddCurrency(MoneyType.Coin, 500);
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("IconBox", 1);
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("DailyMissionClear", 1);
            }

            playerDataBase.DailyMissionClear = true;

            lockReceiveObj.SetActive(true);

            alarmIndex = 0;
            alarm.SetActive(false);
        }

        FirebaseAnalytics.LogEvent("DailyMission");
    }

    IEnumerator DailyMissionTimer()
    {
        System.DateTime f = System.DateTime.Now;
        System.DateTime g = System.DateTime.Today.AddDays(1);
        System.TimeSpan h = g - f;

        int i = h.Hours;
        int j = h.Minutes;
        int k = h.Seconds;
        int total = i + j + k;
        string l, m, n;

        if (i > 9)
        {
            l = i.ToString();
        }
        else
        {
            l = "0" + i.ToString();
        }

        if (j > 9)
        {
            m = j.ToString();
        }
        else
        {
            m = "0" + j.ToString();
        }

        if (k > 9)
        {
            n = k.ToString();
        }
        else
        {
            n = "0" + k.ToString();
        }

        timerText.text = localization + " : " + l + ":" + m + ":" + n;

        yield return new WaitForSeconds(1);
        StartCoroutine(DailyMissionTimer());
    }
}
