using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class ShopClass
{
    public string catalogVersion = "";
    public string itemClass = "";
    public string itemId = "";
    public string itemInstanceId = "";
    public string virtualCurrency = "";
    public uint price = 0;
}

[CreateAssetMenu(fileName = "ShopDataBase", menuName = "ScriptableObjects/ShopDataBase")]
public class ShopDataBase : ScriptableObject
{
    [Title("GooglePlay")]
    public ShopClass removeAds;

    [Space]
    [Title("Item")]
    public List<ShopClass> itemList = new List<ShopClass>();

    [Space]
    [Title("Icon")]
    public List<IconClass> iconList = new List<IconClass>();

    [Space]
    [Title("ETC")]
    public List<ShopClass> etcList = new List<ShopClass>();

    public void Initialize()
    {
        itemList.Clear();

        for(int i = 0; i < System.Enum.GetValues(typeof(ItemType)).Length; i ++)
        {
            ShopClass shopClass = new ShopClass();
            itemList.Add(shopClass);
        }

        iconList.Clear();

        for (int i = 0; i < System.Enum.GetValues(typeof(IconType)).Length; i++)
        {
            IconClass iconClass = new IconClass();
            IconType iconType = IconType.Icon_0 + i;
            iconClass.iconType = iconType;
            iconList.Add(iconClass);
        }

        etcList.Clear();
        for (int i = 0; i < System.Enum.GetValues(typeof(ETCType)).Length; i++)
        {
            ShopClass shopClass = new ShopClass();
            etcList.Add(shopClass);
        }

    }

    public List<ShopClass> ItemList
    {
        get
        {
            return itemList;
        }
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
        switch(shopClass.itemId)
        {
            case "Clock":
                itemList[0] = shopClass;
                break;
            case "Shield":
                itemList[1] = shopClass;
                break;
            case "Combo":
                itemList[2] = shopClass;
                break;
            case "Exp":
                itemList[3] = shopClass;
                break;
            case "Slow":
                itemList[4] = shopClass;
                break;
        }
        //itemList.Add(shopClass);

        //itemList = Enumerable.Reverse(itemList).ToList();
    }

    public void SetETC(ShopClass shopClass)
    {
        switch(shopClass.itemId)
        {
            case "IconBox":
                etcList[0] = shopClass;
                break;
        }
    }

    public void SetItemInstanceId(string itemid, string instanceid)
    {
        for(int i = 0; i < itemList.Count; i ++)
        {
            if(itemList[i].itemId.Equals(itemid))
            {
                itemList[i].itemInstanceId = instanceid;
            }
        }
    }

    public string GetItemInstanceId(string itemid)
    {
        string itemInstanceId = "";

        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i].itemId.Equals(itemid))
            {
                itemInstanceId = itemList[i].itemInstanceId;
            }
        }

        return itemInstanceId;
    }

    #region Icon
    public void SetIcon(IconType type, int number)
    {
        for (int i = 0; i < iconList.Count; i++)
        {
            if (iconList[i].iconType.Equals(type))
            {
                iconList[i].count = number;
                break;
            }
        }
    }

    public IconClass GetIconState(IconType type)
    {
        IconClass iconClass = new IconClass();
        for (int i = 0; i < iconList.Count; i++)
        {
            if (iconList[i].iconType.Equals(type))
            {
                iconClass = iconList[i];
            }
        }

        return iconClass;
    }

    #endregion
}
