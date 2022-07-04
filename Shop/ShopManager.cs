using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public GameObject shopView;

    public NotionManager notionManager;

    public ShopItemContent shopItemContent;
    public RectTransform shopItemTransform;

    public List<ShopItemContent> shopContentList = new List<ShopItemContent>();

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
}
