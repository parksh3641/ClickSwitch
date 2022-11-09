using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeftRightActionContent : MonoBehaviour
{
    public int index = 0;

    public Image icon;
    public Image backgroundImg;
    public Image focus;

    public Sprite[] iconArray;
    public Sprite[] backgroundImgArray;

    private void Awake()
    {
        focus.enabled = false;
    }

    private void OnDisable()
    {
        focus.enabled = false;
    }

    public void Initialize(int number)
    {
        index = number;

        icon.sprite = iconArray[number];
    }

    public void First()
    {
        focus.enabled = true;
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
