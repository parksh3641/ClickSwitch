using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopContent : MonoBehaviour
{
    public ShopType shopType = ShopType.RemoveAds;

    public Image mainIcon;
    public Text TitleText;
    public Text priceText;

    public Sprite[] mainIconArray;

    void Start()
    {
        mainIcon.sprite = mainIconArray[(int)shopType];

        switch (shopType)
        {
            case ShopType.RemoveAds:
                TitleText.text = LocalizationManager.instance.GetString("RemoveAds");
                break;
        }

        switch (GameStateManager.instance.Language)
        {
            case LanguageType.Korean:
                priceText.text = "1200 KRW";
                break;
            case LanguageType.English:
                priceText.text = "1 USD";

                break;
            default:


                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
