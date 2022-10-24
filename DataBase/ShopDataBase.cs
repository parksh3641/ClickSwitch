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

    [Space]
    [Title("Banner")]
    public List<BannerClass> bannerList = new List<BannerClass>();

    public void Initialize()
    {
        itemList.Clear();
        iconList.Clear();
        etcList.Clear();
        bannerList.Clear();

        for (int i = 0; i < System.Enum.GetValues(typeof(ItemType)).Length; i ++)
        {
            ShopClass shopClass = new ShopClass();
            itemList.Add(shopClass);
        }

        for (int i = 0; i < System.Enum.GetValues(typeof(IconType)).Length; i++)
        {
            IconClass iconClass = new IconClass();
            IconType iconType = IconType.Icon_0 + i;
            iconClass.iconType = iconType;
            iconList.Add(iconClass);
        }

        for (int i = 0; i < System.Enum.GetValues(typeof(ETCType)).Length; i++)
        {
            ShopClass shopClass = new ShopClass();
            etcList.Add(shopClass);
        }

        for (int i = 0; i < System.Enum.GetValues(typeof(BannerType)).Length; i++)
        {
            BannerClass bannerClass = new BannerClass();
            BannerType bannerType = BannerType.Banner_0 + i;
            bannerClass.bannerType = bannerType;
            bannerList.Add(bannerClass);
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

    public void AddIcon(IconType type)
    {
        for (int i = 0; i < iconList.Count; i++)
        {
            if (iconList[i].iconType.Equals(type))
            {
                iconList[i].count += 1;
                break;
            }
        }
    }

    public void AddIconAll(IconType type)
    {
        for (int i = 0; i < iconList.Count; i++)
        {
            if (iconList[i].iconType.Equals(type))
            {
                iconList[i].count = 5;
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

    public int GetIconNumber(IconType type)
    {
        int number = 0;
        for (int i = 0; i < iconList.Count; i++)
        {
            if (iconList[i].iconType.Equals(type))
            {
                number = iconList[i].count;
            }
        }
        return number;
    }

    public int GetIconHoldNumber()
    {
        int number = 0;
        for (int i = 0; i < iconList.Count; i++)
        {
            if(iconList[i].count >= 5)
            {
                number++;
            }
        }
        return number + 3;
    }

    #endregion

    #region Banner
    public void SetBanner(BannerType type, int number)
    {
        for (int i = 0; i < bannerList.Count; i++)
        {
            if (bannerList[i].bannerType.Equals(type))
            {
                bannerList[i].count = number;
                break;
            }
        }
    }

    public BannerClass GetBannerState(BannerType type)
    {
        BannerClass bannerClass = new BannerClass();
        for (int i = 0; i < bannerList.Count; i++)
        {
            if (bannerList[i].bannerType.Equals(type))
            {
                bannerClass = bannerList[i];
            }
        }

        return bannerClass;
    }

    #endregion
}
