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

    public NotionManager notionManager;

    public ShopItemContent shopItemContent;
    public RectTransform shopItemTransform;

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
    public Text watchAdCountText;

    private int adCoolTime = 0;

    WaitForSeconds waitForSeconds = new WaitForSeconds(1);

    List<ShopItemContent> shopContentList = new List<ShopItemContent>();
    List<ShopItemContent> shopContentETCList = new List<ShopItemContent>();

    Sprite[] vcArray;
    Sprite[] itemArray;
    Sprite[] etcArray;

    PlayerDataBase playerDataBase;
    ShopDataBase shopDataBase;
    ImageDataBase imageDataBase;

    private void Awake()
    {
        shopView.SetActive(false);
        buyWindow.SetActive(false);

        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;
        if (shopDataBase == null) shopDataBase = Resources.Load("ShopDataBase") as ShopDataBase;
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;
    }

    public void Initialize()
    {
        vcArray = imageDataBase.GetVCArray();
        itemArray = imageDataBase.GetItemArray();
        etcArray = imageDataBase.GetETCArray();

        for (int i = 0; i < shopDataBase.etcList.Count; i++)
        {
            ShopItemContent monster = Instantiate(shopItemContent);
            monster.transform.parent = shopItemTransform;
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
            monster.transform.parent = shopItemTransform;
            monster.transform.position = Vector3.zero;
            monster.transform.rotation = Quaternion.identity;
            monster.transform.localScale = Vector3.one;
            monster.Initialize(this, shopDataBase.ItemList[i], itemArray[i]);
            monster.gameObject.SetActive(true);

            shopContentList.Add(monster);
        }

        shopItemTransform.anchoredPosition = new Vector2(0, -9999);
    }

    public void OpenShop()
    {
        if (!shopView.activeSelf)
        {
            shopView.SetActive(true);

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
        if(shopClass.itemId.Equals("IconBox"))
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
            Debug.Log("???? ????");

            watchAdLock.SetActive(true);
            GameStateManager.instance.WatchAd = false;

            PlayerPrefs.SetString("AdCoolTime", DateTime.Now.AddSeconds(ValueManager.instance.GetAdCoolTime()).ToString("yyyy-MM-dd HH:mm:ss"));

            adCoolTime = (int)ValueManager.instance.GetAdCoolTime();

            StartCoroutine(WatchAdCorution());
        }
        else
        {
            Debug.Log("???? ???? ????");

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
        NotionManager.instance.UseNotion(NotionType.SuccessWatchAd);

        if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdateAddCurrency(MoneyType.Coin, 300);

        SetWatchAd(true);
    }

    #endregion
}
