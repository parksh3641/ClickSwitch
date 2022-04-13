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

    private float value = 0;
    [SerializeField]
    private float timer = 0;
    private float comboTimer = 0;


    private void Awake()
    {
        comboText.text = "";
        fillamount.fillAmount = 0;

        value = 0;
    }

    private void Start()
    {
        comboTimer = ValueManager.instance.GetComboTimer();

        StartCoroutine(TimerCoroutine());
    }

    public void OnStartCombo()
    {
        Debug.Log("Combo");
        value += 1;

        timer = comboTimer;
        fillamount.fillAmount = 1;
        comboText.text = "Combo : " + value.ToString();

        notion.gameObject.SetActive(false);
        notion.txt.text = "+" + 1.ToString();
        notion.gameObject.SetActive(true);
    }

    public void OnStopCombo()
    {
        value = 0;

        timer = 0;
        fillamount.fillAmount = 0;

        comboText.text = "";
    }

    IEnumerator TimerCoroutine()
    {
        if(timer > 0)
        {
            timer -= 0.01f;

            fillamount.fillAmount = timer / comboTimer;
        }
        else
        {
            OnStopCombo();
        }

        yield return new WaitForSeconds(0.01f);
        StartCoroutine(TimerCoroutine());
    }
}
