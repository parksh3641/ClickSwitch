using Firebase.Analytics;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, IGameEvent
{
    [Title("Event")]
    public UnityEvent eGameStart;
    public UnityEvent eGamePause;
    public UnityEvent eGameEnd;

    public Text timerText;
    public Text scoreText;
    public LocalizationContent levelText;

    public LocalizationContent infoBestScoreText;
    public LocalizationContent infoBestComboText;

    public WarningController warningController;

    [Space]
    [Title("CurrneyUI")]
    public GameObject virtualCurrencyUI;
    public Text[] goldText;
    public Text[] crystalText;

    public GameObject gamePlayView;
    public GameObject[] gamePlayUI;

    [Space]
    [Title("MainUI")]
    public GameObject gameStartUI;
    public GameObject gameReadyUI;
    public Text gameReadyText;

    [Title("ItemUI")]
    public ItemUseContent[] itemUseContentArray;

    [Space]
    [Title("TutorialUI")]
    public GameObject tutorialUI;
    public LocalizationContent tutorialText;

    [Space]
    [Title("EndUI")]
    public GameObject gameEndAnimView;
    public BarAnimation gameEndAnimation;
    public GameObject gameEndUI;
    public GameObject newRecordObj;
    public GameObject newComboObj;
    public GameObject doubleCoinObj;
    public GameObject doubleExpObj;
    public Text plusGoldText;
    public Text plusExpText;
    public Text nowScoreText;
    public Text nowComboText;
    public Text getGoldText;
    public Text getExpText;

    [Space]
    [Title("Trophy")]
    public GameObject trophyView;
    public Image trophyIcon;
    Sprite[] iconArray;

    [Space]
    [Title("GameMode")]
    public GameObject gameModeView;
    public LocalizationContent gameModeText;

    [Space]
    [Title("Ad")]
    public GameObject watchAdLock;
    public Text watchAdCountText;


    [Space]
    [Title("OptionUI")]
    public GameObject gameOptionUI;
    public GameObject languageUI;
    public GameObject[] etcUI;
    public LanguageContent[] languageContentArray;
    public Text versionText2;
    public RectTransform languageGrid;
    public GameObject logOutView;
    public GameObject repairView;

    public GameObject etcView;


    [Space]
    [Title("CancleUI")]
    public GameObject cancleWindowUI;
    public GameObject cancleUI;

    [Space]
    [Title("LoginUI")]
    public FadeInOut loadingUI;
    public Text messageText;
    public GameObject loginUI;
    public GameObject[] loginButtonList;
    public Text platformText;
    public Text versionText;

    public GameObject updateUI;

    [Space]
    [Title("NotionUI")]
    public Notion scoreNotion;
    public Notion coinNotion;

    [Space]
    [Title("Network")]
    public GameObject networkView;

    [Space]
    [Title("Value")]
    private float score = 0;
    private int bestScore = 0;
    private int bestCombo = 0;
    private int adCoolTime = 0;
    private int plusCoin = 0;

    [Title("Bool")]
    [SerializeField]
    private bool pause = false;
    private bool checkUI = false;

    [Title("Manager")]
    public ComboManager comboManager;
    public RankingManager rankingManager;
    public ProfileManager profileManager;
    public NickNameManager nickNameManager;
    public SoundManager soundManager;
    public ShopManager shopManager;
    public IconManager iconManager;
    public NewsManager newsManager;
    public LevelManager levelManager;
    public ResetManager resetManager;
    public TrophyManager trophyManager;
    public HelpManager helpManager;
    public MailBoxManager mailBoxManager;
    public DailyManager dailyManager;
    public ModeManager modeManager;
    public UpgradeManager upgradeManager;
    public BannerManager bannerManager;
    public ProgressManager progressManager;
    public ReviewManager reviewManager;
    public ItemManager itemManager;
    public LockManager lockManager;
    public WeeklyManager weeklyManager;
    public AttendanceManager attendanceManager;
    public EventManager eventManager;
    public CastleManager castleManager;

    [Title("Animation")]
    public CoinAnimation goldAnimation;


    [Title("DataBase")]
    PlayerDataBase playerDataBase;
    UpgradeDataBase upgradeDataBase;
    ImageDataBase imageDataBase;
    ShopDataBase shopDataBase;

    private Dictionary<string, string> playerData = new Dictionary<string, string>();

    WaitForSeconds waitForSeconds = new WaitForSeconds(1);
    WaitForSeconds waitForSeconds2 = new WaitForSeconds(0.1f);


    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;
        if (upgradeDataBase == null) upgradeDataBase = Resources.Load("UpgradeDataBase") as UpgradeDataBase;
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;
        if (shopDataBase == null) shopDataBase = Resources.Load("ShopDataBase") as ShopDataBase;

        iconArray = imageDataBase.GetIconArray();

        updateUI.SetActive(false);

        GameManager.eGameStart += this.GameStart;
        GameManager.eGamePause += this.GamePause;

        GameManager.PlusScore += this.PlusScore;
        GameManager.MinusScore += this.MinusScore;
        GameManager.PlusCoin += this.PlusCoin;

        timerText.text = "";
        timerText.color = new Color(1, 1, 0);
        scoreText.text = "";

        versionText.text = "v" + Application.version;
        versionText2.text = "v" + Application.version;

        infoBestScoreText.gameObject.SetActive(false);
        infoBestComboText.gameObject.SetActive(false);

        goldText[0].text = "0";
        goldText[1].text = "0";
        crystalText[0].text = "0";
        crystalText[1].text = "0";

        scoreNotion.gameObject.SetActive(false);
        coinNotion.gameObject.SetActive(false);

        gameOptionUI.SetActive(false);
        languageUI.SetActive(false);

        logOutView.SetActive(false);
        repairView.SetActive(false);
        etcView.SetActive(false);

        gamePlayView.SetActive(false);

        for (int i = 0; i < gamePlayUI.Length; i ++)
        {
            gamePlayUI[i].SetActive(false);
        }

        gameStartUI.SetActive(true);
        gameReadyUI.SetActive(false);
        tutorialUI.SetActive(false);
        gameEndAnimView.SetActive(false);
        gameEndUI.SetActive(false);
        cancleWindowUI.SetActive(false);
        cancleUI.SetActive(false);
        networkView.SetActive(false);
        trophyView.SetActive(false);
        gameModeView.SetActive(false);
    }

    private void Start()
    {
        if (languageGrid != null) languageGrid.anchoredPosition = new Vector2(0, -9999);

        loginUI.SetActive(false);
        loadingUI.gameObject.SetActive(true);

        StartCoroutine(LoadingCoroution());

        if(!GameStateManager.instance.AutoLogin)
        {
            loginUI.SetActive(true);
            SetLoginUI();
        }
    }

    IEnumerator LoadingCoroution()
    {
        if(loadingUI.gameObject.activeInHierarchy)
        {
            messageText.text = LocalizationManager.instance.GetString("Login");
            yield return new WaitForSeconds(0.5f);
            messageText.text = LocalizationManager.instance.GetString("Login") + ".";
            yield return new WaitForSeconds(0.5f);
            messageText.text = LocalizationManager.instance.GetString("Login") + "..";
            yield return new WaitForSeconds(0.5f);
            messageText.text = LocalizationManager.instance.GetString("Login") + "...";
            yield return new WaitForSeconds(0.5f);
        }
        else
        {
            yield break;
        }

        StartCoroutine(LoadingCoroution());
    }

    //void Update()
    //{
    //    if(Application.platform == RuntimePlatform.Android)
    //    {
    //        if(Input.GetKey(KeyCode.Escape))
    //        {
    //            if(resetManager.gameMenuView.activeInHierarchy)
    //            {
    //                resetManager.OpenMenu();
    //            }
    //            else if(rankingManager.rankingView.activeInHierarchy)
    //            {
    //                rankingManager.OpenRanking();
    //            }
    //            else if(profileManager.profileView.activeInHierarchy)
    //            {
    //                if (iconManager.iconView.activeInHierarchy)
    //                {
    //                    iconManager.OpenIcon();
    //                }
    //                else if (nickNameManager.nickNameView.activeInHierarchy)
    //                {
    //                    nickNameManager.OpenNickName();
    //                }
    //                else
    //                {
    //                    profileManager.OpenProfile();
    //                }
    //            }
    //            else if(shopManager.shopView.activeInHierarchy)
    //            {
    //                shopManager.OpenShop();
    //            }
    //            else if(gameOptionUI.activeInHierarchy)
    //            {
    //                OpenOption();
    //            }
    //            else if (newsManager.newsView.activeInHierarchy)
    //            {
    //                newsManager.OpenNews();
    //            }
    //            else if (trophyManager.trophyView.activeInHierarchy)
    //            {
    //                trophyManager.OpenTrophy();
    //            }
    //            else if (helpManager.helpView.activeInHierarchy)
    //            {
    //                helpManager.OpenHelp();
    //            }
    //            else
    //            {
    //                Debug.Log("???? ????");
    //            }
    //        }
    //    }
    //}

    public void SetLoginUI()
    {
        //platformText.text = LocalizationManager.instance.GetString("Platform");

        for (int i = 0; i < loginButtonList.Length; i++)
        {
            loginButtonList[i].SetActive(false);
        }

#if UNITY_ANDROID
        loginButtonList[0].SetActive(true);
#elif UNITY_IOS
        loginButtonList[1].SetActive(true);
#endif
    }


    private void OnApplicationQuit()
    {
        GameManager.eGameStart -= this.GameStart;
        GameManager.eGamePause -= this.GamePause;

        GameManager.PlusScore -= this.PlusScore;
        GameManager.MinusScore -= this.MinusScore;
        GameManager.PlusCoin -= this.PlusCoin;
    }

    private void OnApplicationFocus(bool focus)
    {
        //if (GameStateManager.instance.DontStopGame) return;

        if (focus)
        {

        }
        else
        {
            if (cancleUI.activeSelf)
            {
                OpenGameStop();
            }
        }
    }

    public void RenewalVC()
    {
        Debug.Log("Renewal VC");

        goldText[0].text = playerDataBase.Coin.ToString();
        goldText[1].text = playerDataBase.Coin.ToString();
        crystalText[0].text = playerDataBase.Crystal.ToString();
        crystalText[1].text = playerDataBase.Crystal.ToString();
    }

    public void SetItem()
    {
        int useItem = 0;

        Sprite[] itemArray = imageDataBase.GetItemArray();

        for(int i = 0; i < itemUseContentArray.Length; i ++)
        {
            itemUseContentArray[i].Initialize(itemArray[i]);
        }

        if(GameStateManager.instance.WatchAdItem)
        {
            if(GameStateManager.instance.GameModeType == GameModeType.Perfect)
            {
                itemUseContentArray[1].UseItem();
                itemUseContentArray[2].UseItem();
                itemUseContentArray[3].UseItem();
                useItem += 3;
            }
            else
            {
                if(GameStateManager.instance.GamePlayType != GamePlayType.GameChoice8)
                {
                    for (int i = 0; i < itemUseContentArray.Length; i++)
                    {
                        itemUseContentArray[i].UseItem();
                        useItem++;
                    }
                }
            }
        }
        else
        {
            if (GameStateManager.instance.Clock)
            {
                itemUseContentArray[0].UseItem();
                useItem++;
            }
            if (GameStateManager.instance.Shield)
            {
                itemUseContentArray[1].UseItem();
                useItem++;
            }
            if (GameStateManager.instance.Combo)
            {
                itemUseContentArray[2].UseItem();
                useItem++;
            }
            if (GameStateManager.instance.Exp)
            {
                itemUseContentArray[3].UseItem();
                useItem++;
            }
            if (GameStateManager.instance.Slow)
            {
                itemUseContentArray[4].UseItem();
                useItem++;
            }
        }
        weeklyManager.UpdateWeeklyMissionReport(WeeklyMissionType.UseItem, useItem);
    }

    public void UsedItem(ItemType type)
    {
        itemUseContentArray[(int)type].UsedItem();

        FirebaseAnalytics.LogEvent(type.ToString());
    }

    void SetEtcUI(bool check)
    {
        virtualCurrencyUI.SetActive(check);

        for(int i = 0; i < etcUI.Length; i ++)
        {
            etcUI[i].SetActive(check);
        }

        cancleUI.SetActive(!check);
    }


