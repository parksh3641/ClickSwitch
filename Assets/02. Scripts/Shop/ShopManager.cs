using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public GameObject shopView;


    private void Awake()
    {
        shopView.SetActive(false);
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

    public void BuyRemoveAds()
    {
        //playfab에 아이템 넣어주기
    }
}
