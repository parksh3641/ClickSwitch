using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    ShopClass shopClass;

    public GameObject shopView;
    public GameObject showVCView;

    public GameObject alarm;

    public NotionManager notionManager;

    [Space]
    [Title("InAppPurchase")]
    public GameObject removeAd;
    public GameObject paidProgress;

    public ShopItemContent shopItemContent;
    public RectTransform[] scrollViewTransform;

    [Space]
    [Title("TopMenu")]
    public Image[] topMenuImgArray;
    public Sprite[] topMenuSpriteArray;

    [Title("ScrollView")]
    public GameObject[] scrollVeiwArray;

    [Space]
    [Title("BuyWindow")]
    public GameObject buyWindow;

    public Image buyItemBackground;
    public Image buyItemIcon;

    public Image buyButtonBackground;
    public Image buyButtonIcon;

    public Text buyCountText;
    public Text buyPriceText;

    private int buyCount = 0;
    private int buyPrice = 0;

    [Space]
    [Title("Ad")]
    public GameObject watchAdLock;
    public GameObject dailyLock;
    public Text watchAdCountText;

    private int topNumber = 0;
    private int adCoolTime = 0;

    WaitForSeconds waitForSeconds = new WaitForSeconds(1);

    List<ShopItemContent> shopContentList = new List<ShopItemContent>();
    List<ShopItemContent> shopContentETCList = new List<ShopItemContent>();

    Sprite[] vcArray;
    Sprite[] itemArray;
    Sprite[] etcArray;

    public SoundManager soundManager;

    PlayerDataBase playerDataBase;
    ShopDataBase shopDataBase;
    ImageDataBase imageDataBase;

    private void Awake()
    {
        shopView.SetActive(false);
        showVCView.SetActive(false);
        buyWindow.SetActive(false);
        alarm.SetActive(false);

        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;
        if (shopDataBase == null) shopDataBase = Resources.Load("ShopDataBase") as ShopDataBase;
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;
    }

    void Start()
    {
        if(!GameStateManager.instance.DailyReward) alarm.SetActive(true);
    }

    public void Initialize()
    {
        vcArray = imageDataBase.GetVCArray();
        itemArray = imageDataBase.GetItemArray();
        etcArray = imageDataBase.GetETCArray();

        for (int i = 0; i < shopDataBase.etcList.Count; i++)
        {
            ShopItemContent monster = Instantiate(shopItemContent);
            monster.transform.parent = scrollViewTransform[1];
            monster.transform.position = Vector3.zero;
            monster.transform.rotation = Quaternion.identity;
            monster.transform.localScale = Vector3.one;
            monster.InitializeETC(this, shopDataBase.etcList[i], etcArray[i]);
            monster.gameObject.SetActive(true);

            shopContentETCList.Add(monster);
        }

        for (int i = 0; i < shopDataBase.ItemList.Count; i++)
        {
            ShopItemContent monster = Instantiate(shopItemContent);
            monster.transform.parent = scrollViewTransform[1];
            monster.transform.position = Vector3.zero;
            monster.transform.rotation = Quaternion.identity;
            monster.transform.localScale = Vector3.one;
            monster.Initialize(this, shopDataBase.ItemList[i], itemArray[i]);
            monster.gameObject.SetActive(true);

            shopContentList.Add(monster);
        }

        for (int i = 0; i < scrollViewTransform.Length; i ++)
        {
            scrollViewTransform[i].anchoredPosition = new Vector2(0, -4999);
        }

        if (!GameStateManager.instance.DailyReward)
        {
            dailyLock.SetActive(false);
        }
        else
        {
            dailyLock.SetActive(true);
        }

        topNumber = -1;
        ChangeTopMenu(0);

        CheckPurchaseItem();
    }

    public void CheckPurchaseItem()
    {
        if(playerDataBase.RemoveAd)
        {
            removeAd.SetActive(false);
        }

        if(playerDataBase.PaidProgress)
        {
            paidProgress.SetActive(false);
        }
    }

    public void OpenShop()
    {
        if (!shopView.activeSelf)
        {
            shopView.SetActive(true);
            showVCView.SetActive(true);

            if (!GameStateManager.instance.WatchAd)
            {
                LoadWatchAd();
            }
            else
            {
                SetWatchAd(false);
            }
        }
        else
        {
            shopView.SetActive(false);
            showVCView.SetActive(false);
        }
    }

    public void OpenBuyWindow(ShopClass _shopClass, Sprite icon)
    {
        buyWindow.SetActive(true);

        shopClass = _shopClass;

        switch (_shopClass.virtualCurrency)
        {
            case "GO":
                buyButtonIcon.sprite = vcArray[0];
                break;
            case "ST":
                buyButtonIcon.sprite = vcArray[1];
                break;
        }

        buyItemIcon.sprite = icon;

        buyCount = 1;
        buyPrice = (int)_shopClass.price;

        buyCountText.text = buyCount.ToString();
        buyPriceText.text = buyPrice.ToString();
    }

    public void CloseBuyWindow()
    {
        buyWindow.SetActive(false);
    }

    public void ChangeTopMenu(int number)
    {
        if (topNumber != number)
        {
            topNumber = number;

            for (int i = 0; i < topMenuImgArray.Length; i++)
            {
                topMenuImgArray[i].sprite = topMenuSpriteArray[0];
            }
            topMenuImgArray[number].sprite = topMenuSpriteArray[1];

            ChangeShopView(topNumber);
        }
    }

    public void ChangeShopView(int number)
    {
        for(int i = 0; i < scrollVeiwArray.Length; i ++)
        {
            scrollVeiwArray[i].SetActive(false);
        }

        scrollVeiwArray[number].SetActive(true);
    }

    public void UpBuyCount()
    {
        if (buyCount + 1 < 100)
        {
            buyCount++;
        }

        buyCountText.text = buyCount.ToString();
        buyPriceText.text = (buyPrice * buyCount).ToString();
    }

    public void DownBuyCount()
    {
        if(buyCount - 1 > 0)
        {
            buyCount--;
        }

        buyCountText.text = buyCount.ToString();
        buyPriceText.text = (buyPrice * buyCount).ToString();
    }

    public void OnBuyItem()
    {
        int price = buyPrice * buyCount;

        switch (shopClass.virtualCurrency)
        {
            case "GO":
                if (playerDataBase.Coin < price)
                {
                    CheckBuyItem(false);
                    return;
                }
                else
                {
                    if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdateSubtractCurrency(MoneyType.Coin, price);
                }
                break;
            case "ST":
                if (playerDataBase.Crystal < price)
                {
                    CheckBuyItem(false);
                    return;
                }
                else
                {
                    if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdateSubtractCurrency(MoneyType.Crystal, price);
                }
                break;
        }

        soundManager.PlaySFX(GameSfxType.BuyItem);

        if (shopClass.itemId.Equals("IconBox"))
        {
            if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdatePlayerStatisticsInsert("IconBox",buyCount);

            CheckBuyItem(true);
        }
        else
        {
            if (PlayfabManager.instance.isActive) PlayfabManager.instance.PurchaseItem(shopClass, CheckBuyItem, buyCount);
        }
    }

    public void CheckBuyItem(bool check)
    {
        if (check)
        {
            notionManager.UseNotion(NotionType.SuccessBuyItem);

            CloseBuyWindow();
        }
        else
        {
            notionManager.UseNotion(NotionType.FailBuyItem);
        }
    }

