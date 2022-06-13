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
    [Title("ItemId")]
    [ShowInInspector]
    private ShopClass removeAds;



    public void Initialize()
    {

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
}
