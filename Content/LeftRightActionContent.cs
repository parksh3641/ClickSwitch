using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeftRightActionContent : MonoBehaviour
{
    public int index = 0;

    public Image icon;
    public Image backgroundImg;

    public Sprite[] iconArray;
    public Sprite[] backgroundImgArray;

    public void Initialize(int number)
    {
        index = number;

        icon.sprite = iconArray[number];
    }

    public void RandomBackground()
    {
        int random = Random.Range(0, backgroundImgArray.Length - 1);

        backgroundImg.sprite = backgroundImgArray[random];
    }

    public int GetIndex()
    {
        return index;
    }
}
