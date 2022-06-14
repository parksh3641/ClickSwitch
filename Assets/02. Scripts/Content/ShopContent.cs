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
                priceText.text = "₩ 1200";
                break;
            case LanguageType.English:
                priceText.text = "USD $ 1";
                break;
            case LanguageType.Japenese:
                priceText.text = "130 円";
                break;
            case LanguageType.Chinese:
                priceText.text = "6 元";
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
