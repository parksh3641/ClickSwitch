using PlayFab.ClientModels;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class WorldScoreInformation
{
    public int season = 0;
    public bool checkOne = false;
    public bool checkTwo = false;
    public bool checkThree = false;
    public bool checkFour = false;
    public bool checkFive = false;
    public bool checkSix = false;

    public bool GetCheck(int number)
    {
        bool check = false;
        switch(number)
        {
            case 0:
                check = checkOne;
                break;
            case 1:
                check = checkTwo;
                break;
            case 2:
                check = checkThree;
                break;
            case 3:
                check = checkFour;
                break;
            case 4:
                check = checkFive;
                break;
            case 5:
                check = checkSix;
                break;
        }
        return check;
    }
}

public class EventManager : MonoBehaviour
{
    public GameObject eventView;
    public GameObject showVCView;

    [Title("EnterView")]
    public GameObject welcomeEnterView;
    public GameObject worldScoreEnterView;

    [Title("Welcome")]
    public GameObject welcomeView;
    public RectTransform welcomGrid;
    public GameObject welcomeAlarm;
    public GameObject welcomeAlarm2;
    public WelcomeContent[] welcomeContentArray;

    [Space]
    [Title("WorldScore")]
    public GameObject worldScoreView;
    public GameObject worldScoreAlarm;

    public WorldScoreContent[] worldScoreContentArray;

    public List<WorldScoreInformation> worldScoreInformationList = new List<WorldScoreInformation>();

    public LocalizationContent worldScoreSeasonText;

    public Image worldScoreFillamount;
    public Text worldScoreTitleText;
    public Text mySupportScoreText;

    DateTime system;

    private string date = "";
    private int season = 0;

    private int tempScore = 0;
    private int totalScore = 0;
    private int nowScore = 0;
    private int needScore = 0;

    WorldScoreInformation worldScoreInfo = new WorldScoreInformation();

    Dictionary<string, string> titleData = new Dictionary<string, string>();

    PlayerDataBase playerDataBase;

    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;

        eventView.SetActive(false);
        showVCView.SetActive(false);

        welcomeView.SetActive(false);
        worldScoreView.SetActive(false);

        welcomeAlarm.SetActive(false);
        welcomeAlarm2.SetActive(false);
        worldScoreAlarm.SetActive(false);

