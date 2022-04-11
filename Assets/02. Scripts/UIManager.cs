using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, IGameEvent
{
    [Title("Event")]
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
    public Text newRecordText;
    public Text nowScoreText;
    public Text bestScoreText;
    public Text rankText;

    [Space]
    [Title("OptionUI")]
    public GameObject gameOptionUI;
    public GameObject languageUI;

    [Space]
    [Title("LoginUI")]
    public GameObject loginUI;

    [Space]
    [Title("Value")]
    [SerializeField]
    private float score = 0;

    [Title("Bool")]
    [SerializeField]
    private bool pause = false;

    [Title("Corution")]
    public IEnumerator readyTimerCorution;
    public IEnumerator timerCoroutine;

    [Title("DataBase")]
    public PlayerDataBase playerDataBase;

    WaitForSeconds waitForSeconds = new WaitForSeconds(1);


    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;

        GameManager.GameStart += this.GameStart;
        GameManager.GamePause += this.GamePause;
        GameManager.GameEnd += this.GameEnd;

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

    }

    private void Start()
    {
        readyTimerCorution = ReadyTimerCorution(ValueManager.instance.GetReadyTimer());
        timerCoroutine = TimerCorution(ValueManager.instance.GetTimer());

        loginUI.SetActive(!GameStateManager.instance.AutoLogin);

        VirtualCurrency();
    }

    private void OnApplicationQuit()
    {
        GameManager.GameStart -= this.GameStart;
        GameManager.GamePause -= this.GamePause;
        GameManager.GameEnd -= this.GameEnd;

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
        virtualCurrencyUI.SetActive(check);
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

    public void OnLoginSuccess()
    {
        loginUI.SetActive(false);

        VirtualCurrency();
    }

    #endregion
    public void GameStart()
    {
        StartCoroutine(readyTimerCorution);

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
        StopCoroutine(timerCoroutine);

        OnVirtualCurrency(true);
    }

    public void PlusScore(int index)
    {
        score += index;
        scoreText.text = "SCORE : " + score.ToString();
    }

    public void MinusScore(int index)
    {
        score = (score - index >= 0) ? score -= index : score = 0;
        scoreText.text = "SCORE : " + score.ToString();
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
        PlusScore(0);

        StartCoroutine(timerCoroutine);
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
    }

    #endregion
}
