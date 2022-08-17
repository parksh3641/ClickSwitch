using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public static StateManager instance;

    public bool isInit = false;

    public NickNameManager nickNameManager;
    public ShopManager shopManager;
    public ItemManager itemManager;
    public IconManager iconManager;
    public NewsManager newsManager;
    public LevelManager levelManager;
    public TrophyManager trophyManager;
    public HelpManager helpManager;
    public MailBoxManager mailBoxManager;
    public DailyManager dailyManager;
    public UpgradeManager upgradeManager;
    public IconBoxManager iconBoxManager;

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

            nickNameManager.Initialize();
            shopManager.Initialize();
            itemManager.Initialize();
            iconManager.Initialize();
            newsManager.Initialize();
            levelManager.Initialize();
            trophyManager.Initialize();
            helpManager.Initialize();
            mailBoxManager.Initialize();
            dailyManager.Initialize();
            upgradeManager.Initialize();
            iconBoxManager.Initialize();
        }
    }

    public void ChangeNumber()
    {
        eChangeNumber();
    }
}
