using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopContent : MonoBehaviour
{
    public ShopType shopType = ShopType.RemoveAds;

    public Image mainIcon;
    public LocalizationContent titleText;
    public Text priceText;

    public GameObject price;
    public GameObject reward;

    Sprite[] mainIconArray;

    ImageDataBase imageDataBase;

    private void Awake()
    {
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDatabase") as ImageDataBase;

        mainIconArray = imageDataBase.GetShopArray();

        price.SetActive(false);
        reward.SetActive(false);
    }

    void Start()
    {
        mainIcon.sprite = mainIconArray[(int)shopType];

        switch (shopType)
        {
            case ShopType.RemoveAds:
                titleText.name = "RemoveAds";

                switch (GameStateManager.instance.Language)
                {
                    case LanguageType.Korean:
                        priceText.text = "₩ 2500";
                        break;
                    default:
                        priceText.text = "USD $ 2";
                        break;
                }

                price.SetActive(true);

                break;
            case ShopType.Coin1000:
                titleText.name = "Coin1000";

                switch (GameStateManager.instance.Language)
                {
                    case LanguageType.Korean:
                        priceText.text = "₩ 1200";
                        break;
                    default:
                        priceText.text = "USD $ 1";
                        break;
                }

                price.SetActive(true);

                break;
            case ShopType.WatchAd:
                titleText.name = "WatchAd";

                reward.SetActive(true);

                break;
        }

        titleText.ReLoad();
    }
}
