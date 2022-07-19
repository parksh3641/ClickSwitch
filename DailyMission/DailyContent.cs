using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DailyContent : MonoBehaviour
{
    DailyMission dailyMission;
    DailyManager dailyManager;

    public Image icon;
    public LocalizationContent titleText;
    public Text goalText;

    private int index = 0;
    private int goal = 0;
    private bool alarm = false;
    private bool clear = false;

    public GameObject lockReceiveObj;
    public GameObject clearObj;

    ImageDataBase imageDataBase;
    Sprite[] iconArray;


    private void Awake()
    {
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;

        iconArray = imageDataBase.GetIconArray();

        lockReceiveObj.SetActive(true);
        clearObj.SetActive(false);
    }

    public void Initialize(DailyMission mission, int number, DailyManager manager)
    {
        dailyMission = mission;
        index = number;
        dailyManager = manager;

        icon.sprite = iconArray[(int)mission.gamePlayType];
        titleText.name = mission.missionType.ToString();

        goal = mission.goal;

        clear = mission.clear;

        if (clear)
        {
            lockReceiveObj.SetActive(true);
            clearObj.SetActive(true);
        }
    }

    public void UpdateState(int number)
    {
        goalText.text = number + " / " + goal.ToString();

        if(number >= goal)
        {
            lockReceiveObj.SetActive(false);

            if(!alarm)
            {
                alarm = true;

                if(!clear)
                {
                    dailyManager.OnSetAlarm();
                }
            }
        }
    }


    public void OnReceive()
    {
        if(!lockReceiveObj.activeInHierarchy)
        {
            dailyManager.Received(dailyMission, index);

            lockReceiveObj.SetActive(true);
            clearObj.SetActive(true);
        }
    }
}
