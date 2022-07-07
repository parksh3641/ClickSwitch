using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DailyContent : MonoBehaviour
{
    DailyMission dailyMission;

    public Image icon;
    public Text goalText;

    public GameObject nowReceiveObj;
    public GameObject receiveButton;

    public MissionRewardContent[] missionRewardContents;

    DailyManager dailyManager;


    ImageDataBase imageDataBase;
    Sprite[] vcArray;
    Sprite[] iconArray;


    private void Awake()
    {
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;

        vcArray = imageDataBase.GetVCArray();

        nowReceiveObj.SetActive(false);
        receiveButton.SetActive(false);
    }

    public void Initialize(DailyMission mission, DailyManager manager)
    {
        dailyMission = mission;
        dailyManager = manager;

        icon.sprite = iconArray[(int)mission.gamePlayType];
        goalText.text = mission.goal.ToString();

        for (int i = 0; i < missionRewardContents.Length; i ++)
        {
            missionRewardContents[i].obj.SetActive(false);
        }

        for(int i =0; i < mission.rewardCount; i ++)
        {
            missionRewardContents[i].icon.sprite = vcArray[(int)mission.missionRewards[i].rewardType];
            missionRewardContents[i].rewardNumberText.text = mission.missionRewards[i].rewardNumber.ToString();
            missionRewardContents[i].obj.SetActive(true);
        }
    }

    public void SuccessDaily()
    {
        receiveButton.SetActive(true);
    }

    public void NowReceived()
    {
        nowReceiveObj.SetActive(true);
        receiveButton.SetActive(false);
    }

    public void OnClick()
    {
        dailyManager.Receive(dailyMission);
    }
}
