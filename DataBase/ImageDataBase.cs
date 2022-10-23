using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ImageDataBase", menuName = "ScriptableObjects/ImageDataBase")]
public class ImageDataBase : ScriptableObject
{
    [Title("Rank")]
    public Sprite[] rankArray;

    [Title("FilpCard")]
    public Sprite[] filpCardArray;

    [Title("FingerSnap")]
    public Sprite[] fingerSnapArray;

    [Title("GameChoice Icon")]
    public Sprite[] iconArray;

    [Title("GameMode Background")]
    public Sprite[] modeBackgroundArray;

    [Title("Country")]
    public Sprite[] countryArray;

    [Title("Shop")]
    public Sprite[] shopArray;
    public Sprite[] vcArray;

    [Title("Item")]
    public Sprite[] itemArray;
    public Sprite[] etcArray;

    [Title("Upgrade")]
    public Sprite[] upgradeIconArray;

    [Title("Profile Icon")]
    public Sprite[] profileIconArray;

    [Title("Banner")]
    public Sprite[] bannerArray;

    public Sprite[] GetRankArray()
    {
        return rankArray;
    }

    public Sprite[] GetFilpCardArray()
    {
        return filpCardArray;
    }

    public Sprite[] GetFingerSnapArray()
    {
        return fingerSnapArray;
    }

    public Sprite[] GetIconArray()
    {
        return iconArray;
    }

    public Sprite[] GetModeBackgroundArray()
    {
        return modeBackgroundArray;
    }

    public Sprite[] GetProfileIconArray()
    {
        return profileIconArray;
    }

    public Sprite GetProfileIconArray(IconType type)
    {
        return profileIconArray[(int)type];
    }

    public Sprite[] GetCountryArray()
    {
        return countryArray;
    }

    public Sprite[] GetShopArray()
    {
        return shopArray;
    }

    public Sprite[] GetVCArray()
    {
        return vcArray;
    }

    public Sprite[] GetItemArray()
    {
        return itemArray;
    }

    public Sprite[] GetETCArray()
    {
        return etcArray;
    }

    public Sprite[] GetUpgradeArray()
    {
        return upgradeIconArray;
    }

    public Sprite[] GetBannerArray()
    {
        return bannerArray;
    }

    public Sprite GetBannerArray(BannerType type)
    {
        return bannerArray[(int)type];
    }
}
