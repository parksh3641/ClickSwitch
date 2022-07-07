using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public GameObject shopView;

    public NotionManager notionManager;

    public ShopItemContent shopItemContent;
    public RectTransform shopItemTransform;

    [Space]
    [Title("Ad")]
    public GameObject watchAdLock;
    public Text watchAdCountText;

    private int adCoolTime = 0;

    WaitForSeconds waitForSeconds = new WaitForSeconds(1);

    List<ShopItemContent> shopContentList = new List<ShopItemContent>();

    Sprite[] itemArray;

    PlayerDataBase playerDataBase;
    ShopDataBase shopDataBase;
    ImageDataBase imageDataBase;

    private void Awake()
    {
        shopView.SetActive(false);

        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;
        if (shopDataBase == null) shopDataBase = Resources.Load("ShopDataBase") as ShopDataBase;
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;
    }

    public void Initialize()
    {
        itemArray = imageDataBase.GetItemArray();

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

    public void OnBuy(ShopClass shopClass)
    {
        if (PlayfabManager.instance.isActive) PlayfabManager.instance.PurchaseItem(shopClass, CheckBuyItem);
    }

    public void CheckBuyItem(bool check)
    {
        if (check)
        {
            notionManager.UseNotion(NotionType.SuccessBuyItem);
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
            Debug.Log("±¤°í Àá±Ý");

            watchAdLock.SetActive(true);
            GameStateManager.instance.WatchAd = false;

            PlayerPrefs.SetString("AdCoolTime", DateTime.Now.AddSeconds(ValueManager.instance.GetAdCoolTime()).ToString("yyyy-MM-dd HH:mm:ss"));

            adCoolTime = (int)ValueManager.instance.GetAdCoolTime();

            StartCoroutine(WatchAdCorution());
        }
        else
        {
            Debug.Log("±¤°í Àá±Ý ÇØÁ¦");

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

        if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdateAddCurrency(MoneyType.Coin, 200);

        SetWatchAd(true);
    }

    #endregion
}
