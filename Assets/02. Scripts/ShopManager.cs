using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public GameObject shopView;

    public ShopContent shopContent;


    public List<ShopContent> shopContentList = new List<ShopContent>();

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
}