#region WatchAd

    void SetWatchAd(bool check)
    {
        if (check)
        {
            Debug.Log("Watch Ad Play");

            watchAdLock.SetActive(true);
            GameStateManager.instance.WatchAd = false;

            PlayerPrefs.SetString("AdCoolTime", DateTime.Now.AddSeconds(ValueManager.instance.GetAdCoolTime()).ToString("yyyy-MM-dd HH:mm:ss"));

            adCoolTime = (int)ValueManager.instance.GetAdCoolTime();

            StartCoroutine(WatchAdCorution());
        }
        else
        {
            Debug.Log("Watch Ad Stop");

            watchAdLock.SetActive(false);
            GameStateManager.instance.WatchAd = true;

            StopAllCoroutines();
        }
    }

    void LoadWatchAd()
    {
        DateTime time = DateTime.Parse(PlayerPrefs.GetString("AdCoolTime"));
        DateTime now = DateTime.Now;

        TimeSpan span = time - now;

        if (span.TotalSeconds > 0)
        {
            adCoolTime = (int)span.TotalSeconds;

            watchAdLock.SetActive(true);

            StopAllCoroutines();
            StartCoroutine(WatchAdCorution());
        }
        else
        {
            SetWatchAd(false);
        }
    }

    IEnumerator WatchAdCorution()
    {
        if (adCoolTime > 0)
        {
            adCoolTime -= 1;
        }
        else
        {
            SetWatchAd(false);
            yield break;
        }

        watchAdCountText.text = (adCoolTime / 60).ToString("D2") + ":" + (adCoolTime % 60).ToString("D2");

        yield return waitForSeconds;
        StartCoroutine(WatchAdCorution());
    }

    public void SuccessWatchAd()
    {
        if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdateAddCurrency(MoneyType.Coin, 500);

        NotionManager.instance.UseNotion(NotionType.SuccessWatchAd);

        SetWatchAd(true);
    }

    public void CustomReward(int number)
    {
        CustomReward(ShopType.RemoveAds + number);
    }

    public void CustomReward(ShopType type)
    {
        Debug.Log(type);
        switch (type)
        {
            case ShopType.Coin1000:
                if(playerDataBase.Coin + 2000 < 10000000)
                {
                    if (playerDataBase.Crystal >= 60)
                    {
                        if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdateSubtractCurrency(MoneyType.Crystal, 60);
                        if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdateAddCurrency(MoneyType.Coin, 2000);

                        NotionManager.instance.UseNotion(NotionType.ReceiveNotion);
                    }
                    else
                    {
                        NotionManager.instance.UseNotion(NotionType.LowCrystalNotion);
                    }
                }
                else
                {
                    NotionManager.instance.UseNotion(NotionType.FailBuyItem);
                }
                break;
            case ShopType.Coin2000:
                if (playerDataBase.Coin + 20000 < 10000000)
                {
                    if (playerDataBase.Crystal >= 500)
                    {
                        if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdateSubtractCurrency(MoneyType.Crystal, 500);
                        if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdateAddCurrency(MoneyType.Coin, 20000);

                        NotionManager.instance.UseNotion(NotionType.ReceiveNotion);
                    }
                    else
                    {
                        NotionManager.instance.UseNotion(NotionType.LowCrystalNotion);
                    }
                }
                else
                {
                    NotionManager.instance.UseNotion(NotionType.FailBuyItem);
                }
                break;
            case ShopType.Coin3000:
                if (playerDataBase.Coin + 200000 < 10000000)
                {
                    if (playerDataBase.Crystal >= 4500)
                    {
                        if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdateSubtractCurrency(MoneyType.Crystal, 4500);
                        if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdateAddCurrency(MoneyType.Coin, 200000);

                        NotionManager.instance.UseNotion(NotionType.ReceiveNotion);
                    }
                    else
                    {
                        NotionManager.instance.UseNotion(NotionType.LowCrystalNotion);
                    }
                }
                else
                {
                    NotionManager.instance.UseNotion(NotionType.FailBuyItem);
                }
                break;
            case ShopType.DailyReward:
                if(!GameStateManager.instance.DailyReward && !dailyLock.activeInHierarchy)
                {
                    if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdateAddCurrency(MoneyType.Coin, 150);

                    dailyLock.SetActive(true);

                    NotionManager.instance.UseNotion(NotionType.ReceiveNotion);

                    GameStateManager.instance.DailyReward = true;
                }
                else
                {
                    NotionManager.instance.UseNotion(NotionType.NowReceivedNotion);
                }

                alarm.SetActive(false);
                break;
        }
    }

    #endregion
}
