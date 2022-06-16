using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShopClass
{
    public string catalogVersion = "";
    public string itemClass = "";
    public string itemId = "";
    public string virtualCurrency = "";
    public uint price = 0;
}

[CreateAssetMenu(fileName = "ShopDataBase", menuName = "ScriptableObjects/ShopDataBase")]
public class ShopDataBase : ScriptableObject
{
    [Title("GooglePlay")]
    [ShowInInspector]
    private ShopClass removeAds;

    [Space]
    [Title("Item")]
    [ShowInInspector]
    private List<ShopClass> itemList = new List<ShopClass>();


    public void Initialize()
    {
        itemList.Clear();
    }


    public ShopClass RemoveAds
    {
        get
        {
            return removeAds;
        }
        set
        {
            removeAds = value;
        }
    }

    public void SetItem(ShopClass shopClass)
    {
        itemList.Add(shopClass);
    }
}
