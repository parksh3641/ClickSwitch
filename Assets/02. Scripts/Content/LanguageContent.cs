using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageContent : MonoBehaviour
{
    public LanguageType languageType = LanguageType.Korean;

    public Image country;
    Sprite[] countryArray;

    public GameObject frame;

    public ImageDataBase imageDataBase;

    private void Awake()
    {
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;

        countryArray = imageDataBase.GetCountryArray();

        country.sprite = countryArray[(int)languageType];
    }


    public void OnFrame(bool check)
    {
        frame.SetActive(check);
    }

}
