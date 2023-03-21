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
    public GameObject buyCrystal;

    [Space]
    [Title("Text")]
    public LocalizationContent priceText;
    public Text coinText;
    public Text crystalText;
    public Text buyCrystalText;

    Sprite[] mainIconArray;

    ImageDataBase imageDataBase;

    private void Awake()
    {
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDatabase") as ImageDataBase;

        mainIconArray = imageDataBase.GetShopArray();

        price.SetActive(false);
        coin.SetActive(false);
        crystal.SetActive(false);
        buyCrystal.SetActive(false);
    }

    void Start()
    {
        mainIcon.sprite = mainIconArray[(int)shopType];

        switch (shopType)
        {
            case ShopType.RemoveAds:
                titleText.localizationName = "RemoveAds";

                priceText.localizationName = "ShopRemoveAds";

                price.SetActive(true);

                break;
            case ShopType.WatchAd:
                titleText.localizationName = "WatchAd";

                crystalText.text = "30";

                crystal.SetActive(true);

                break;
            case ShopType.Coin1000:
                titleText.localizationName = "Coin1000";

                buyCrystalText.text = "60";

                buyCrystal.SetActive(true);
                break;
            case ShopType.Coin2000:
                titleText.localizationName = "Coin2000";

                buyCrystalText.text = "500";

                buyCrystal.SetActive(true);
                break;
            case ShopType.Coin3000:
                titleText.localizationName = "Coin3000";

                buyCrystalText.text = "4500";

                buyCrystal.SetActive(true);
                break;
            case ShopType.Crystal100:
                titleText.localizationName = "Crystal100";

                priceText.localizationName = "ShopCrystal100";

                price.SetActive(true);
                break;
            case ShopType.Crystal200:
                titleText.localizationName = "Crystal200";

                priceText.localizationName = "ShopCrystal200";

                price.SetActive(true);
                break;
            case ShopType.Crystal300:
                titleText.localizationName = "Crystal300";

                priceText.localizationName = "ShopCrystal300";

                price.SetActive(true);
                break;
            case ShopType.DailyShopReward:
                titleText.localizationName = "DailyReward";

                crystalText.text = "30";

                crystal.SetActive(true);

                break;
            case ShopType.PaidProgress:
                titleText.localizationName = "PaidProgress";

                priceText.localizationName = "ShopPaidProgress";

                price.SetActive(true);
                break;
        }

        priceText.ReLoad();
        titleText.ReLoad();
    }
}
