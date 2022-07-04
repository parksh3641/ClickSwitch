using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpContent : MonoBehaviour
{
    public Image icon;
    public LocalizationContent text;


    ImageDataBase imageDataBase;

    Sprite[] iconArray;
    Sprite[] itemArray;

    private void Awake()
    {
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;

        iconArray = imageDataBase.GetIconArray();
        itemArray = imageDataBase.GetItemArray();
    }


    public void InitializeGame(GamePlayType type)
    {
        icon.sprite = iconArray[(int)type];

        text.name = "Information_" + type.ToString();
        text.ReLoad();
    }

    public void InitalizeItem(ItemType type)
    {
        icon.sprite = itemArray[(int)type];

        text.name = "Information_" + type.ToString();
        text.ReLoad();
    }
}
