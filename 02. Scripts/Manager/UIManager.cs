using Firebase.Analytics;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, IGameEvent
{
    [Title("Event")]
    public UnityEvent eGameStart;
    public UnityEvent eGamePause;
    public UnityEvent eGameEnd;

    public Text timerText;
    public Text scoreText;

    public LocalizationContent infoBestScoreText;
    public LocalizationContent infoBestComboText;

    [Space]
    [Title("CurrneyUI")]
    public GameObject virtualCurrencyUI;
    public Text goldText;
    public Text crystalText;

    public GameObject gameMenuUI;
    public GameObject gamePlayView;
    public GameObject[] gamePlayUI;

    [Space]
    [Title("ReadyUI")]
    public GameObject gameReadyUI;
    public Text gameReadyText;

    [Title("ItemUI")]
    public ItemUseContent[] itemUseContentArray;

    [Space]
    [Title("EndUI")]
    public GameObject gameEndUI;
    public GameObject newRecordObj;
    public GameObject newComboObj;
    public GameObject doubleCoinObj;
    public Text nowScoreText;
    public Text bestScoreText;
    public Text nowComboText;
    public Text bestComboText;
    public Text getGoldText;
    public Text rankText;

    [Space]
    [Title("Ad")]
    public GameObject watchAdButton;
    public GameObject watchAdLock;
    public Text watchAdCountText;


    [Space]
    [Title("OptionUI")]
    public GameObject gameOptionUI;
    public GameObject languageUI;
    public GameObject[] etcUI;
    public LanguageContent[] languageContentArray;

    [Space]
    [Title("CancleUI")]
    public GameObject cancleWindowUI;
    public GameObject cancleUI;

    [Space]
    [Title("LoginUI")]
    public FadeInOut loadingUI;
    public GameObject loginUI;
    public GameObject[] loginButtonList;
    public Text platformText;
    public Text versionText;

    public GameObject updateUI;


    [Space]
    [Title("NotionUI")]
    public Notion scoreNotion;

    [Space]
    [Title("Value")]
    private float score = 0;
    private int bestScore = 0;
    private int bestCombo = 0;
    private int adCoolTime = 0;

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
    public GoogleAdsManager googldAdsManager;
    public ShopManager shopManager;
    public AchievementManager achievementManager;

    [Title("Animation")]
    public CoinAnimation goldAnimation;
    public CoinAnimation crystalAnimation;

    [Title("DataBase")]
    public PlayerDataBase playerDataBase;
    ImageDataBase imageDataBase;

    WaitForSeconds waitForSeconds = new WaitForSeconds(1);


    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;

        updateUI.SetActive(false);

        GameManager.eGameStart += this.GameStart;
        GameManager.eGamePause += this.GamePause;

        GameManager.PlusScore += this.PlusScore;
        GameManager.MinusScore += this.MinusScore;

        timerText.text = "";
        timerText.color = new Color(1, 1, 0);
        scoreText.text = "";

        infoBestScoreText.gameObject.SetActive(false);
        infoBestComboText.gameObject.SetActive(false);

        goldText.text = "0";
        crystalText.text = "0";

        gameMenuUI.SetActive(false);
        gameOptionUI.SetActive(false);
        languageUI.SetActive(false);

        gamePlayView.SetActive(false);

        for (int i = 0; i < gamePlayUI.Length; i ++)
        {
            gamePlayUI[i].SetActive(false);
        }

        gameReadyUI.SetActive(false);
        gameEndUI.SetActive(false);
        cancleWindowUI.SetActive(false);
        cancleUI.SetActive(false);
    }

    private void Start()
    {
        loadingUI.gameObject.SetActive(true);

        loginUI.SetActive(!GameStateManager.instance.AutoLogin);

        SetLoginUI();
    }

    public void SetLoginUI()
    {
        versionText.text = "v" + Application.version;

        platformText.text = LocalizationManager.instance.GetString("Platform");

        for (int i = 0; i < loginButtonList.Length; i++)
        {
            loginButtonList[i].SetActive(false);
        }

#if UNITY_EDITOR
        platformText.text += " : " + "Editor";
        loginButtonList[0].SetActive(true);
#elif UNITY_WEBGL
        platformText.text += " : " + "WebGL";
        loginButtonList[0].SetActive(true);
#elif UNITY_ANDROID
        platformText.text += " : " + "Android";
        loginButtonList[1].SetActive(true);
#elif UNITY_IOS
        platformText.text += " : " + "IOS";
        loginButtonList[2].SetActive(true);
#endif
    }


    private void OnApplicationQuit()
    {
        GameManager.eGameStart -= this.GameStart;
        GameManager.eGamePause -= this.GamePause;

        GameManager.PlusScore -= this.PlusScore;
        GameManager.MinusScore -= this.MinusScore;
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {

        }
        else
        {
            if(cancleUI.activeSelf)
            {
                OpenGameStop();
            }
        }
    }

    public void RenewalVC()
    {
        Debug.Log("화폐 갱신");

        goldText.text = playerDataBase.Coin.ToString();
        crystalText.text = playerDataBase.Crystal.ToString();
    }

    public void SetItem()
    {
        Sprite[] itemArray = imageDataBase.GetItemArray();

        for(int i = 0; i < itemUseContentArray.Length; i ++)
        {
            itemUseContentArray[i].Initialize(itemArray[i]);
        }

        if (GameStateManager.instance.Clock) itemUseContentArray[0].UseItem();
        if (GameStateManager.instance.Shield) itemUseContentArray[1].UseItem();
    }

    public void UsedItem(ItemType type)
    {
        itemUseContentArray[(int)type].UsedItem();
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
        gameMenuUI.SetActive(true);
    }

    public void CloseMenu()
    {
        gameMenuUI.SetActive(false);
    }

    public void OpenGamePlayUI(GamePlayType type)
    {
        gamePlayView.SetActive(true);

        gamePlayUI[(int)type].SetActive(true);

        SetItem();

        infoBestScoreText.gameObject.SetActive(true);
        infoBestComboText.gameObject.SetActive(true);

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
                bestScore = playerDataBase.BestFingerSnapScore;
                bestCombo = playerDataBase.BestFingerSnapCombo;
                break;
            case GamePlayType.GameChoice7:
                break;
            case GamePlayType.GameChoice8:
                break;
        }

        comboManager.SetBestCombo(bestCombo);

        infoBestScoreText.SetNumber(bestScore);
        infoBestComboText.SetNumber(bestCombo);
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
            Time.timeScale = 0;
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

            for (int i = 0; i < languageContentArray.Length; i++)
            {
                languageContentArray[i].OnFrame(false);
            }

            languageContentArray[(int)GameStateManager.instance.Language -1].OnFrame(true);
        }
    }

    public void OpenRanking()
    {
        rankingManager.OpenRanking();
    }

    public void OpenProfile()
    {
        profileManager.OpenProfile();
    }

    public void OpenNickName()
    {
        nickNameManager.OpenNickName();
    }

    public void OpenShop()
    {
        shopManager.OpenShop();
    }

    public void OpenAchievement()
    {
        achievementManager.OpenAchievement();
    }

    public void OnLoginSuccess()
    {
        loadingUI.FadeIn();

        loginUI.SetActive(false);

        RenewalVC();
    }

    public void OnLogout()
    {
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
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.unity3d.toucharcade");
#endif
    }

    #endregion
    public void GameStart()
    {
        Debug.Log("Game Start");

        pause = false;

        StartCoroutine("ReadyTimerCorution", ValueManager.instance.GetReadyTime());

        SetEtcUI(false);
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

    int money = 0;

    public void GameEnd()
    {
        Debug.Log("Game End");

        FirebaseAnalytics.LogEvent(GameStateManager.instance.GamePlayType.ToString(),"GetScore", score);
        FirebaseAnalytics.LogEvent(GameStateManager.instance.GamePlayType.ToString(), "GetCombo", comboManager.GetCombo());

        soundManager.PlayBGM(GameBGMType.End);

        scoreText.text = "";
        comboManager.OnStopCombo();

        CloseGamePlayUI();
        gameEndUI.SetActive(true);

        newRecordObj.SetActive(false);
        newComboObj.SetActive(false);

        int bestScore = 0;
        int bestCombo = 0;
        int combo = comboManager.GetCombo();

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
                bestScore = playerDataBase.BestFingerSnapScore;
                bestCombo = playerDataBase.BestFingerSnapCombo;
                break;
        }

        if(Comparison(score,bestScore))
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
                    playerDataBase.BestFingerSnapScore = (int)score;
                    if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdatePlayerStatisticsInsert("FingerSnapScore", (int)score);
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
                    playerDataBase.BestFingerSnapCombo = combo;
                    if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdatePlayerStatisticsInsert("FingerSnapCombo", combo);
                    break;
            }

            bestCombo = combo;
        }

        UpdateTotalScore();
        UpdateTotalCombo();


        money = (int)(score / 10);

        if(money > 0)
        {
            if (playerDataBase.RemoveAd)
            {
                doubleCoinObj.SetActive(true);
                watchAdButton.SetActive(false);

                if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdateAddCurrency(MoneyType.Coin, money * 2);

                getGoldText.text = money + " + " + money.ToString();
            }
            else
            {
                doubleCoinObj.SetActive(false);
                watchAdButton.SetActive(true);

                if (!GameStateManager.instance.WatchAd)
                {
                    LoadWatchAd();
                }
                else
                {
                    SetWatchAd(false);
                }

                if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdateAddCurrency(MoneyType.Coin, money);

                getGoldText.text = money.ToString();
            }
        }
        else
        {
            getGoldText.text = "0";
        }

        nowScoreText.text = score.ToString();
        nowComboText.text = combo.ToString();

        GameReset();
    }

    void SetWatchAd(bool check)
    {
        if(check)
        {
            Debug.Log("광고 잠금");

            watchAdLock.SetActive(true);
            GameStateManager.instance.WatchAd = false;

            PlayerPrefs.SetString("AdCoolTime",DateTime.Now.AddSeconds(ValueManager.instance.GetAdCoolTime()).ToString("yyyy-MM-dd HH:mm:ss"));

            adCoolTime = (int)ValueManager.instance.GetAdCoolTime();

            StartCoroutine(WatchAdCorution());
        }
        else
        {
            Debug.Log("광고 잠금 해제");

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

        yield return new WaitForSeconds(1f);
        StartCoroutine(WatchAdCorution());
    }

    public void SuccessWatchAd()
    {
        Debug.Log("광고 보상 : 코인 2배 지급!");

        if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdateAddCurrency(MoneyType.Coin, money);

        getGoldText.text = money + " + " + money.ToString();

        SetWatchAd(true);

    }

    void UpdateTotalScore()
    {
        int nowTotalScore = playerDataBase.BestSpeedTouchScore + playerDataBase.BestMoleCatchScore + playerDataBase.BestFilpCardScore + 
            playerDataBase.BestButtonActionScore + playerDataBase.BestTimingActionScore + playerDataBase.BestFingerSnapScore;

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
            playerDataBase.BestButtonActionCombo + playerDataBase.BestTimingActionCombo + playerDataBase.BestFingerSnapCombo;

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

        soundManager.PlayBGM(GameBGMType.Lobby);

        StopAllCoroutines();

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
        score = 0;

        SetEtcUI(true);

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

        gameEndUI.SetActive(false);
    }


    public void PlusScore(int index)
    {
        score += index;
        scoreText.text = LocalizationManager.instance.GetString("Score") + " : " + score.ToString();

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

    public void MinusScore(int index)
    {
        if(GameStateManager.instance.Vibration)
        {
            Handheld.Vibrate();
        }

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
        scoreNotion.txt.text = "-" + index.ToString();
        scoreNotion.gameObject.SetActive(true);

        comboManager.OnStopCombo();
    }

    public void WaitNotionUI(GamePlayType type)
    {
        comboManager.WaitNotionUI(type);
    }

#region Corution

    private IEnumerator ReadyTimerCorution(float time)
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

        if (GameStateManager.instance.Clock) timer += ValueManager.instance.GetClockTime();

        StartCoroutine("TimerCorution", timer);
    }

    private IEnumerator TimerCorution(float time)
    {
        float number = time;

        while(number > 0)
        {
            if (!pause)
            {
                number -= 1;
                timerText.text = number.ToString();

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

            yield return waitForSeconds;
        }

        timerText.text = "";
        soundManager.HighTimer();

        GameEnd();
    }

    #endregion
}
