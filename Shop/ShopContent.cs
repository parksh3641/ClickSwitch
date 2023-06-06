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
    public GameObject onTime;

    [Space]
    [Title("Text")]
    public LocalizationContent priceText;
    public Text coinText;
    public Text crystalText;
    public Text buyCrystalText;
    public Text onTimeText;

    string platform = "";

    Sprite[] mainIconArray;

    ImageDataBase imageDataBase;
    PlayerDataBase playerDataBase;

    private void Awake()
    {
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDatabase") as ImageDataBase;
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;

        mainIconArray = imageDataBase.GetShopArray();

        price.SetActive(false);
        coin.SetActive(false);
        crystal.SetActive(false);
        buyCrystal.SetActive(false);
        onTime.SetActive(false);
    }

    void Start()
    {
        mainIcon.sprite = mainIconArray[(int)shopType];

#if UNITY_ANDROID || UNITY_EDITOR
        platform = "_AOS";
#endif
        platform = "_IOS";

        switch (shopType)
        {
            case ShopType.RemoveAds:
                titleText.localizationName = "RemoveAds";

                priceText.localizationName = "ShopRemoveAds" + platform;

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

                priceText.localizationName = "ShopCrystal100" + platform;

                price.SetActive(true);

                if (!playerDataBase.Crystal100)
                {
                    onTime.SetActive(true);

                    onTimeText.text = "+80";
                }
                break;
            case ShopType.Crystal200:
                titleText.localizationName = "Crystal200";

                priceText.localizationName = "ShopCrystal200" + platform;

                price.SetActive(true);

                if (!playerDataBase.Crystal200)
                {
                    onTime.SetActive(true);

                    onTimeText.text = "+500";
                }
                break;
            case ShopType.Crystal300:
                titleText.localizationName = "Crystal300";

                priceText.localizationName = "ShopCrystal300" + platform;

                price.SetActive(true);

                if (!playerDataBase.Crystal300)
                {
                    onTime.SetActive(true);

                    onTimeText.text = "+1200";
                }
                break;
            case ShopType.DailyShopReward:
                titleText.localizationName = "DailyReward";

                crystalText.text = "30";

                crystal.SetActive(true);
                break;
            case ShopType.PaidProgress:
                titleText.localizationName = "PaidProgress";

                priceText.localizationName = "ShopPaidProgress" + platform;

                price.SetActive(true);

                break;
            case ShopType.Crystal400:
                titleText.localizationName = "Crystal400";

                priceText.localizationName = "ShopCrystal400" + platform;

                price.SetActive(true);

                if (!playerDataBase.Crystal400)
                {
                    onTime.SetActive(true);

                    onTimeText.text = "+2500";
                }
                break;
            case ShopType.Crystal500:
                titleText.localizationName = "Crystal500";

                priceText.localizationName = "ShopCrystal500" + platform;

                price.SetActive(true);

                if (!playerDataBase.Crystal500)
                {
                    onTime.SetActive(true);

                    onTimeText.text = "+6500";
                }
                break;
            case ShopType.Crystal600:
                titleText.localizationName = "Crystal600";

                priceText.localizationName = "ShopCrystal600" + platform;

                price.SetActive(true);

                if (!playerDataBase.Crystal600)
                {
                    onTime.SetActive(true);

                    onTimeText.text = "+14000";
                }
                break;
            case ShopType.StartPack1:
                titleText.localizationName = "StartPack1";

                priceText.localizationName = "ShopStartPack1" + platform;

                price.SetActive(true);
                break;
            case ShopType.StartPack2:
                break;
            case ShopType.StartPack3:
                break;
            case ShopType.CoinX2:
                titleText.localizationName = "CoinX2";

                priceText.localizationName = "ShopCoinX2" + platform;

                price.SetActive(true);
                break;
            case ShopType.ExpX2:
                titleText.localizationName = "ExpX2";

                priceText.localizationName = "ShopExpX2" + platform;

                price.SetActive(true);
                break;
        }

        priceText.ReLoad();
        titleText.ReLoad();
    }
}
