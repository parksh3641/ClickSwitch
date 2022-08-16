using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemContent : MonoBehaviour
{
    ShopClass shopClass;

    public LocalizationContent titleText;

    public Image icon;
    public Text countText;

    public Image vcIcon;
    public Text priceText;

    bool delay = false;

    Sprite[] vcArray;

    ShopManager shopManager;
    ImageDataBase imageDataBase;

    void Awake()
    {
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;
    }

    public void Initialize(ShopManager manager, ShopClass shop, Sprite sprite)
    {
        vcArray = imageDataBase.GetVCArray();

        titleText.name = "Item" + shop.itemId;
        titleText.ReLoad();

        shopManager = manager;
        shopClass = shop;
        icon.sprite = sprite;
        countText.text = "";

        switch (shop.virtualCurrency)
        {
            case "GO":
                vcIcon.sprite = vcArray[0];
                break;
            case "ST":
                vcIcon.sprite = vcArray[1];
                break;
        }

        priceText.text = shop.price.ToString();
    }

    public void InitializeETC(ShopManager manager, ShopClass shop, Sprite sprite)
    {
        vcArray = imageDataBase.GetVCArray();

        titleText.name = shop.itemId;
        titleText.ReLoad();

        shopManager = manager;
        shopClass = shop;
        icon.sprite = sprite;
        countText.text = "";

        switch (shop.virtualCurrency)
        {
            case "GO":
                vcIcon.sprite = vcArray[0];
                break;
            case "ST":
                vcIcon.sprite = vcArray[1];
                break;
        }

        priceText.text = shop.price.ToString();
    }

    public void OnClick()
    {
        if(!delay)
        {
            //shopManager.OnBuy(shopClass);
            shopManager.OpenBuyWindow(shopClass, icon.sprite);

            delay = true;
            StartCoroutine(DelayCourtion());
        }
    }


    IEnumerator DelayCourtion()
    {
        yield return new WaitForSeconds(0.5f);
        delay = false;
    }
}
