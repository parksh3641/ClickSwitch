using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public static StateManager instance;

    public ShopManager shopManager;
    public ItemManager itemManager;

    public delegate void PurchasEvent();
    public static event PurchasEvent eChangeNumber;

    void Awake()
    {
        instance = this;
    }

    public void Initialize()
    {
        shopManager.Initialize();
        itemManager.Initialize();
    }

    public void ChangeNumber()
    {
        eChangeNumber();
    }
}
