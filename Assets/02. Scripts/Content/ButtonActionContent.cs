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

    public void Initialize(int number)
    {
        index = number;

        mainText.text = strArray[number];
    }

    public int GetIndex()
    {
        return index;
    }
}
