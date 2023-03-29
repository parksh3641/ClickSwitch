using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttendanceManager : MonoBehaviour
{
    public GameObject attendanceView;
    public GameObject showVCView;

    public GameObject alarm;

    public Text timerText;

    public AttendanceContent[] attendanceContentArray;

    [Title("7 Day")]
    public LocalizationContent titleText;
    public ReceiveContent[] receiveContentArray;
    public GameObject lockObj;
    public GameObject clearObj;

    string localization_NextQuest = "";
    string localization_Hours = "";
    string localization_Minutes = "";

    WaitForSeconds waitForSeconds = new WaitForSeconds(1);

    public ResetManager resetManager;
    PlayerDataBase playerDataBase;

    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;

        attendanceView.SetActive(false);
        showVCView.SetActive(false);
        lockObj.SetActive(true);
        clearObj.SetActive(false);
    }

    private void Start()
    {
        StartCoroutine(TimerCoroution());
    }

    [Button]
    public void NextDay()
    {
        playerDataBase.AttendanceCheck = false;

        CheckAttendance();
    }

    public void OpenAttendanceView()
    {
        if(!attendanceView.activeInHierarchy)
        {
            if (playerDataBase.AttendanceDay == DateTime.Today.ToString("yyyyMMdd"))
            {
                resetManager.Initialize();
            }

            attendanceView.SetActive(true);
            showVCView.SetActive(true);

            timerText.text = "";

            titleText.ReLoad();

            Initialize();

            CheckAttendance();
        }
        else
        {
            attendanceView.SetActive(false);
            showVCView.SetActive(false);
        }
    }

    public void Initialize()
    {
        attendanceContentArray[0].receiveContent.Initialize(RewardType.Coin, 1000);
        attendanceContentArray[1].receiveContent.Initialize(RewardType.Crystal, 30);
        attendanceContentArray[2].receiveContent.Initialize(RewardType.Coin, 1000);
        attendanceContentArray[3].receiveContent.Initialize(RewardType.Crystal, 30);
        attendanceContentArray[4].receiveContent.Initialize(RewardType.Coin, 1000);
        attendanceContentArray[5].receiveContent.Initialize(RewardType.Crystal, 30);

        receiveContentArray[0].Initialize(RewardType.Coin, 3000);
        receiveContentArray[1].Initialize(RewardType.Crystal, 100);
        receiveContentArray[2].Initialize(RewardType.IconBox, 1);
    }

    public void CheckAttendance()
    {
        localization_NextQuest = LocalizationManager.instance.GetString("NextRewardDay");
        localization_Hours = LocalizationManager.instance.GetString("Hours");
        localization_Minutes = LocalizationManager.instance.GetString("Minutes");

        for (int i = 0; i < attendanceContentArray.Length; i ++)
        {
            attendanceContentArray[i].Initialize(playerDataBase.AttendanceCount, playerDataBase.AttendanceCheck, this);
        }

        lockObj.SetActive(true);

        if (playerDataBase.AttendanceCount == 6 && !playerDataBase.AttendanceCheck)
        {
            lockObj.SetActive(false);
        }

        clearObj.SetActive(false);

        if (playerDataBase.AttendanceCount >= 7 && playerDataBase.AttendanceCheck)
        {
            lockObj.SetActive(false);
            clearObj.SetActive(true);
        }
    }

    public void ReceiveButton7Day()
    {
        ReceiveButton(6, SuccessReceive);
    }

    public void SuccessReceive()
    {
        clearObj.SetActive(true);
    }

    public void ReceiveButton(int number, Action action)
    {
        if (playerDataBase.AttendanceCheck) return;

        switch (number)
        {
            case 0:
                PlayfabManager.instance.UpdateAddCurrency(MoneyType.Coin, 1000);
                break;
            case 1:
                PlayfabManager.instance.UpdateAddCurrency(MoneyType.Crystal, 30);
                break;
            case 2:
                PlayfabManager.instance.UpdateAddCurrency(MoneyType.Coin, 1000);
                break;
            case 3:
                PlayfabManager.instance.UpdateAddCurrency(MoneyType.Crystal, 30);
                break;
            case 4:
                PlayfabManager.instance.UpdateAddCurrency(MoneyType.Coin, 1000);
                break;
            case 5:
                PlayfabManager.instance.UpdateAddCurrency(MoneyType.Crystal, 30);
                break;
            case 6:
                PlayfabManager.instance.UpdateAddCurrency(MoneyType.Coin, 3000);
                PlayfabManager.instance.UpdateAddCurrency(MoneyType.Crystal, 100);
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("IconBox", 1);

                clearObj.SetActive(true);

                break;
        }

        playerDataBase.AttendanceCount += 1;
        playerDataBase.AttendanceCheck = true;

        PlayfabManager.instance.UpdatePlayerStatisticsInsert("AttendanceCount", playerDataBase.AttendanceCount);
        PlayfabManager.instance.UpdatePlayerStatisticsInsert("AttendanceCheck", 1);

        action.Invoke();

        CheckAttendance();

        OnCheckAlarm();

        NotionManager.instance.UseNotion(NotionType.ReceiveNotion);
    }

    IEnumerator TimerCoroution()
    {
        System.DateTime f = System.DateTime.Now;
        System.DateTime g = System.DateTime.Today.AddDays(1);
        System.TimeSpan h = g - f;

        timerText.text = localization_NextQuest + " : " + h.Hours.ToString("D2") + localization_Hours + " " + h.Minutes.ToString("D2") + localization_Minutes;

        yield return waitForSeconds;
        StartCoroutine(TimerCoroution());
    }

    public void OnSetAlarm()
    {
        alarm.SetActive(true);
    }

    public void OnCheckAlarm()
    {
        alarm.SetActive(false);
    }
}
