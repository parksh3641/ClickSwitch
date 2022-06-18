using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ImageDataBase", menuName = "ScriptableObjects/ImageDataBase")]
public class ImageDataBase : ScriptableObject
{
    public Sprite[] iconArray;

    public Sprite[] countryArray;

    [Title("Shop")]
    public Sprite[] shopArray;
    public Sprite[] vcArray;

    public Sprite[] itemBackgroundArray;
    public Sprite[] itemArray;


    public Sprite[] GetVCArray()
    {
        return vcArray;
    }

    public Sprite[] GetIconArray()
    {
        return iconArray;
    }

    public Sprite[] GetCountryArray()
    {
        return countryArray;
    }

    public Sprite[] GetShopArray()
    {
        return shopArray;
    }

    public Sprite[] GetItemBackgroundArray()
    {
        return itemBackgroundArray;
    }

    public Sprite[] GetItemArray()
    {
        return itemArray;
    }
}
