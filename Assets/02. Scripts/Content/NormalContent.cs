using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NormalContent : MonoBehaviour, IContentEvent
{
    GamePlayType gamePlayType = GamePlayType.GameChoice1;

    [SerializeField]
    private bool isActive = false;


    public UnityEvent clickEvent;
    public UnityEvent clickSoundEvent;
    public UnityEvent failSoundEvent;

    public Image backgroundImg;
    public Sprite[] backgroundImgList;
    public int index = 0;
    public Text numberText;

    [Title("FilpCard")]
    public Image filpCardImg;
    public Sprite[] filpCardImgList;

    void Awake()
    {
        clickSoundEvent.AddListener(() => { GameObject.FindWithTag("ClickSound").GetComponent<AudioSource>().Play(); });
        failSoundEvent.AddListener(() => { GameObject.FindWithTag("FailSound").GetComponent<AudioSource>().Play(); });

        numberText.text = "";

        filpCardImg.enabled = false;
    }

    public void Initialize(GamePlayType type)
    {
        gamePlayType = type;

        clickEvent.RemoveAllListeners();

        switch (type)
        {
            case GamePlayType.GameChoice1:
                clickEvent.AddListener(() => { GameObject.FindWithTag("GameManager").GetComponent<GameManager>().CheckNumber(index, ChoiceAction); });
                break;
            case GamePlayType.GameChoice2:
                clickEvent.AddListener(() => { GameObject.FindWithTag("GameManager").GetComponent<GameManager>().CheckMole(index, ChoiceMoleAction); });
                break;
            case GamePlayType.GameChoice3:
                clickEvent.AddListener(() => { GameObject.FindWithTag("GameManager").GetComponent<GameManager>().CheckFilpCard(index, isActive, ChoiceCardAction); });
                break;
        }
    }

    public void OnReset()
    {
        backgroundImg.sprite = backgroundImgList[0];

        isActive = true;
    }

    public void NormalReset(int number)
    {
        index = number;
        numberText.text = index.ToString();

        backgroundImg.sprite = backgroundImgList[0];
        backgroundImg.enabled = true;

        isActive = true;
    }

    public void FilpCardReset(int number)
    {
        filpCardImg.sprite = filpCardImgList[number];
        filpCardImg.enabled = false;
        index = number;

        isActive = true;
    }

    public void First()
    {
        backgroundImg.sprite = backgroundImgList[1];
    }


    public void Choice()
    {
        clickSoundEvent.Invoke();

        if(isActive)
        {
            clickEvent.Invoke();
        }
    }

    public void ChoiceAction(bool check)
    {
        if(check)
        {
            isActive = false;

            backgroundImg.enabled = false;
            numberText.text = "";
        }
        else
        {
            failSoundEvent.Invoke();
        }
    }

    public void ChoiceMoleAction(bool check)
    {
        if (check)
        {
            isActive = false;

            backgroundImg.sprite = backgroundImgList[0];
        }
        else
        {
            failSoundEvent.Invoke();
        }
    }

    public void ChoiceCardAction(int check)
    {
        switch(check)
        {
            case 0:
                isActive = false;

                filpCardImg.enabled = true;
                break;
            case 1:
                isActive = false;

                filpCardImg.enabled = true;

                break;
            case 2:
                isActive = true;

                filpCardImg.enabled = false;

                failSoundEvent.Invoke();
                break;
        }
    }

    public int GetIndex()
    {
        return index;
    }
}
