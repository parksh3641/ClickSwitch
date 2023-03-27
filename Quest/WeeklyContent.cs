using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeeklyContent : MonoBehaviour
{
    public LocalizationContent titleText;

    public Text goalText;
    public Image fillAmount;

    public GameObject lockReceiveObj;
    public GameObject clearObj;

    public int value = 0;
    public int goal = 0;

    WeeklyManager weeklyManager;

    WeeklyMission weeklyMission;
    WeeklyMissionList weeklyMissionList;

    private void Awake()
    {
        if (weeklyMissionList == null) weeklyMissionList = Resources.Load("WeeklyMissionList") as WeeklyMissionList;
    }

    public void Initialize(WeeklyMissionType type, WeeklyManager manager)
    {
        weeklyMission = weeklyMissionList.GetWeeklyMission(type);

        weeklyManager = manager;

        titleText.localizationName = type.ToString();
        titleText.ReLoad();

        value = weeklyMissionList.GetWeeklyData(type);
        goal = weeklyMission.goal;

        lockReceiveObj.SetActive(true);

        if (value >= goal)
        {
            value = goal;

            lockReceiveObj.SetActive(false);

            if(!weeklyMission.clear)
            {
                manager.OnSetAlarm();
            }
        }

        fillAmount.fillAmount = value / (goal * 1.0f);
        goalText.text = value + "/" + goal;

        clearObj.SetActive(false);

        if (weeklyMission.clear)
        {
            clearObj.SetActive(true);
        }
    }

    public void UpdateValue()
    {
        Initialize(weeklyMission.weeklyMissonType, weeklyManager);
    }

    public void ReceiveButton()
    {
        weeklyManager.RecevieButton(weeklyMission.weeklyMissonType);

        clearObj.SetActive(true);
    }
}
