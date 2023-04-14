using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class CastleManager : MonoBehaviour
{
    public GameObject castleView;
    public GameObject showVCView;

    public GameObject alarm;

    [Title("Lock")]
    public ButtonScaleAnimation button;
    public GameObject quickLockObj;
    public GameObject lockObj;
    public Text lockTimerText;


    [Title("Text")]
    public Text castleLevelText;
    public Text levelUpCostText;
    public Text timerText;
    public Text coinText;
    public Text expText;

    private int timer = 0;

    private int addCoin = 0;
    private int addExp = 0;
    private int addCrystal = 0;

    private int saveCoin = 0;
    private int saveExp = 0;

    string localization_Time;
    string localization_Hours = "";
    string localization_Minutes = "";
    string localization_Seconds = "";

    public ReceiveContent[] receiveContents;

    [Title("Animation")]
    public CoinAnimation goldAnimation;
    public SoundManager soundManager;

    DateTime time; //적립을 한 시점
    DateTime serverTime; //최대 적립 날짜
    TimeSpan timeSpan;

    WaitForSeconds waitForSeconds = new WaitForSeconds(1);

    public LevelManager levelManager;
    PlayerDataBase playerDataBase;

    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;

        castleView.SetActive(false);
        alarm.SetActive(false);
    }

    public void RewardStart()
    {
        StopAllCoroutines();
        StartCoroutine(TimerCoroution());
    }

    public void OpenCastleView()
    {
        if(!castleView.activeInHierarchy)
        {
            castleView.SetActive(true);
            showVCView.SetActive(true);

            timerText.text = "";

            localization_Time = LocalizationManager.instance.GetString("RewardTime");
            localization_Hours = LocalizationManager.instance.GetString("Hours");
            localization_Minutes = LocalizationManager.instance.GetString("Minutes");
            localization_Seconds = LocalizationManager.instance.GetString("Seconds");

            if (!GameStateManager.instance.DailyCastleReward)
            {
                quickLockObj.SetActive(false);
            }
            else
            {
                quickLockObj.SetActive(true);
                button.StopAnim();
            }

            CheckCastle();
        }
        else
        {
            castleView.SetActive(false);
            showVCView.SetActive(false);
        }
    }

    public void Initialize()
    {
        if (playerDataBase.Level >= 5)
        {
            if(playerDataBase.CastleDate.Length > 1)
            {
                time = DateTime.ParseExact(DateTime.Now.ToString("yyyy") + playerDataBase.CastleDate.Substring(1, playerDataBase.CastleDate.Length - 1),"yyyyMMddHHmm", CultureInfo.CurrentCulture);
                serverTime = DateTime.ParseExact(DateTime.Now.ToString("yyyy") + playerDataBase.CastleServerDate.Substring(1, playerDataBase.CastleServerDate.Length - 1), "yyyyMMddHHmm", CultureInfo.CurrentCulture);

            }
            else
            {
                playerDataBase.CastleDate = DateTime.Now.ToString("MMddHHmm");
                playerDataBase.CastleServerDate = DateTime.Now.AddDays(1).ToString("MMddHHmm");

                time = DateTime.Now;
                serverTime = DateTime.Now.AddDays(1);

                PlayfabManager.instance.UpdatePlayerStatisticsInsert("CastleDate", int.Parse("1" + playerDataBase.CastleDate));
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("CastleServerDate", int.Parse("1" + playerDataBase.CastleServerDate));
            }

            StartCoroutine(TimerCoroution());

            timeSpan = DateTime.Now - time;

            if(timeSpan.TotalMinutes >= 10)
            {
                OnSetAlarm();
            }
        }
    }

    public void CheckCastle()
    {
        castleLevelText.text = LocalizationManager.instance.GetString("CastleLevel") + " : " + playerDataBase.CastleLevel + " / " + (playerDataBase.Level + 1);

        addCrystal = ((playerDataBase.CastleLevel + 1) * 10);
        addCoin = (50 + ((playerDataBase.CastleLevel) * 5));
        addExp = (10 + ((playerDataBase.CastleLevel) * 1));

        levelUpCostText.text = addCrystal.ToString();
        coinText.text = addCoin.ToString() + "/" + localization_Hours + "  (+5)";
        expText.text = addExp.ToString() + "/" + localization_Hours + "  (+1)";

        receiveContents[0].Initialize(RewardType.Coin, saveCoin);
        receiveContents[1].Initialize(RewardType.Experience, saveExp);

        if (DateTime.Compare(DateTime.Now, serverTime) == 1)
        {
            timerText.text = localization_Time + " : " + "24" + localization_Hours + " " + "00" + localization_Minutes;

            CheckReward(60 * 24);

            lockObj.SetActive(false);
        }
    }

    public void CheckReward(double minute)
    {
        saveCoin = (int)Mathf.Round((addCoin / (60 * (1.0f))) * (float)minute);
        saveExp = (int)Mathf.Round((addExp / (60 * (1.0f))) * (float)minute);

        if(receiveContents[0].gameObject.activeInHierarchy)
        {
            receiveContents[0].Initialize(RewardType.Coin, saveCoin);
            receiveContents[1].Initialize(RewardType.Experience, saveExp);
        }
    }


    public void LevelUpButton()
    {
        if(playerDataBase.CastleLevel < playerDataBase.Level + 1)
        {
            if(playerDataBase.Crystal >= addCrystal)
            {
                PlayfabManager.instance.UpdateSubtractCurrency(MoneyType.Crystal, addCrystal);

                playerDataBase.CastleLevel += 1;

                PlayfabManager.instance.UpdatePlayerStatisticsInsert("CastleLevel", playerDataBase.CastleLevel);

                CheckCastle();

                soundManager.PlaySFX(GameSfxType.LevelUp);

                NotionManager.instance.UseNotion(NotionType.UpgradeSuccess);
            }
            else
            {
                NotionManager.instance.UseNotion(NotionType.LowCrystalNotion);
            }
        }
        else
        {
            NotionManager.instance.UseNotion(NotionType.UpgradeMax);
        }
    }

    public void ReceiveButton()
    {
        if(!lockObj.activeInHierarchy)
        {
            playerDataBase.CastleDate = DateTime.Now.ToString("MMddHHmm");
            playerDataBase.CastleServerDate = DateTime.Now.AddDays(1).ToString("MMddHHmm");

            time = DateTime.Now;
            serverTime = DateTime.Now.AddDays(1);

            PlayfabManager.instance.UpdatePlayerStatisticsInsert("CastleDate", int.Parse("1" + playerDataBase.CastleDate));
            PlayfabManager.instance.UpdatePlayerStatisticsInsert("CastleServerDate", int.Parse("1" + playerDataBase.CastleServerDate));

            PlayfabManager.instance.UpdateAddCurrency(MoneyType.Coin, saveCoin);
            levelManager.CheckLevelUp(saveExp);

            goldAnimation.OnPlayExpAnimation();

            StopAllCoroutines();
            StartCoroutine(TimerCoroution());

            OnCheckAlarm();

            NotionManager.instance.UseNotion(NotionType.ReceiveNotion);
        }
    }

    IEnumerator TimerCoroution()
    {
        if(DateTime.Compare(DateTime.Now, serverTime) == -1)
        {
            timeSpan = DateTime.Now - time;

            if(timeSpan.TotalMinutes > 0)
            {
                CheckReward(timeSpan.TotalMinutes);
            }
        }
        else
        {
            yield break;
        }

        if (timeSpan.Hours > 0)
        {
            timerText.text = localization_Time + " : " + timeSpan.Hours.ToString("D2") + localization_Hours + " " + timeSpan.Minutes.ToString("D2") + localization_Minutes;
        }
        else
        {
            if (timeSpan.Minutes == 0)
            {
                timerText.text = localization_Time + " : " + timeSpan.Seconds.ToString("D2") + localization_Seconds;
            }
            else
            {
                timerText.text = localization_Time + " : " + timeSpan.Minutes.ToString("D2") + localization_Minutes + " " + timeSpan.Seconds.ToString("D2") + localization_Seconds;
            }
        }

        if (timeSpan.Minutes >= 10 || timeSpan.Hours >= 1)
        {
            lockObj.SetActive(false);
        }
        else
        {
            lockObj.SetActive(true);

            if(timeSpan.Seconds == 0)
            {
                lockTimerText.text = (10 - timeSpan.Minutes).ToString("D2") + ":00";
            }
            else if(timeSpan.Seconds == 60)
            {
                lockTimerText.text = (9 - timeSpan.Minutes).ToString("D2") + ":00";
            }
            else
            {
                lockTimerText.text = (9 - timeSpan.Minutes).ToString("D2") + ":" + (60 - timeSpan.Seconds).ToString("D2");
            }
        }

        yield return waitForSeconds;
        StartCoroutine(TimerCoroution());
    }

    public void SuccessWatchAd()
    {
        GameStateManager.instance.DailyCastleReward = true;

        quickLockObj.SetActive(true);

        button.StopAnim();

        int quickCoin = addCoin * 12;
        int quickExp = addExp * 12;

        PlayfabManager.instance.UpdateAddCurrency(MoneyType.Coin, quickCoin);
        levelManager.CheckLevelUp(addExp);

        goldAnimation.OnPlayExpAnimation();

        NotionManager.instance.UseNotion(NotionType.SuccessWatchAd);
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