#region Button
    public void OpenMenu()
    {
        resetManager.OpenGameMenu();
    }

    public void CloseMenu()
    {
        resetManager.gameMenuView.SetActive(false);
    }

    public void CheckMenu()
    {
        resetManager.CheckGameMode();
    }

    public void OpenGamePlayUI(GamePlayType type)
    {
        gameStartUI.SetActive(false);
        gamePlayView.SetActive(true);

        gamePlayUI[(int)type].SetActive(true);

        SetItem();

        infoBestScoreText.gameObject.SetActive(true);
        infoBestComboText.gameObject.SetActive(true);

        bestScore = 0;
        bestCombo = 0;
        plusCoin = 0;

        switch (type)
        {
            case GamePlayType.GameChoice1:
                bestScore = playerDataBase.BestSpeedTouchScore;
                bestCombo = playerDataBase.BestSpeedTouchCombo;
                break;
            case GamePlayType.GameChoice2:
                bestScore = playerDataBase.BestMoleCatchScore;
                bestCombo = playerDataBase.BestMoleCatchCombo;
                break;
            case GamePlayType.GameChoice3:
                bestScore = playerDataBase.BestFilpCardScore;
                bestCombo = playerDataBase.BestFilpCardCombo;
                break;
            case GamePlayType.GameChoice4:
                bestScore = playerDataBase.BestButtonActionScore;
                bestCombo = playerDataBase.BestButtonActionCombo;
                break;
            case GamePlayType.GameChoice5:
                bestScore = playerDataBase.BestTimingActionScore;
                bestCombo = playerDataBase.BestTimingActionCombo;
                break;
            case GamePlayType.GameChoice6:
                bestScore = playerDataBase.BestDragActionScore;
                bestCombo = playerDataBase.BestDragActionCombo;
                break;
            case GamePlayType.GameChoice7:
                bestScore = playerDataBase.BestLeftRightScore;
                bestCombo = playerDataBase.BestLeftRightCombo;
                break;
            case GamePlayType.GameChoice8:
                bestScore = playerDataBase.BestCoinRushScore;
                bestCombo = playerDataBase.BestCoinRushCombo;
                break;
            default:
                bestScore = 0;
                bestCombo = 0;
                break;
        }

        comboManager.SetBestCombo(bestCombo);

        infoBestScoreText.SetNumber(bestScore);
        infoBestComboText.SetNumber(bestCombo);

        comboManager.barAnimation.OnReset();
        gameEndAnimation.OnReset();
        gameEndAnimView.SetActive(false);
    }

    public void CloseGamePlayUI()
    {
        gamePlayView.SetActive(false);

        infoBestScoreText.gameObject.SetActive(false);
        infoBestComboText.gameObject.SetActive(false);

        for (int i = 0; i < gamePlayUI.Length; i ++)
        {
            gamePlayUI[i].SetActive(false);
        }
    }

    #region Option
    public void OpenOption()
    {
        if(gameOptionUI.activeSelf)
        {
            gameOptionUI.SetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            gameOptionUI.SetActive(true);

            FirebaseAnalytics.LogEvent("OpenSetting");

            if(GameStateManager.instance.PlayGame)
            {
                Time.timeScale = 0;
            }
        }
    }

    public void OpenLanguage()
    {
        if (languageUI.activeSelf)
        {
            languageUI.SetActive(false);
        }
        else
        {
            languageUI.SetActive(true);

            FirebaseAnalytics.LogEvent("OpenLanguage");

            for (int i = 0; i < languageContentArray.Length; i++)
            {
                languageContentArray[i].OnFrame(false);
            }

            languageContentArray[(int)GameStateManager.instance.Language -1].OnFrame(true);
        }
    }

    public void OpenLogOutView()
    {
        logOutView.SetActive(true);
    }

    public void CloseLogOutView()
    {
        logOutView.SetActive(false);
    }

    public void LogOutButton()
    {
        CloseLogOutView();

        PlayfabManager.instance.LogOut();
    }

    public void OpenRepairView()
    {
        repairView.SetActive(true);
    }

    public void CloseRepairView()
    {
        repairView.SetActive(false);
    }

    public void RepairButton()
    {
        CloseRepairView();

        PlayerPrefs.SetInt("Repair", 1);

        PlayfabManager.instance.LogOut();
    }

    #endregion
    public void OpenRanking()
    {
        rankingManager.OpenRanking();

        FirebaseAnalytics.LogEvent("OpenRanking");
    }

    public void OpenProfile()
    {
        profileManager.OpenProfile();

        FirebaseAnalytics.LogEvent("OpenProfile");
    }

    public void OpenNickName()
    {
        nickNameManager.OpenNickName();

        FirebaseAnalytics.LogEvent("OpenNickName");
    }

    public void OpenShop()
    {
        shopManager.OpenShop();

        FirebaseAnalytics.LogEvent("OpenShop");
    }

    public void OpenIcon()
    {
        iconManager.OpenIcon();

        FirebaseAnalytics.LogEvent("OpenIcon");
    }

    public void OpenNews()
    {
        etcView.SetActive(false);

        newsManager.OpenNews();

        FirebaseAnalytics.LogEvent("OpenNews");
    }

    public void OpenTrophy()
    {
        trophyManager.OpenTrophy();

        FirebaseAnalytics.LogEvent("OpenTrophy");
    }

    public void OpenHelp()
    {
        etcView.SetActive(false);

        helpManager.OpenHelp();

        FirebaseAnalytics.LogEvent("OpenHelp");
    }

    public void OpenMailBox()
    {
        etcView.SetActive(false);

        mailBoxManager.OpenMail();

        FirebaseAnalytics.LogEvent("OpenMail");
    }

    public void OpenDailyMission()
    {
        dailyManager.OpenDailyView();

        FirebaseAnalytics.LogEvent("OpenQuest");
    }

    public void OpenUpgrade()
    {
        upgradeManager.OpenUpgrade();

        FirebaseAnalytics.LogEvent("OpenUpgrade");
    }

    public void OpenBanner()
    {
        bannerManager.OpenBanner();

        FirebaseAnalytics.LogEvent("OpenBanner");
    }

    public void OpenProgress()
    {
        progressManager.OpenProgress();

        if(gameStartUI.activeInHierarchy)
        {
            gameStartUI.SetActive(false);
        }
        else
        {
            gameStartUI.SetActive(true);
        }

        FirebaseAnalytics.LogEvent("OpenProgress");
    }

    public void OpenETC()
    {
        if(!etcView.activeInHierarchy)
        {
            etcView.SetActive(true);
        }
        else
        {
            etcView.SetActive(false);
        }

        FirebaseAnalytics.LogEvent("OpenETC");
    }

    public void OpenAttendance()
    {
        attendanceManager.OpenAttendanceView();

        FirebaseAnalytics.LogEvent("OpenAttendance");
    }

    public void OpenEvent()
    {
        eventManager.OpenEventView();

        FirebaseAnalytics.LogEvent("OpenEvent");
    }

    public void OpenCastle()
    {
        castleManager.OpenCastleView();

        FirebaseAnalytics.LogEvent("OpenCastle");
    }

    public void OnLoginSuccess()
    {
        loadingUI.FadeIn();

        loginUI.SetActive(false);

        RenewalVC();
    }

    public void OnLogout()
    {
        OpenOption();

        loginUI.SetActive(true);

        SetLoginUI();
    }

    public void OnNeedUpdate()
    {
        loginUI.SetActive(false);

        updateUI.SetActive(true);
    }

    public void OnUpdate()
    {
#if UNITY_ANDROID
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.unity3d.toucharcade");
#elif UNITY_IOS
        Application.OpenURL("https://apps.apple.com/us/app/gosu-of-touch-tap-arcade/id1637056029");
#else
        Application.OpenURL("https://apps.apple.com/us/app/gosu-of-touch-tap-arcade/id1637056029");
#endif
    }

    #endregion
    public void GameStart()
    {
        Debug.Log("Game Start");

        itemManager.OpenItem();

        GameStateManager.instance.PlayGame = true;

        pause = false;

        StartCoroutine("ReadyTimerCoroution", ValueManager.instance.GetReadyTime());

        SetEtcUI(false);

        levelText.localizationName = "Info_" + GameStateManager.instance.GameModeType.ToString();
        levelText.ReLoad();

        modeManager.OnMode();

        CheckTutorial();
    }

    void CheckTutorial()
    {
        tutorialUI.SetActive(false);

        switch (GameStateManager.instance.GamePlayType)
        {
            case GamePlayType.GameChoice1:
                if(playerDataBase.BestSpeedTouchScore == 0 && !GameStateManager.instance.GameChoice1Tutorial)
                {
                    tutorialUI.SetActive(true);
                    tutorialText.localizationName = "Information_GameChoice1";
                    tutorialText.ReLoad();

                    GameStateManager.instance.GameChoice1Tutorial = true;
                }

                break;
            case GamePlayType.GameChoice2:
                if (playerDataBase.BestMoleCatchScore == 0 && !GameStateManager.instance.GameChoice2Tutorial)
                {
                    tutorialUI.SetActive(true);
                    tutorialText.localizationName = "Information_GameChoice2";
                    tutorialText.ReLoad();

                    GameStateManager.instance.GameChoice2Tutorial = true;
                }

                break;
            case GamePlayType.GameChoice3:

                break;
            case GamePlayType.GameChoice4:
                if (playerDataBase.BestButtonActionScore == 0 && !GameStateManager.instance.GameChoice4Tutorial)
                {
                    tutorialUI.SetActive(true);
                    tutorialText.localizationName = "Information_GameChoice4";
                    tutorialText.ReLoad();

                    GameStateManager.instance.GameChoice4Tutorial = true;
                }

                break;
            case GamePlayType.GameChoice5:
                if (playerDataBase.BestTimingActionScore == 0 && !GameStateManager.instance.GameChoice5Tutorial)
                {
                    tutorialUI.SetActive(true);
                    tutorialText.localizationName = "Information_GameChoice5";
                    tutorialText.ReLoad();

                    GameStateManager.instance.GameChoice5Tutorial = true;
                }

                break;
            case GamePlayType.GameChoice6:
                if (playerDataBase.BestDragActionScore == 0 && !GameStateManager.instance.GameChoice6Tutorial)
                {
                    tutorialUI.SetActive(true);
                    tutorialText.localizationName = "Information_GameChoice6";
                    tutorialText.ReLoad();

                    GameStateManager.instance.GameChoice6Tutorial = true;
                }

                break;
            case GamePlayType.GameChoice7:
                if (playerDataBase.BestLeftRightScore == 0 && !GameStateManager.instance.GameChoice7Tutorial)
                {
                    tutorialUI.SetActive(true);
                    tutorialText.localizationName = "Information_GameChoice7";
                    tutorialText.ReLoad();

                    GameStateManager.instance.GameChoice7Tutorial = true;
                }

                break;
            case GamePlayType.GameChoice8:
                if (playerDataBase.BestCoinRushScore == 0 && !GameStateManager.instance.GameChoice8Tutorial)
                {
                    tutorialUI.SetActive(true);
                    tutorialText.localizationName = "Information_GameChoice8";
                    tutorialText.ReLoad();

                    GameStateManager.instance.GameChoice8Tutorial = true;
                }

                break;
        }
    }

    public void GamePause()
    {
        if(pause)
        {
            pause = false;
        }
        else
        {
            pause = true;
        }
    }

    public void GameEndAnimation()
    {
        gameEndAnimView.SetActive(true);
        gameEndAnimation.PlayAnimation();

        soundManager.StopBGM();
        soundManager.PlaySFX(GameSfxType.GameOver);

        StartCoroutine(GameEndCoroution());
    }

    IEnumerator GameEndCoroution()
    {
        yield return new WaitForSeconds(3f);
        GameEnd();
    }

    float money = 0;
    float exp = 0;
    float plus = 0;

    public void GameEnd()
    {
        Debug.Log("Game End");

        CloseGamePlayUI();
        gameEndAnimation.OnReset();
        gameEndAnimView.SetActive(false);
        gameEndUI.SetActive(true);

        soundManager.PlayBGM(GameBGMType.End);

        if (!NetworkConnect.instance.CheckConnectInternet())
        {
            networkView.SetActive(true);
            return;
        }
        else
        {
            networkView.SetActive(false);
        }

        //FirebaseAnalytics.LogEvent(GameStateManager.instance.GamePlayType.ToString(), "GetScore", score);
        //FirebaseAnalytics.LogEvent(GameStateManager.instance.GamePlayType.ToString(), "GetCombo", comboManager.GetCombo());

        switch (GameStateManager.instance.GameModeType)
        {
            case GameModeType.Easy:
                break;
            case GameModeType.Normal:
                score = score + (score * 0.3f);
                break;
            case GameModeType.Hard:
                score = score + (score * 0.5f);
                break;
            case GameModeType.Perfect:
                break;
        }

        float plusIcon = shopDataBase.GetIconHoldNumber() * 0.005f;
        float plusTrophy = playerDataBase.GetTrophyHoldNumber() * 0.02f;
        float plusLevel = playerDataBase.AddScoreLevel * 0.002f;

        score = score + (score * plusIcon) + (score * plusTrophy) + (score * plusLevel);

        GameModeLevel gameModeLevel = playerDataBase.GetGameMode(GameStateManager.instance.GamePlayType);

        weeklyManager.UpdateWeeklyMissionReport(WeeklyMissionType.GetScore, (int)score);


        switch(playerDataBase.Season)
        {
            case 1:
                playerDataBase.WorldScore1 += (int)score;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("WorldScore1", playerDataBase.WorldScore1);

                break;
            case 2:
                playerDataBase.WorldScore2 += (int)score;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("WorldScore1", playerDataBase.WorldScore2);

                break;
            case 3:
                playerDataBase.WorldScore3 += (int)score;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("WorldScore1", playerDataBase.WorldScore3);

                break;
            default:
                playerDataBase.WorldScore1 += (int)score;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("WorldScore1", playerDataBase.WorldScore1);

                break;
        }

        switch (GameStateManager.instance.GameModeType)
        {
            case GameModeType.Easy:
                if ((int)score >= ValueManager.instance.GetNormalClearScore(GameStateManager.instance.GamePlayType) && !gameModeLevel.normal)
                {
                    gameModeView.SetActive(true);

                    gameModeText.localizationName = "Normal";
                    gameModeText.ReLoad();

                    gameModeLevel.normal = true;
                    playerDataBase.SetGameMode(GameStateManager.instance.GamePlayType, gameModeLevel);

                    playerData.Clear();
                    playerData.Add("GameMode_" + (int)gameModeLevel.gamePlayType, JsonUtility.ToJson(gameModeLevel));

                    if (PlayfabManager.instance.isActive) PlayfabManager.instance.SetPlayerData(playerData);
                }
                break;
            case GameModeType.Normal:
                if ((int)score >= ValueManager.instance.GetHardClearScore(GameStateManager.instance.GamePlayType) && !gameModeLevel.hard)
                {
                    gameModeView.SetActive(true);

                    gameModeText.localizationName = "Hard";
                    gameModeText.ReLoad();

                    gameModeLevel.hard = true;
                    playerDataBase.SetGameMode(GameStateManager.instance.GamePlayType, gameModeLevel);

                    playerData.Clear();
                    playerData.Add("GameMode_" + (int)gameModeLevel.gamePlayType, JsonUtility.ToJson(gameModeLevel));

                    if (PlayfabManager.instance.isActive) PlayfabManager.instance.SetPlayerData(playerData);
                }

                break;
            case GameModeType.Hard:

                break;
            case GameModeType.Perfect:
                float clearScore = 0;
                bool fail = false;

                clearScore = ValueManager.instance.GetPerfectClearScore(GameStateManager.instance.GamePlayType);
                fail = GameStateManager.instance.Fail;

                if ((int)score > clearScore && !fail)
                {
                    Debug.Log("Get Trophy!");

                    if (!playerDataBase.GetTrophyIsAcive(GameStateManager.instance.GamePlayType))
                    {
                        trophyView.SetActive(true);

                        trophyIcon.sprite = iconArray[(int)GameStateManager.instance.GamePlayType];

                        TrophyData trophyContent = new TrophyData();

                        trophyContent.gamePlayType = GameStateManager.instance.GamePlayType;
                        trophyContent.isActive = true;
                        trophyContent.number = 1;
                        trophyContent.date = DateTime.Today.ToString("yyyy-MM-dd");

                        playerData.Clear();
                        playerData.Add(GameStateManager.instance.GamePlayType.ToString(), JsonUtility.ToJson(trophyContent));

                        playerDataBase.SetTrophyData(trophyContent);

                        if (PlayfabManager.instance.isActive) PlayfabManager.instance.SetPlayerData(playerData);

                        FirebaseAnalytics.LogEvent(GameStateManager.instance.GamePlayType.ToString(), "Trophy", 1);
                    }
                    else
                    {
                        TrophyData trophyContent = new TrophyData();

                        trophyContent = playerDataBase.GetTrophyData(GameStateManager.instance.GamePlayType);

                        trophyContent.number += 1;

                        playerData.Clear();
                        playerData.Add(GameStateManager.instance.GamePlayType.ToString(), JsonUtility.ToJson(trophyContent));

                        playerDataBase.SetTrophyData(trophyContent);

                        if (PlayfabManager.instance.isActive) PlayfabManager.instance.SetPlayerData(playerData);

                        FirebaseAnalytics.LogEvent(GameStateManager.instance.GamePlayType.ToString(), "Trophy", trophyContent.number);
                    }
                }
                break;
        }

        switch (GameStateManager.instance.GamePlayType)
        {
            case GamePlayType.GameChoice8:
                if (GameStateManager.instance.CoinRushTryCount > 0)
                {
                    GameStateManager.instance.CoinRushTryCount -= 1;

                    weeklyManager.UpdateWeeklyMissionReport(WeeklyMissionType.ChallengeCoinRush, 1);
                }
                break;
        }

        if (!GameStateManager.instance.WatchAd)
        {
            LoadWatchAd();
        }
        else
        {
            SetWatchAd(false);
        }

        DailyMissionReport report = new DailyMissionReport();
        report = playerDataBase.GetDailyMissionReport(GameStateManager.instance.GamePlayType);
        report.doPlay += 1;

        weeklyManager.UpdateWeeklyMissionReport(WeeklyMissionType.GamePlay, 1);

        if (report.getScore < (int)score)
        {
            report.getScore = (int)score;
        }
        
        if(report.getCombo < comboManager.GetCombo())
        {
            report.getCombo = comboManager.GetCombo();
        }

        playerDataBase.SetDailyMissionReport(report);

        scoreText.text = "";
        comboManager.OnStopCombo();

        newRecordObj.SetActive(false);
        newComboObj.SetActive(false);

        int bestScore = 0;
        int bestCombo = 0;
        int combo = comboManager.GetCombo();

        weeklyManager.UpdateWeeklyMissionReport(WeeklyMissionType.GetCombo, combo);

        switch (GameStateManager.instance.GamePlayType)
        {
            case GamePlayType.GameChoice1:
                bestScore = playerDataBase.BestSpeedTouchScore;
                bestCombo = playerDataBase.BestSpeedTouchCombo;
                break;
            case GamePlayType.GameChoice2:
                bestScore = playerDataBase.BestMoleCatchScore;
                bestCombo = playerDataBase.BestMoleCatchCombo;
                break;
            case GamePlayType.GameChoice3:
                bestScore = playerDataBase.BestFilpCardScore;
                bestCombo = playerDataBase.BestFilpCardCombo;
                break;
            case GamePlayType.GameChoice4:
                bestScore = playerDataBase.BestButtonActionScore;
                bestCombo = playerDataBase.BestButtonActionCombo;
                break;
            case GamePlayType.GameChoice5:
                bestScore = playerDataBase.BestTimingActionScore;
                bestCombo = playerDataBase.BestTimingActionCombo;
                break;
            case GamePlayType.GameChoice6:
                bestScore = playerDataBase.BestDragActionScore;
                bestCombo = playerDataBase.BestDragActionCombo;
                break;
            case GamePlayType.GameChoice7:
                bestScore = playerDataBase.BestLeftRightScore;
                bestCombo = playerDataBase.BestLeftRightCombo;
                break;
            case GamePlayType.GameChoice8:
                bestScore = playerDataBase.BestCoinRushScore;
                bestCombo = playerDataBase.BestCoinRushCombo;
                break;
        }

        if (Comparison((int)score, bestScore))
        {
            Debug.Log("Best Score !");
            newRecordObj.SetActive(true);

            switch (GameStateManager.instance.GamePlayType)
            {
                case GamePlayType.GameChoice1:
                    playerDataBase.BestSpeedTouchScore = (int)score;
                    if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdatePlayerStatisticsInsert("SpeedTouchScore", (int)score);
                    break;
                case GamePlayType.GameChoice2:
                    playerDataBase.BestMoleCatchScore = (int)score;
                    if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdatePlayerStatisticsInsert("MoleCatchScore", (int)score);
                    break;
                case GamePlayType.GameChoice3:
                    playerDataBase.BestFilpCardScore = (int)score;
                    if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdatePlayerStatisticsInsert("FilpCardScore", (int)score);
                    break;
                case GamePlayType.GameChoice4:
                    playerDataBase.BestButtonActionScore = (int)score;
                    if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdatePlayerStatisticsInsert("ButtonActionScore", (int)score);
                    break;
                case GamePlayType.GameChoice5:
                    playerDataBase.BestTimingActionScore = (int)score;
                    if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdatePlayerStatisticsInsert("TimingActionScore", (int)score);
                    break;
                case GamePlayType.GameChoice6:
                    playerDataBase.BestDragActionScore = (int)score;
                    if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdatePlayerStatisticsInsert("DragActionScore", (int)score);
                    break;
                case GamePlayType.GameChoice7:
                    playerDataBase.BestLeftRightScore = (int)score;
                    if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdatePlayerStatisticsInsert("LeftRightScore", (int)score);
                    break;
                case GamePlayType.GameChoice8:
                    playerDataBase.BestCoinRushScore = (int)score;
                    if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdatePlayerStatisticsInsert("CoinRushScore", (int)score);
                    break;
            }

            bestScore = (int)score;
        }
        else
        {
            newRecordObj.SetActive(false);
        }


        if(Comparison(combo, bestCombo))
        {
            Debug.Log("Best Combo !");

            newComboObj.SetActive(true);

            switch (GameStateManager.instance.GamePlayType)
            {
                case GamePlayType.GameChoice1:
                    playerDataBase.BestSpeedTouchCombo = combo;
                    if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdatePlayerStatisticsInsert("SpeedTouchCombo", combo);
                    break;
                case GamePlayType.GameChoice2:
                    playerDataBase.BestMoleCatchCombo = combo;
                    if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdatePlayerStatisticsInsert("MoleCatchCombo", combo);
                    break;
                case GamePlayType.GameChoice3:
                    playerDataBase.BestFilpCardCombo = combo;
                    if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdatePlayerStatisticsInsert("FilpCardCombo", combo);
                    break;
                case GamePlayType.GameChoice4:
                    playerDataBase.BestButtonActionCombo = combo;
                    if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdatePlayerStatisticsInsert("ButtonActionCombo", combo);
                    break;
                case GamePlayType.GameChoice5:
                    playerDataBase.BestTimingActionCombo = combo;
                    if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdatePlayerStatisticsInsert("TimingActionCombo", combo);
                    break;
                case GamePlayType.GameChoice6:
                    playerDataBase.BestDragActionCombo = combo;
                    if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdatePlayerStatisticsInsert("DragActionCombo", combo);
                    break;
                case GamePlayType.GameChoice7:
                    playerDataBase.BestLeftRightCombo = combo;
                    if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdatePlayerStatisticsInsert("LeftRightCombo", combo);
                    break;
                case GamePlayType.GameChoice8:
                    playerDataBase.BestCoinRushCombo = combo;
                    if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdatePlayerStatisticsInsert("CoinRushCombo", combo);
                    break;
            }

            bestCombo = combo;
        }

        UpdateTotalScore();
        UpdateTotalCombo();

        doubleCoinObj.SetActive(false);

        money = (int)(score * 2) + plusCoin;

        int level = playerDataBase.Level + 1;

        if (level > 50) level = 50;

        if (playerDataBase.CoinX2) level += 100;

        if (money > 0)
        {
            if (level > 0)
            {
                doubleCoinObj.SetActive(true);
                plusGoldText.text = "+ " + level + "%";

                plus = Mathf.Round((level + playerDataBase.AddGoldLevel) / 100.0f);
            }

            money = money + (money * plus);

            money = MathF.Round(money);

            if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdateAddCurrency(MoneyType.Coin, (int)(money));

            getGoldText.text = money.ToString();
        }
        else
        {
            getGoldText.text = "0";
        }

        exp = 0;

        if(score > 100)
        {
            if(score >= 1000)
            {
                exp += 200;
            }
            else
            {
                exp += 100;
            }
        }
        else
        {
            exp += 30;
        }
        
        exp += ((int)score / 10);
        exp += (combo / 5);

        doubleExpObj.SetActive(false);
        float expPlus = 0;

        expPlus = upgradeDataBase.GetValue(UpgradeType.AddExp, playerDataBase.AddExpLevel);

        if(GameStateManager.instance.Exp)
        {
            expPlus += 30;
        }

        if(playerDataBase.ExpX2)
        {
            expPlus += 100;
        }

        if(expPlus > 0)
        {
            doubleExpObj.SetActive(true);
            plusExpText.text = "+" + Mathf.Round(expPlus) + "%";
        }

        exp = exp + (exp * (expPlus /100.0f));

        exp = MathF.Round(exp);

        levelManager.CheckLevelUp(exp);

        goldAnimation.OnPlayExpAnimation();

        nowScoreText.text = ((int)score).ToString();
        nowComboText.text = combo.ToString();
        getExpText.text = ((int)exp).ToString();

        profileManager.Initialize();

        GameReset();
    }

    public void CloseTrophyView()
    {
        trophyView.SetActive(false);
    }

    public void CloseGameModeView()
    {
        gameModeView.SetActive(false);
    }

    #region WatchAd

    void SetWatchAd(bool check)
    {
        if(check)
        {
            Debug.Log("???? ????");

            watchAdLock.SetActive(true);
            GameStateManager.instance.WatchAd = false;

            PlayerPrefs.SetString("AdCoolTime",DateTime.Now.AddSeconds(ValueManager.instance.GetAdCoolTime()).ToString("yyyy-MM-dd HH:mm:ss"));

            adCoolTime = (int)ValueManager.instance.GetAdCoolTime();

            StartCoroutine(WatchAdCorution());
        }
        else
        {
            Debug.Log("???? ???? ????");

            watchAdLock.SetActive(false);
            GameStateManager.instance.WatchAd = true;

            StopAllCoroutines();
        }
    }

    void LoadWatchAd()
    {
        DateTime time = DateTime.Parse(PlayerPrefs.GetString("AdCoolTime"));
        DateTime now = DateTime.Now;

        TimeSpan span = time - now;

        if(span.TotalSeconds > 0)
        {
            adCoolTime = (int)span.TotalSeconds;

            watchAdLock.SetActive(true);

            StopAllCoroutines();
            StartCoroutine(WatchAdCorution());
        }
        else
        {
            SetWatchAd(false);
        }
    }

    IEnumerator WatchAdCorution()
    {
        if(adCoolTime > 0)
        {
            adCoolTime -= 1;
        }
        else
        {
            SetWatchAd(false);
            yield break;
        }

        watchAdCountText.text = (adCoolTime / 60).ToString("D2") + ":" + (adCoolTime % 60).ToString("D2");

        yield return waitForSeconds;
        StartCoroutine(WatchAdCorution());
    }

    public void SuccessWatchAd()
    {
        NotionManager.instance.UseNotion(NotionType.SuccessWatchAd);

        if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdateAddCurrency(MoneyType.Coin, (int)money);

        getGoldText.text = money + " + " + money.ToString();

        SetWatchAd(true);
    }

    #endregion

    void UpdateTotalScore()
    {
        int nowTotalScore = playerDataBase.BestSpeedTouchScore + playerDataBase.BestMoleCatchScore + playerDataBase.BestFilpCardScore + 
            playerDataBase.BestButtonActionScore + playerDataBase.BestTimingActionScore + playerDataBase.BestDragActionScore
            + playerDataBase.BestLeftRightScore;

        if (Comparison(nowTotalScore, playerDataBase.TotalScore))
        {
            Debug.Log("Best Total Score");
            if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdatePlayerStatisticsInsert("TotalScore", nowTotalScore);

            playerDataBase.TotalScore = nowTotalScore;
        }
    }

    void UpdateTotalCombo()
    {
        int nowTotalCombo = playerDataBase.BestSpeedTouchCombo + playerDataBase.BestMoleCatchCombo + playerDataBase.BestFilpCardCombo + 
            playerDataBase.BestButtonActionCombo + playerDataBase.BestTimingActionCombo + playerDataBase.BestDragActionCombo
            + playerDataBase.BestLeftRightCombo;

        if (Comparison(nowTotalCombo, playerDataBase.TotalCombo))
        {
            Debug.Log("Best Total Combo");
            if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdatePlayerStatisticsInsert("TotalCombo", nowTotalCombo);

            playerDataBase.TotalCombo = nowTotalCombo;
        }
    }

    public void OpenGameStop()
    {
        if(cancleWindowUI.activeSelf)
        {
            cancleWindowUI.SetActive(false);

            if(!gameOptionUI.activeInHierarchy)
            {
                Time.timeScale = 1;
            }
        }
        else
        {
            cancleWindowUI.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void GameStop()
    {
        Debug.Log("Game Stop");

        Time.timeScale = 1;
        GameStateManager.instance.DontStopGame = false;

        soundManager.PlayBGM(GameBGMType.Lobby);

        StopAllCoroutines();

        gameStartUI.SetActive(true);
        gameReadyUI.SetActive(false);
        cancleWindowUI.SetActive(false);

        timerText.text = "";
        timerText.color = new Color(1, 1, 0);
        scoreText.text = "";
        int number = comboManager.GetCombo();
        comboManager.OnStopCombo();

        CloseGamePlayUI();

        GameReset();
    }

    void GameReset()
    {
        GameStateManager.instance.PlayGame = false;

        timerText.text = "";
        score = 0;

        SetEtcUI(true);
        modeManager.OffMode();

        tutorialUI.SetActive(false);

        eGameEnd.Invoke();
    }

    public bool Comparison(float A, float B)
    {
        bool check = false;

        if(A > B)
        {
            check = true;
        }

        return check;
    }

    public void CloseGameEnd()
    {
        soundManager.PlayBGM(GameBGMType.Lobby);

        gameStartUI.SetActive(true);
        gameEndUI.SetActive(false);

        lockManager.Initialize();

        if (!GameStateManager.instance.InAppReview && playerDataBase.TotalScore >= 1000)
        {
            reviewManager.OpenReview();
        }
    }


    public void PlusScore(int index)
    {
        if (index == 0 || gameEndAnimView.activeInHierarchy) return;

        if (pause) pause = false;

        score += index;
        scoreText.text = LocalizationManager.instance.GetString("Score") + " : " + score.ToString();

        if(score > 100 && tutorialUI.activeInHierarchy)
        {
            tutorialUI.SetActive(false);
        }

        if (bestScore != 0)
        {
            if (score > bestScore)
            {
                scoreText.resizeTextMaxSize = 80;
                scoreText.color = new Color(1, 0, 0);
            }
            else
            {
                scoreText.resizeTextMaxSize = 60;
                scoreText.color = new Color(1, 1, 0);
            }
        }

        scoreNotion.gameObject.SetActive(false);
        scoreNotion.txt.color = new Color(1, 1, 0);
        scoreNotion.txt.text = "+" + index.ToString();
        scoreNotion.gameObject.SetActive(true);

        comboManager.OnStartCombo();
    }

    public void PlusCoin(int index)
    {
        plusCoin += index;

        coinNotion.gameObject.SetActive(false);
        coinNotion.txt.text = "+" + index.ToString();
        coinNotion.gameObject.SetActive(true);
    }

    public void MinusScore(int index)
    {
        if (index == 0 || gameEndAnimView.activeInHierarchy) return;

        if (GameStateManager.instance.Vibration)
        {
            Handheld.Vibrate();
        }

        soundManager.PlaySFX(GameSfxType.Fail);
        warningController.Hit();

        score = (score - index >= 0) ? score -= index : score = 0;
        scoreText.text = LocalizationManager.instance.GetString("Score") + " : " + score.ToString();

        if (bestScore != 0)
        {
            if (score < bestScore)
            {
                scoreText.resizeTextMaxSize = 60;
                scoreText.color = new Color(1, 1, 0);
            }
        }

        scoreNotion.gameObject.SetActive(false);
        scoreNotion.txt.color = new Color(1, 0, 0);

        if(index == 0)
        {
            scoreNotion.txt.text = "";
        }
        else
        {
            scoreNotion.txt.text = "-" + index.ToString();
        }

        scoreNotion.gameObject.SetActive(true);

        comboManager.OnStopCombo();
    }

    public void WaitNotionUI(GamePlayType type)
    {
        comboManager.WaitNotionUI(type);
    }

#region Coroution

    private IEnumerator ReadyTimerCoroution(float time)
    {
        float number = time;

        gameReadyUI.SetActive(true);

        while (number > 0)
        {
            if (!pause)
            {
                number -= 1;
                gameReadyText.text = number.ToString();
            }

            yield return waitForSeconds;
        }

        gameReadyUI.SetActive(false);
        //PlusScore(0);

        eGameStart.Invoke();

        float timer = ValueManager.instance.GetGamePlayTime();

        if(GameStateManager.instance.GameModeType != GameModeType.Perfect && GameStateManager.instance.GamePlayType != GamePlayType.GameChoice8)
        {
            //timer += upgradeDataBase.GetValue(UpgradeType.StartTime, playerDataBase.StartTimeLevel);
            timer += playerDataBase.StartTimeLevel * 0.1f;

            if (GameStateManager.instance.Clock)
            {
                timer += ValueManager.instance.GetClockAddTime();
            }

            if (GameStateManager.instance.Slow)
            {
                Time.timeScale = 0.9f;
            }
        }

        StartCoroutine("TimerCoroution", timer);
    }

    private IEnumerator TimerCoroution(float time)
    {
        float number = time;

        while(number > 0.1f)
        {
            if (!pause)
            {
                number -= 0.1f;
                timerText.text = number.ToString("N1");

                if (number < 11)
                {
                    timerText.color = new Color(225 / 255f, 34 / 255f, 12 / 255f);
                    soundManager.LowTimer();
                }
                else
                {
                    timerText.color = new Color(1, 1, 0);
                    soundManager.HighTimer();
                }
            }

            yield return waitForSeconds2;
        }

        timerText.text = "0";
        soundManager.HighTimer();

        //GameEnd();

        GameEndAnimation();
    }

    #endregion
}
