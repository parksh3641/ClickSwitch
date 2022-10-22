using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BannerContent : MonoBehaviour
{
    public BannerType bannerType;
    BannerClass bannerClass;

    public Image banner;

    public int count = 0;

    public Text nickNameText;
    public LocalizationContent lockText;
    public GameObject lockObject;
    public GameObject checkMark;

    BannerManager bannerManager;

    ShopDataBase shopDataBase;
    ImageDataBase imageDataBase;

    private void Awake()
    {
        if (shopDataBase == null) shopDataBase = Resources.Load("ShopDataBase") as ShopDataBase;
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;

        lockObject.SetActive(true);
        checkMark.SetActive(false);
    }

    public void Initialize(BannerManager manager, BannerType type)
    {
        if (GameStateManager.instance.NickName != null)
        {
            nickNameText.text = GameStateManager.instance.NickName;
        }
        else
        {
            nickNameText.text = GameStateManager.instance.CustomId;
        }

        SetLockText(type);

        bannerManager = manager;
        bannerType = type;

        banner.sprite = imageDataBase.GetBannerArray(bannerType);

        InitState();
    }

    public void InitState()
    {
        bannerClass = shopDataBase.GetBannerState(bannerType);

        if (bannerClass.count >= 1)
        {
            UnLock();
        }
    }

    public void UnLock()
    {
        lockObject.SetActive(false);
    }

    public void CheckMark(bool check)
    {
        checkMark.SetActive(check);
    }

    public void OnClick()
    {
        if (!lockObject.activeSelf)
        {
            bannerManager.UseBanner(bannerType);
        }
    }

    public void SetLockText(BannerType type)
    {

        lockText.name = "";
        switch (type)
        {
            case BannerType.Banner_0:
                lockText.name = "None";
                break;
            case BannerType.Banner_1:
                lockText.name = "None";
                break;
            case BannerType.Banner_2:
                lockText.name = "None";
                break;
            case BannerType.Banner_3:
                lockText.name = "LockShop";
                break;
            case BannerType.Banner_4:
                lockText.name = "LockShop";
                break;
            case BannerType.Banner_5:
                lockText.name = "LockShop";
                break;
            case BannerType.Banner_6:
                lockText.name = "LockShop";
                break;
            case BannerType.Banner_7:
                lockText.name = "LockShop";
                break;
            case BannerType.Banner_8:
                lockText.name = "LockProgress";
                break;
            case BannerType.Banner_9:
                lockText.name = "LockProgress";
                break;
            case BannerType.Banner_10:
                lockText.name = "LockProgress";
                break;
            case BannerType.Banner_11:
                lockText.name = "LockProgress";
                break;
            case BannerType.Banner_12:
                lockText.name = "LockProgress";
                break;
            case BannerType.Banner_13:
                lockText.name = "LockTrophy1";
                break;
            case BannerType.Banner_14:
                lockText.name = "LockTrophy2";
                break;
            case BannerType.Banner_15:
                lockText.name = "LockTrophy3";
                break;
            case BannerType.Banner_16:
                lockText.name = "LockTrophy4";
                break;
            case BannerType.Banner_17:
                lockText.name = "LockTrophy5";
                break;
            case BannerType.Banner_18:
                lockText.name = "LockTrophy6";
                break;
            case BannerType.Banner_19:
                lockText.name = "LockTrophy7";
                break;
            case BannerType.Banner_20:
                lockText.name = "LockTrophy8";
                break;
        }

        lockText.ReLoad();
    }
}
