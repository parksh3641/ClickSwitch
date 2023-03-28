using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WelcomeContent : MonoBehaviour
{
    public int index = 0;
    public int maxScore = 500;

    public LocalizationContent titleText;
    public Text infoText;

    public Image icon;

    public ReceiveContent receiveContent;

    public GameObject foucsObj;
    public GameObject lockObj;
    public GameObject clearObj;

    public EventManager eventManager;

    Sprite[] iconArray;

    ImageDataBase imageDataBase;

    private void Awake()
    {
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;

        iconArray = imageDataBase.GetIconArray();
    }

    public void Initialize(int number, bool check, int score, EventManager manager)
    {
        eventManager = manager;

        titleText.localizationName = LocalizationManager.instance.GetString("Welcome" + (index + 1));
        titleText.ReLoad();

        infoText.text = LocalizationManager.instance.GetString("BestScore");

        if(score >= maxScore)
        {
            score = maxScore;
        }

        infoText.text += "\n" + score + " / " + maxScore; 

        icon.sprite = iconArray[index];

        foucsObj.SetActive(false);
        clearObj.SetActive(false);

        if (index == number)
        {
            if(!check)
            {
                foucsObj.SetActive(true);
            }

            if (!check && score >= maxScore)
            {
                lockObj.SetActive(false);
            }
            else
            {
                lockObj.SetActive(true);
            }
        }
        else
        {
            if (index > number)
            {
                lockObj.SetActive(true);
            }
            else
            {
                foucsObj.SetActive(false);
                clearObj.SetActive(true);
            }
        }
    }

    public void ReceiveButton()
    {
        eventManager.WelcomeReceiveButton(index, SuccessReceive);
    }

    public void SuccessReceive()
    {
        foucsObj.SetActive(false);
        clearObj.SetActive(true);
    }
}
