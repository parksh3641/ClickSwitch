using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopContent : MonoBehaviour
{
    public ShopType shopType = ShopType.RemoveAds;

    public Image mainIcon;
    public LocalizationContent TitleText;
    public Text priceText;

    Sprite[] mainIconArray;

    ImageDataBase imageDataBase;

    private void Awake()
    {
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDatabase") as ImageDataBase;

        mainIconArray = imageDataBase.GetShopArray();
    }

    void Start()
    {
        mainIcon.sprite = mainIconArray[(int)shopType];

        switch (shopType)
        {
            case ShopType.RemoveAds:
                TitleText.name = "RemoveAds";
                break;
            case ShopType.Coin1000:
                TitleText.name = "Coin1000";
                break;
        }

        TitleText.ReLoad();

        switch (GameStateManager.instance.Language)
        {
            case LanguageType.Korean:
                priceText.text = "₩ 1200";
                break;
            case LanguageType.English:
                priceText.text = "USD $ 1";
                break;
            case LanguageType.Japenese:
                priceText.text = "120 円";
                break;
            default:


                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
