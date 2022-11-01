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
    public LocalizationContent priceText;
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

                priceText.name = "ShopRemoveAds";

                price.SetActive(true);

                break;
            case ShopType.WatchAd:
                titleText.name = "WatchAd";

                coinText.text = "500";

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

                priceText.name = "ShopCrystal100";

                price.SetActive(true);
                break;
            case ShopType.Crystal200:
                titleText.name = "Crystal200";

                priceText.name = "ShopCrystal200";

                price.SetActive(true);
                break;
            case ShopType.Crystal300:
                titleText.name = "Crystal300";

                priceText.name = "ShopCrystal300";

                price.SetActive(true);
                break;
            case ShopType.DailyReward:
                titleText.name = "DailyReward";

                coinText.text = "150";

                coin.SetActive(true);

                break;
            case ShopType.PaidProgress:
                titleText.name = "PaidProgress";

                priceText.name = "ShopPaidProgress";

                price.SetActive(true);
                break;
        }

        priceText.ReLoad();
        titleText.ReLoad();
    }
}
