using Sirenix.OdinInspector;
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


    [Title("CurrneyUI")]
    public GameObject virtualCurrencyUI;
    public Text goldText;
    public Text crystalText;

    public GameObject gameMenuUI;
    public GameObject[] gamePlayUI;

    [Title("ReadyUI")]
    public GameObject gameReadyUI;
    public Text gameReadyText;

    [Space]
    [Title("EndUI")]
    public GameObject gameEndUI;
    public GameObject newRecordObj;
    public Text nowScoreText;
    public Text bestScoreText;
    public Text nowComboText;
    public Text bestComboText;
    public Text getGoldText;
    public Text rankText;

    [Space]
    [Title("OptionUI")]
    public GameObject gameOptionUI;
    public GameObject languageUI;
    public GameObject[] etcUI;
    public GameObject cancleUI;

    [Space]
    [Title("LoginUI")]
    public GameObject loginUI;

    [Space]
    [Title("NotionUI")]
    public Notion scoreNotion;

    [Space]
    [Title("Value")]
    [SerializeField]
    private float score = 0;

    [Title("Bool")]
    [SerializeField]
    private bool pause = false;

    [Title("Manager")]
    public ComboManager comboManager;
    public RankingManager rankingManager;
    public CoinAnimation goldAnimation;
    public CoinAnimation crystalAnimation;

    [Title("DataBase")]
    public PlayerDataBase playerDataBase;

    WaitForSeconds waitForSeconds = new WaitForSeconds(1);


    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;

        GameManager.eGameStart += this.GameStart;
        GameManager.eGamePause += this.GamePause;
        GameManager.eGameEnd += this.GameEnd;

        GameManager.PlusScore += this.PlusScore;
        GameManager.MinusScore += this.MinusScore;

        timerText.text = "";
        scoreText.text = "";

        gameMenuUI.SetActive(false);
        gameOptionUI.SetActive(false);
        languageUI.SetActive(false);

        for (int i = 0; i < gamePlayUI.Length; i ++)
        {
            gamePlayUI[i].SetActive(false);
        }

        gameReadyUI.SetActive(false);
        gameEndUI.SetActive(false);
        cancleUI.SetActive(false);
    }

    private void Start()
    {
        loginUI.SetActive(!GameStateManager.instance.AutoLogin);

        VirtualCurrency();
    }

    private void OnApplicationQuit()
    {
        GameManager.eGameStart -= this.GameStart;
        GameManager.eGamePause -= this.GamePause;
        GameManager.eGameEnd -= this.GameEnd;

        GameManager.PlusScore -= this.PlusScore;
        GameManager.MinusScore -= this.MinusScore;
    }

    void VirtualCurrency()
    {
        goldText.text = playerDataBase.Gold.ToString();
        crystalText.text = playerDataBase.Crystal.ToString();
    }

    void AddVirtualCurrency(MoneyType type)
    {

    }

    void OnVirtualCurrency(bool check)
    {
        VirtualCurrency();

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
        gamePlayUI[(int)type].SetActive(true);
    }

    public void CloseGamePlayUI()
    {
        for(int i = 0; i < gamePlayUI.Length; i ++)
        {
            gamePlayUI[i].SetActive(false);
        }
    }

    public void OpenOption()
    {
        eGamePause.Invoke();

        if(gameOptionUI.activeSelf)
        {
            gameOptionUI.SetActive(false);
        }
        else
        {
            gameOptionUI.SetActive(true);
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
        }
    }

    public void OpenRanking()
    {
        rankingManager.OpenRanking();
    }

    public void OnLoginSuccess()
    {
        loginUI.SetActive(false);

        VirtualCurrency();
    }

    #endregion
    public void GameStart()
    {
        Debug.Log("Game Start");

        StartCoroutine("ReadyTimerCorution", ValueManager.instance.GetReadyTimer());

        OnVirtualCurrency(false);
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

    public void GameEnd()
    {
        Debug.Log("Game End");

        scoreText.text = "";
        comboManager.OnStopCombo();

        CloseGamePlayUI();
        gameEndUI.SetActive(true);

        int bestScore = 0;
        int bestCombo = 0;
        int combo = comboManager.GetCombo();

        switch (GameStateManager.instance.GamePlayType)
        {
            case GamePlayType.Normal:
                bestScore = playerDataBase.BestSpeedTouchScore;
                bestCombo = playerDataBase.BestSpeedTouchCombo;
                break;
            case GamePlayType.MoleCatch:
                bestScore = playerDataBase.BestMoleCatchScore;
                bestCombo = playerDataBase.BestMoleCatchCombo;
                break;
            case GamePlayType.FlipCard:
                bestScore = playerDataBase.BestFilpCardScore;
                bestCombo = playerDataBase.BestFilpCardCombo;
                break;
        }

        if(Comparison(score,bestScore))
        {
            Debug.Log("Best Score !");
            newRecordObj.SetActive(true);

            switch (GameStateManager.instance.GamePlayType)
            {
                case GamePlayType.Normal:
                    playerDataBase.BestSpeedTouchScore = (int)score;
                    if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdatePlayerStatisticsInsert("Score", (int)score);
                    break;
                case GamePlayType.MoleCatch:
                    playerDataBase.BestMoleCatchScore = (int)score;
                    if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdatePlayerStatisticsInsert("MoleCatchScore", (int)score);
                    break;
                case GamePlayType.FlipCard:
                    playerDataBase.BestFilpCardScore = (int)score;
                    if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdatePlayerStatisticsInsert("FilpCardScore", (int)score);
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

            switch (GameStateManager.instance.GamePlayType)
            {
                case GamePlayType.Normal:
                    playerDataBase.BestSpeedTouchCombo = combo;
                    if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdatePlayerStatisticsInsert("Combo", combo);
                    break;
                case GamePlayType.MoleCatch:
                    playerDataBase.BestMoleCatchCombo = combo;
                    if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdatePlayerStatisticsInsert("MoleCatchCombo", combo);
                    break;
                case GamePlayType.FlipCard:
                    playerDataBase.BestFilpCardCombo = combo;
                    if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdatePlayerStatisticsInsert("FilpCardCombo", combo);
                    break;
            }

            bestCombo = combo;
        }

        UpdateTotalScore();

        int money = (int)(score / 10);

        if(money > 0)
        {
            if (PlayfabManager.instance.isActive)
            {
                PlayfabManager.instance.UpdateAddCurrency(MoneyType.Gold, money);
            }
            playerDataBase.Gold += money;

            goldAnimation.OnPlay(playerDataBase.Gold, money);
        }

        nowScoreText.text = "SCORE" + "\n" + score.ToString();
        bestScoreText.text = "BEST" + "\n" + bestScore.ToString();

        nowComboText.text = "COMBO" + "\n" + combo.ToString();
        bestComboText.text = "BEST" + "\n" + bestCombo.ToString();

        getGoldText.text = "Coin" + "\n" + ((int)(score / 10)).ToString();
        rankText.text = "µî¼ö" + "\n" + "99 ¡æ 99";

        GameReset();
    }

    void UpdateTotalScore()
    {
        int nowTotalScore = playerDataBase.BestSpeedTouchScore + playerDataBase.BestMoleCatchScore + playerDataBase.BestFilpCardScore;

        if (Comparison(nowTotalScore, playerDataBase.TotalScore))
        {
            Debug.Log("Best Total Score");
            if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdatePlayerStatisticsInsert("TotalScore", nowTotalScore);
        }
    }

    public void GameStop()
    {
        Debug.Log("Game Stop");

        StopAllCoroutines();

        timerText.text = "";
        scoreText.text = "";
        int number = comboManager.GetCombo();
        comboManager.OnStopCombo();

        eGameEnd.Invoke();

        CloseGamePlayUI();

        GameReset();
    }

    void GameReset()
    {
        score = 0;

        OnVirtualCurrency(true);

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
        gameEndUI.SetActive(false);
    }


    public void PlusScore(int index)
    {
        score += index;
        scoreText.text = "SCORE : " + score.ToString();

        scoreNotion.gameObject.SetActive(false);
        scoreNotion.txt.color = new Color(0, 1, 0);
        scoreNotion.txt.text = "+" + index.ToString();
        scoreNotion.gameObject.SetActive(true);

        comboManager.OnStartCombo();
    }

    public void MinusScore(int index)
    {
        score = (score - index >= 0) ? score -= index : score = 0;
        scoreText.text = "SCORE : " + score.ToString();

        scoreNotion.gameObject.SetActive(false);
        scoreNotion.txt.color = new Color(1, 0, 0);
        scoreNotion.txt.text = "-" + index.ToString();
        scoreNotion.gameObject.SetActive(true);

        comboManager.OnStopCombo();
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

        StartCoroutine("TimerCorution", ValueManager.instance.GetTimer());
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
            }

            yield return waitForSeconds;
        }

        timerText.text = "";

        GameEnd();
    }

    #endregion
}