        welcomGrid.anchoredPosition = new Vector2(0, -999);
    }

    public void SetWorldScoreInformation(WorldScoreInformation info)
    {
        worldScoreInformationList.Add(info);
    }

    public void Initialize()
    {
        if(playerDataBase.WelcomeCount >= 7)
        {
            welcomeEnterView.SetActive(false);
        }
        else
        {
            welcomeEnterView.SetActive(true);

            if(!playerDataBase.WelcomeCheck)
            {
                OnSetWelcomeAlarm();
            }

            PlayfabManager.instance.GetTitleInternalData("WorldScoreDate", SetDate);
            PlayfabManager.instance.GetTitleInternalData("WorldScoreNeed", SetNeed);
            PlayfabManager.instance.GetTitleInternalData("Season", SetSeason);
        }
    }

    public void SetDate(string date)
    {
        system = DateTime.ParseExact(date, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);

        if (DateTime.Compare(DateTime.Today, system) == -1)
        {
            worldScoreEnterView.SetActive(true);
        }
        else
        {
            worldScoreEnterView.SetActive(false);
        }
    }

    public void SetNeed(string need)
    {
        needScore = int.Parse(need);
    }

    public void SetSeason(string season)
    {
        this.season = int.Parse(season);

        playerDataBase.Season = this.season;

        worldScoreSeasonText.localizationName = "Season" + this.season;
    }

    public void OpenEventView()
    {
        if(!eventView.activeInHierarchy)
        {
            eventView.SetActive(true);
            showVCView.SetActive(true);

            CheckEvent();
        }
        else
        {
            eventView.SetActive(false);
            showVCView.SetActive(false);
        }
    }

    public void CheckEvent()
    {

    }


    #region Welcome
    public void InitializeWelcome()
    {
        welcomeContentArray[0].receiveContent.Initialize(RewardType.Crystal, 100);
        welcomeContentArray[1].receiveContent.Initialize(RewardType.Crystal, 150);
        welcomeContentArray[2].receiveContent.Initialize(RewardType.Crystal, 200);
        welcomeContentArray[3].receiveContent.Initialize(RewardType.Crystal, 250);
        welcomeContentArray[4].receiveContent.Initialize(RewardType.Crystal, 300);
        welcomeContentArray[5].receiveContent.Initialize(RewardType.Crystal, 400);
        welcomeContentArray[6].receiveContent.Initialize(RewardType.Crystal, 500);
    }

    public void OpenWelcomView()
    {
        if (!welcomeView.activeInHierarchy)
        {
            welcomeView.SetActive(true);

            InitializeWelcome();

            CheckWelcome();
        }
        else
        {
            welcomeView.SetActive(false);
        }
    }

    public void CheckWelcome()
    {
        welcomeContentArray[0].Initialize(playerDataBase.WelcomeCount, playerDataBase.WelcomeCheck, playerDataBase.BestSpeedTouchScore, this);
        welcomeContentArray[1].Initialize(playerDataBase.WelcomeCount, playerDataBase.WelcomeCheck, playerDataBase.BestMoleCatchScore, this);
        welcomeContentArray[2].Initialize(playerDataBase.WelcomeCount, playerDataBase.WelcomeCheck, playerDataBase.BestFilpCardScore, this);
        welcomeContentArray[3].Initialize(playerDataBase.WelcomeCount, playerDataBase.WelcomeCheck, playerDataBase.BestButtonActionScore, this);
        welcomeContentArray[4].Initialize(playerDataBase.WelcomeCount, playerDataBase.WelcomeCheck, playerDataBase.BestTimingActionScore, this);
        welcomeContentArray[5].Initialize(playerDataBase.WelcomeCount, playerDataBase.WelcomeCheck, playerDataBase.BestDragActionScore, this);
        welcomeContentArray[6].Initialize(playerDataBase.WelcomeCount, playerDataBase.WelcomeCheck, playerDataBase.BestLeftRightScore, this);
    }

    public void WelcomeReceiveButton(int number, System.Action action)
    {
        if (playerDataBase.WelcomeCheck) return;

        switch (number)
        {
            case 0:
                PlayfabManager.instance.UpdateAddCurrency(MoneyType.Crystal, 100);
                break;
            case 1:
                PlayfabManager.instance.UpdateAddCurrency(MoneyType.Crystal, 150);
                break;
            case 2:
                PlayfabManager.instance.UpdateAddCurrency(MoneyType.Crystal, 200);
                break;
            case 3:
                PlayfabManager.instance.UpdateAddCurrency(MoneyType.Crystal, 250);
                break;
            case 4:
                PlayfabManager.instance.UpdateAddCurrency(MoneyType.Crystal, 300);
                break;
            case 5:
                PlayfabManager.instance.UpdateAddCurrency(MoneyType.Crystal, 350);
                break;
            case 6:
                PlayfabManager.instance.UpdateAddCurrency(MoneyType.Crystal, 400);
                break;
        }

        playerDataBase.WelcomeCount += 1;
        playerDataBase.WelcomeCheck = true;

        PlayfabManager.instance.UpdatePlayerStatisticsInsert("WelcomeCount", playerDataBase.WelcomeCount);
        PlayfabManager.instance.UpdatePlayerStatisticsInsert("WelcomeCheck", 1);

        action.Invoke();

        CheckWelcome();

        OnCheckWelcomeAlarm();

        NotionManager.instance.UseNotion(NotionType.ReceiveNotion);
    }

    public void OnSetWelcomeAlarm()
    {
        welcomeAlarm.SetActive(true);
        welcomeAlarm2.SetActive(true);
    }

    public void OnCheckWelcomeAlarm()
    {
        welcomeAlarm.SetActive(false);
        welcomeAlarm2.SetActive(false);
    }

    #endregion


    #region WorldScore

    public void OpenWorldScoreView()
    {
        if (!worldScoreView.activeInHierarchy)
        {
            worldScoreView.SetActive(true);

            worldScoreTitleText.text = "";
            worldScoreFillamount.fillAmount = 0;

            CheckWorldScore();
        }
        else
        {
            worldScoreView.SetActive(false);
        }
    }

    public void CheckWorldScore()
    {
        worldScoreContentArray[0].receiveContent.Initialize(RewardType.Coin, 3000);
        worldScoreContentArray[1].receiveContent.Initialize(RewardType.Crystal, 80);
        worldScoreContentArray[2].receiveContent.Initialize(RewardType.Coin, 6000);
        worldScoreContentArray[3].receiveContent.Initialize(RewardType.Crystal, 150);
        worldScoreContentArray[4].receiveContent.Initialize(RewardType.Coin, 10000);
        worldScoreContentArray[5].receiveContent.Initialize(RewardType.IconBox, 5);

        if (PlayfabManager.instance.isActive) PlayfabManager.instance.GetLeaderboarder("WorldScore" + season.ToString(), SetRanking);

        for(int i = 0; i< worldScoreContentArray.Length; i ++)
        {
            if(worldScoreInformationList.Count == season - 1)
            {
                worldScoreContentArray[i].Initialize(false, this);
            }
            else
            {
                worldScoreContentArray[i].Initialize(worldScoreInformationList[GetWorldScoreInformation(season)].GetCheck(i), this);
            }
        }
    }

    public void SetRanking(GetLeaderboardResult result)
    {
        var curBoard = result.Leaderboard;

        nowScore = tempScore;

        foreach (PlayerLeaderboardEntry player in curBoard)
        {
            nowScore += player.StatValue;
        }

        worldScoreFillamount.fillAmount = (nowScore / (needScore * 1.0f));
        worldScoreTitleText.text = nowScore + " / " + needScore + " (" + ((nowScore / (needScore * 1.0f)) * 100).ToString("F2") + "%)";

        totalScore = (int)((nowScore / (needScore * 1.0f)) * 100);

        switch (season)
        {
            case 1:
                mySupportScoreText.text = LocalizationManager.instance.GetString("MySupportScore") + " : " + playerDataBase.WorldScore1;
                break;
            case 2:
                mySupportScoreText.text = LocalizationManager.instance.GetString("MySupportScore") + " : " + playerDataBase.WorldScore2;
                break;
            case 3:
                mySupportScoreText.text = LocalizationManager.instance.GetString("MySupportScore") + " : " + playerDataBase.WorldScore3;
                break;
            default:
                mySupportScoreText.text = LocalizationManager.instance.GetString("MySupportScore") + " : " + playerDataBase.WorldScore1;
                break;
        }
    }

    public void WorldScoreReceiveButton(int index, Action action)
    {
        switch (index)
        {
            case 0:
                if (worldScoreInformationList.Count == season - 1)
                {
                    if (totalScore < 10) return;

                    worldScoreInfo.checkOne = true;
                }
                else
                {
                    if (totalScore < 10 || worldScoreInformationList[GetWorldScoreInformation(season)].checkOne) return;

                    worldScoreInformationList[GetWorldScoreInformation(season)].checkOne = true;
                }

                PlayfabManager.instance.UpdateAddCurrency(MoneyType.Coin, 3000);

                break;
            case 1:
                if (worldScoreInformationList.Count == season - 1)
                {
                    if (totalScore < 20) return;

                    worldScoreInfo.checkTwo = true;
                }
                else
                {
                    if (totalScore < 20 || worldScoreInformationList[GetWorldScoreInformation(season)].checkTwo) return;

                    worldScoreInformationList[GetWorldScoreInformation(season)].checkTwo = true;
                }

                PlayfabManager.instance.UpdateAddCurrency(MoneyType.Crystal, 80);

                break;
            case 2:
                if (worldScoreInformationList.Count == season - 1)
                {
                    if (totalScore < 40) return;

                    worldScoreInfo.checkThree = true;
                }
                else
                {
                    if (totalScore < 40 || worldScoreInformationList[GetWorldScoreInformation(season)].checkThree) return;

                    worldScoreInformationList[GetWorldScoreInformation(season)].checkThree = true;
                }

                PlayfabManager.instance.UpdateAddCurrency(MoneyType.Coin, 6000);

                break;
            case 3:
                if (worldScoreInformationList.Count == season - 1)
                {
                    if (totalScore < 60) return;

                    worldScoreInfo.checkFour = true;
                }
                else
                {
                    if (totalScore < 60 || worldScoreInformationList[GetWorldScoreInformation(season)].checkFour) return;

                    worldScoreInformationList[GetWorldScoreInformation(season)].checkFour = true;
                }

                PlayfabManager.instance.UpdateAddCurrency(MoneyType.Crystal, 150);

                break;
            case 4:
                if (worldScoreInformationList.Count == season - 1)
                {
                    if (totalScore < 80) return;

                    worldScoreInfo.checkFive = true;
                }
                else
                {
                    if (totalScore < 80 || worldScoreInformationList[GetWorldScoreInformation(season)].checkFive) return;

                    worldScoreInformationList[GetWorldScoreInformation(season)].checkFive = true;
                }

                PlayfabManager.instance.UpdateAddCurrency(MoneyType.Coin, 10000);

                break;
            case 5:
                if (worldScoreInformationList.Count == season - 1)
                {
                    if (totalScore < 100) return;

                    worldScoreInfo.checkSix = true;
                }
                else
                {
                    if (totalScore < 100 || worldScoreInformationList[GetWorldScoreInformation(season)].checkSix) return;

                    worldScoreInformationList[GetWorldScoreInformation(season)].checkSix = true;
                }


                PlayfabManager.instance.UpdatePlayerStatisticsInsert("IconBox", 5);

                break;
        }

        titleData.Clear();

        if (worldScoreInformationList.Count == season - 1)
        {
            titleData.Add("WorldScoreSeason_" + season, JsonUtility.ToJson(worldScoreInfo));

            worldScoreInfo.season = season;
            worldScoreInformationList.Add(worldScoreInfo);
        }
        else
        {
            titleData.Add("WorldScoreSeason_" + season, JsonUtility.ToJson(worldScoreInformationList[GetWorldScoreInformation(season)]));
        }

        if (PlayfabManager.instance.isActive) PlayfabManager.instance.SetPlayerData(titleData);

        action.Invoke();

        CheckWorldScore();

        NotionManager.instance.UseNotion(NotionType.ReceiveNotion);
    }

    public int GetWorldScoreInformation(int number)
    {
        int index = 0;

        for(int i = 0; i < worldScoreInformationList.Count; i ++)
        {
            if(worldScoreInformationList[i].season == number)
            {
                index = i;
                break;
            }
        }
        return index;
    }

    #endregion


    #region Button

    [Button]
    public void Welcome1Button()
    {
        playerDataBase.BestSpeedTouchScore = 500;
        playerDataBase.WelcomeCheck = false;

        CheckWelcome();
    }

    [Button]
    public void Welcome2Button()
    {
        playerDataBase.BestMoleCatchScore = 500;
        playerDataBase.WelcomeCheck = false;

        CheckWelcome();
    }

    [Button]
    public void Welcome3Button()
    {
        playerDataBase.BestFilpCardScore = 500;
        playerDataBase.WelcomeCheck = false;

        CheckWelcome();
    }

    [Button]
    public void Welcome4Button()
    {
        playerDataBase.BestButtonActionScore = 500;
        playerDataBase.WelcomeCheck = false;

        CheckWelcome();
    }

    [Button]
    public void Welcome5Button()
    {
        playerDataBase.BestTimingActionScore = 500;
        playerDataBase.WelcomeCheck = false;

        CheckWelcome();
    }

    [Button]
    public void Welcome6Button()
    {
        playerDataBase.BestDragActionScore = 500;
        playerDataBase.WelcomeCheck = false;

        CheckWelcome();
    }

    [Button]
    public void Welcome7Button()
    {
        playerDataBase.BestLeftRightScore = 500;
        playerDataBase.WelcomeCheck = false;

        CheckWelcome();
    }

    [Button]
    public void WorldScore1Button()
    {
        playerDataBase.Season = 1;

        tempScore += 100000;

        CheckWorldScore();
    }

    [Button]
    public void WorldScore2Button()
    {
        playerDataBase.Season = 2;

        tempScore += 100000;

        CheckWorldScore();
    }

    [Button]
    public void WorldScore3Button()
    {
        playerDataBase.Season = 3;

        tempScore += 100000;

        CheckWorldScore();
    }

    #endregion
}
