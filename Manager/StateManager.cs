using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public static StateManager instance;

    public bool isInit = false;

    public ShopManager shopManager;
    public ItemManager itemManager;
    public IconManager iconManager;
    public LevelManager levelManager;
    public TrophyManager trophyManager;
    public HelpManager helpManager;

    public delegate void PurchasEvent();
    public static event PurchasEvent eChangeNumber;

    void Awake()
    {
        instance = this;
    }

    public void Initialize()
    {
        if(!isInit)
        {
            isInit = true;

            shopManager.Initialize();
            itemManager.Initialize();
            iconManager.Initialize();
            levelManager.Initialize();
            trophyManager.Initialize();
            helpManager.Initialize();
        }
    }

    public void ChangeNumber()
    {
        eChangeNumber();
    }
}
