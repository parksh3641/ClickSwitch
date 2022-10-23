using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopContent : MonoBehaviour
{
    public ShopType shopType = ShopType.RemoveAds;

    public Image mainIcon;
    public LocalizationContent titleText;

    [Space]
    [Title("Obj")]
    public GameObject price;
    public GameObject coin;
    public GameObject crystal;

    [Space]
    [Title("Text")]
    public Text priceText;
    public Text coinText;
    public Text crystalText;

    Sprite[] mainIconArray;

    ImageDataBase imageDataBase;

    private void Awake()
    {
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDatabase") as ImageDataBase;

        mainIconArray = imageDataBase.GetShopArray();

        price.SetActive(false);
        coin.SetActive(false);
        crystal.SetActive(false);
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
                        priceText.text = "₩ 1200";
                        break;
                    default:
                        priceText.text = "USD $ 0.99";
                        break;
                }

                price.SetActive(true);

                break;
            case ShopType.WatchAd:
                titleText.name = "WatchAd";

                coinText.text = "400";

                coin.SetActive(true);

                break;
            case ShopType.Coin1000:
                titleText.name = "Coin1000";

                crystalText.text = "60";

                crystal.SetActive(true);
                break;
            case ShopType.Coin2000:
                titleText.name = "Coin2000";

                crystalText.text = "500";

                crystal.SetActive(true);
                break;
            case ShopType.Coin3000:
                titleText.name = "Coin3000";

                crystalText.text = "4500";

                crystal.SetActive(true);
                break;
            case ShopType.Crystal100:
                titleText.name = "Crystal100";

                switch (GameStateManager.instance.Language)
                {
                    case LanguageType.Korean:
                        priceText.text = "₩ 1200";
                        break;
                    default:
                        priceText.text = "USD $ 0.99";
                        break;
                }

                price.SetActive(true);
                break;
            case ShopType.Crystal200:
                titleText.name = "Crystal200";

                switch (GameStateManager.instance.Language)
                {
                    case LanguageType.Korean:
                        priceText.text = "₩ 3000";
                        break;
                    default:
                        priceText.text = "USD $ 2.49";
                        break;
                }

                price.SetActive(true);
                break;
            case ShopType.Crystal300:
                titleText.name = "Crystal300";

                switch (GameStateManager.instance.Language)
                {
                    case LanguageType.Korean:
                        priceText.text = "₩ 6000";
                        break;
                    default:
                        priceText.text = "USD $ 4.99";
                        break;
                }

                price.SetActive(true);
                break;
            case ShopType.DailyReward:
                titleText.name = "DailyReward";

                coinText.text = "150";

                coin.SetActive(true);

                break;
            case ShopType.PaidReward:
                break;
        }

        titleText.ReLoad();
    }
}
