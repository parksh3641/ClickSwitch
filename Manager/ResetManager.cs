using PlayFab;
using PlayFab.ClientModels;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class GameModeLevel
{
    public GamePlayType gamePlayType = GamePlayType.GameChoice1;
    public GameModeType gameModeType = GameModeType.Easy;

    public bool easy = true;
    public bool normal = false;
    public bool hard = false;
}

public class ResetManager : MonoBehaviour
{
    GamePlayType type = GamePlayType.GameChoice1;
    GameModeLevel gameModeLevel;

    public GameObject gameMenuView;

    public Text nextEventText;

    string localization = "";

    DateTime serverTime;

    [Title("Normal Mode")]
    public ModeContent[] gameModeContentArray;

    [Space]
    [Title("Perfect Mode")]
    public EventModeContent eventModeContent;
    public GameObject waitForNextEventObj;


    public ShopManager shopManager;

    PlayerDataBase playerDataBase;

    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;

        nextEventText.text = "";

        gameMenuView.SetActive(false);

        waitForNextEventObj.SetActive(false);
    }

    public void OpenMenu()
    {
        if (!gameMenuView.activeSelf)
        {
            gameMenuView.SetActive(true);

            OnCheckAttendanceDay();

            CheckGameMode();

            nextEventText.text = "";

            localization = LocalizationManager.instance.GetString("NextEvent");
        }
        else
        {
            gameMenuView.SetActive(false);
        }
    }

    public void OnCheckAttendanceDay()
    {
        if (PlayfabManager.instance.isActive) PlayfabManager.instance.GetServerTime(SetModeContent);
    }

    private void SetModeContent(System.DateTime time)
    {
        SetNextEventTime(time);

        if (playerDataBase.AttendanceDay.Length < 2)
        {
            Debug.Log("처음 로그인");

            playerDataBase.AttendanceDay = System.DateTime.Now.ToString("yyyyMMdd");

            if (PlayfabManager.instance.isActive)
            {
                playerDataBase.AccessDate += 1;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("AccessDate", playerDataBase.AccessDate);

                PlayfabManager.instance.UpdatePlayerStatisticsInsert("AttendanceDay", int.Parse(playerDataBase.AttendanceDay));
            }

            GameStateManager.instance.TryCount = 1;
            GameStateManager.instance.EventWatchAd = false;
            GameStateManager.instance.DailyShopReward = false;
            GameStateManager.instance.DailyShopAdsReward = false;
            GameStateManager.instance.StartPack = false;
            GameStateManager.instance.CoinRushTryCount = 1;
            GameStateManager.instance.CoinRushWatchAd = false;
        }

        if (ComparisonDate(playerDataBase.AttendanceDay, time))
        {
            Debug.Log("날짜 초기화");

            type = GamePlayType.GameChoice1;

            switch (System.DateTime.Now.DayOfWeek)
            {
                case System.DayOfWeek.Friday:
                    type = GamePlayType.GameChoice5;
                    break;
                case System.DayOfWeek.Monday:
                    type = GamePlayType.GameChoice1;
                    break;
                case System.DayOfWeek.Saturday:
                    type = GamePlayType.GameChoice6;
                    break;
                case System.DayOfWeek.Sunday:
                    type = GamePlayType.GameChoice7;
                    break;
                case System.DayOfWeek.Thursday:
                    type = GamePlayType.GameChoice4;
                    break;
                case System.DayOfWeek.Tuesday:
                    type = GamePlayType.GameChoice2;
                    break;
                case System.DayOfWeek.Wednesday:
                    type = GamePlayType.GameChoice3;
                    break;
            }

            if (eventModeContent.gameObject.activeInHierarchy)
            {
                eventModeContent.Initialize(type, GameModeType.Perfect);
                eventModeContent.SetClearInformation(type, GameModeType.Perfect);
            }

            playerDataBase.GameMode = ((int)type).ToString();
            playerDataBase.AttendanceDay = System.DateTime.Now.AddDays(1).ToString("yyyyMMdd");

            if (PlayfabManager.instance.isActive)
            {
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("GameMode", (int)type);

                playerDataBase.AccessDate += 1;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("AccessDate", playerDataBase.AccessDate);

                PlayfabManager.instance.UpdatePlayerStatisticsInsert("AttendanceDay", int.Parse(playerDataBase.AttendanceDay));
            }

            GameStateManager.instance.TryCount = 1;
            GameStateManager.instance.EventWatchAd = false;
            GameStateManager.instance.DailyShopReward = false;
            GameStateManager.instance.DailyShopAdsReward = false;
            GameStateManager.instance.StartPack = false;
            GameStateManager.instance.CoinRushTryCount = 1;
            GameStateManager.instance.CoinRushWatchAd = false;


            shopManager.DailyInitialize();
        }
        else
        {
            Debug.Log("아직 하루가 안 지났습니다.");

            if(playerDataBase.GameMode == "")
            {
                Debug.Log("강제 초기화");
                playerDataBase.AttendanceDay = "";
                OnCheckAttendanceDay();
            }

            if (eventModeContent.gameObject.activeInHierarchy)
            {
                eventModeContent.Initialize(GamePlayType.GameChoice1 + int.Parse(playerDataBase.GameMode.ToString()), GameModeType.Perfect);
                eventModeContent.SetClearInformation(GamePlayType.GameChoice1 + int.Parse(playerDataBase.GameMode.ToString()), GameModeType.Perfect);
            }
        }
    }

    public bool ComparisonDate(string target, System.DateTime time)
    {
        System.DateTime server = time;
        System.DateTime system = System.DateTime.ParseExact(target, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);

        bool c = false;

        if (server.Year > system.Year)
        {
            c = true;
        }
        else
        {
            if (server.Year == system.Year)
            {
                if (server.Month > system.Month)
                {
                    c = true;
                }
                else
                {
                    if (server.Month == system.Month)
                    {
                        if (server.Day >= system.Day)
                        {
                            c = true;
                        }
                        else
                        {
                            c = false;
                        }
                    }
                    else
                    {
                        c = false;
                    }
                }
            }
            else
            {
                c = false;
            }
        }

        return c;
    }

    public void SetNextEventTime(DateTime time)
    {
        serverTime = time;

        StopAllCoroutines();
        StartCoroutine(RemainTimerCourtion());
    }

    IEnumerator RemainTimerCourtion()
    {
        serverTime = serverTime.AddSeconds(-1);

        nextEventText.text = localization + " : " + serverTime.ToString("hh:mm:ss");

        yield return new WaitForSeconds(1f);

        StartCoroutine(RemainTimerCourtion());
    }

    public void CheckGameMode()
    {
        for(int i = 0; i < gameModeContentArray.Length; i ++)
        {
            gameModeLevel = playerDataBase.GetGameMode(GamePlayType.GameChoice1 + i);

            if(gameModeLevel.normal)
            {
                gameModeContentArray[i].UnLock();
            }

            if(gameModeLevel.gameModeType != GameModeType.Perfect)
            {
                gameModeContentArray[i].ChangeGameMode(gameModeLevel.gameModeType);
            }
        }
    }
}
