using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldScoreContent : MonoBehaviour
{
    public int index = 0;

    public LocalizationContent titleText;

    public ReceiveContent receiveContent;

    public GameObject clearObj;

    public EventManager eventManager;

    public void Initialize(bool check, EventManager manager)
    {
        clearObj.SetActive(check);
        eventManager = manager;

        titleText.localizationName = "WorldScoreProgress" + (index + 1);
        titleText.ReLoad();
    }

    public void ReceiveButton()
    {
        eventManager.WorldScoreReceiveButton(index, SuccessReceive);
    }

    public void SuccessReceive()
    {
        clearObj.SetActive(true);
    }
}
