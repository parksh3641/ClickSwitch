using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NormalContent : MonoBehaviour, IContentEvent
{
    [SerializeField]
    private bool isActive = false;


    public UnityEvent clickSoundEvent;
    public UnityEvent clickEvent;

    public Image backgroundImg;
    public Sprite[] backgroundImgList;
    public int index = 0;
    public Text numberText;


    void Awake()
    {
        clickSoundEvent.AddListener(() => { GameObject.FindWithTag("ClickSound").GetComponent<AudioSource>().Play(); });
        clickEvent.AddListener(() => { GameObject.FindWithTag("GameManager").GetComponent<GameManager>().CheckNumber(index, ChoiceAction); });
    }

    public void Reset(int number)
    {
        index = number;
        numberText.text = index.ToString();

        backgroundImg.sprite = backgroundImgList[0];
        backgroundImg.enabled = true;

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
    }
}
