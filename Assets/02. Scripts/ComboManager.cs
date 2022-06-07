using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class ComboManager : MonoBehaviour
{
    GamePlayType gamePlayType = GamePlayType.GameChoice1;

    [Title("Main")]
    public GameObject comboObject;
    public Image fillamount;
    public Text comboText;
    public Notion notion;

    [Title("WaitUI")]
    public GameObject waitObject;
    public Text waitNotionText;
    public Image waitFillAmount;

    private int comboIndex = 0;

    private float timer = 0;
    private float comboTimer = 0;

    private float waitTimer = 0;
    private float waitSaveTimer = 0;

    bool pause = false;

    private void Awake()
    {
        comboText.text = "";
        fillamount.fillAmount = 0;

        comboIndex = 0;

        comboObject.SetActive(false);

        GameManager.eGamePause += GamePause;
    }

    private void Start()
    {
        waitObject.SetActive(false);

        comboTimer = ValueManager.instance.GetComboTimer();

        StartCoroutine(TimerCoroutine());
    }

    public void OnStartCombo()
    {
        Debug.Log("Combo Start!");

        comboObject.SetActive(true);

        if (timer == 0) comboIndex = 0;

        comboIndex += 1;

        timer = comboTimer;
        fillamount.fillAmount = 1;
        comboText.text = LocalizationManager.instance.GetString("Combo") + " : " + comboIndex.ToString();

        notion.gameObject.SetActive(false);
        notion.txt.text = "+" + 1.ToString();
        notion.gameObject.SetActive(true);
    }

    public void OnStopCombo()
    {
        comboObject.SetActive(false);

        timer = 0;
        fillamount.fillAmount = 0;
    }


    public int GetCombo()
    {
        comboText.text = "";

        return comboIndex;
    }

    void GamePause()
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

    public void WaitNotionUI(GamePlayType type)
    {
        gamePlayType = type;

        StartCoroutine(WaitNotionUICorution());
    }


    #region Corution
    IEnumerator TimerCoroutine()
    {
        if (!pause)
        {
            if (timer > 0)
            {
                timer -= 0.01f;

                fillamount.fillAmount = timer / comboTimer;
            }
            else
            {
                OnStopCombo();
            }
        }

        yield return new WaitForSeconds(0.01f);
        StartCoroutine(TimerCoroutine());
    }

    IEnumerator WaitNotionUICorution()
    {
        waitTimer = ValueManager.instance.GetCardTimer();
        waitSaveTimer =  waitTimer;

        waitFillAmount.fillAmount = 1;
        waitObject.SetActive(true);
        waitNotionText.text = LocalizationManager.instance.GetString("WaitNotion_" + gamePlayType);

        while (waitTimer > 0)
        {
            waitTimer -= 0.0167f;

            waitFillAmount.fillAmount = waitTimer / waitSaveTimer;

            yield return new WaitForSeconds(0.01f);
        }

        waitObject.SetActive(false);
    }


    #endregion
}
