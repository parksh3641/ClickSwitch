using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class ComboManager : MonoBehaviour
{
    public Image fillamount;

    public Text comboText;

    public Notion notion;

    private int comboIndex = 0;
    [SerializeField]
    private float timer = 0;
    private float comboTimer = 0;

    bool pause = false;

    private void Awake()
    {
        comboText.text = "";
        fillamount.fillAmount = 0;

        comboIndex = 0;

        GameManager.eGamePause += GamePause;
    }

    private void Start()
    {
        comboTimer = ValueManager.instance.GetComboTimer();

        StartCoroutine(TimerCoroutine());
    }

    public void OnStartCombo()
    {
        Debug.Log("Combo");

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
        timer = 0;
        fillamount.fillAmount = 0;
    }

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
}
