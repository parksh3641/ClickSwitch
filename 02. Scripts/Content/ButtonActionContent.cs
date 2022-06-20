using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonActionContent : MonoBehaviour
{
    public int index = 0;

    public Text mainText;

    public string[] strArray;

    public Image backgroundImg;
    public Sprite[] backgroundImgList;

    public void Initialize(int number)
    {
        index = number;

        mainText.text = strArray[number];

        backgroundImg.sprite = backgroundImgList[0];
    }

    public void OnCheck()
    {
        backgroundImg.sprite = backgroundImgList[1];
    }

    public int GetIndex()
    {
        return index;
    }
}
