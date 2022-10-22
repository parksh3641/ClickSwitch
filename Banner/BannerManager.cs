using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class BannerClass
{
    public BannerType bannerType = BannerType.Banner_0;
    public int count = 0;
}

public class BannerManager : MonoBehaviour
{
    BannerType bannerType = BannerType.Banner_0;

    public GameObject bannerView;

    public BannerContent bannerContent;
    public RectTransform bannerContentTransform;

    public Image mainBanner;
    public Image profileBanner;
    public Image banner;

    public Text nickNameText;

    public GameObject saveLockObject;


    public List<BannerContent> bannerContentList = new List<BannerContent>();

    PlayerDataBase playerDataBase;
    ImageDataBase imageDataBase;
    ShopDataBase shopDataBase;

    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;
        if (shopDataBase == null) shopDataBase = Resources.Load("ShopDataBase") as ShopDataBase;

        for (int i = 0; i < System.Enum.GetValues(typeof(BannerType)).Length; i++)
        {
            BannerContent monster = Instantiate(bannerContent);
            monster.transform.parent = bannerContentTransform;
            monster.transform.position = Vector3.zero;
            monster.transform.rotation = Quaternion.identity;
            monster.transform.localScale = Vector3.one;

            monster.gameObject.SetActive(true);

            bannerContentList.Add(monster);
        }

        bannerView.SetActive(false);
        saveLockObject.SetActive(false);

        bannerContentTransform.anchoredPosition = new Vector2(0, -9999);
    }

    public void OpenBanner()
    {
        if (!bannerView.activeSelf)
        {
            bannerView.SetActive(true);

            CheckMyBanner();
        }
        else
        {
            bannerView.SetActive(false);
        }
    }

    void CheckMyBanner()
    {
        if (GameStateManager.instance.NickName != null)
        {
            nickNameText.text = GameStateManager.instance.NickName;
        }
        else
        {
            nickNameText.text = GameStateManager.instance.CustomId;
        }

        for (int i = 0; i < bannerContentList.Count; i++)
        {
            bannerContentList[i].CheckMark(false);
        }

        bannerContentList[playerDataBase.Banner].CheckMark(true);

        Initialize();
    }

    public void Initialize()
    {
        BannerType bannerType = BannerType.Banner_0;

        for (int i = 0; i < bannerContentList.Count; i++)
        {
            if (imageDataBase.GetBannerArray(bannerType))
            {
                bannerContentList[i].Initialize(this, bannerType);

                if (i < 3)
                {
                    bannerContentList[i].UnLock();
                }
            }

            bannerType++;
        }

        bannerType = BannerType.Banner_0;

        mainBanner.sprite = imageDataBase.GetBannerArray(bannerType + playerDataBase.Banner);
        profileBanner.sprite = imageDataBase.GetBannerArray(bannerType + playerDataBase.Banner);
        banner.sprite = imageDataBase.GetBannerArray(bannerType + playerDataBase.Banner);
    }

    public void UseBanner(BannerType type)
    {
        if ((int)type == playerDataBase.Banner)
        {
            saveLockObject.SetActive(true);
        }
        else
        {
            saveLockObject.SetActive(false);
        }

        for (int i = 0; i < bannerContentList.Count; i++)
        {
            bannerContentList[i].CheckMark(false);
        }

        bannerContentList[(int)type].CheckMark(true);

        bannerType = type;

        banner.sprite = imageDataBase.GetBannerArray(bannerType);
    }

    public void SaveBanner()
    {
        playerDataBase.Banner = (int)bannerType;

        mainBanner.sprite = imageDataBase.GetBannerArray(bannerType);
        profileBanner.sprite = imageDataBase.GetBannerArray(bannerType);
        banner.sprite = imageDataBase.GetBannerArray(bannerType);

        for (int i = 0; i < bannerContentList.Count; i++)
        {
            bannerContentList[i].CheckMark(false);
        }

        bannerContentList[(int)bannerType].CheckMark(true);

        saveLockObject.SetActive(true);

        if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdatePlayerStatisticsInsert("Banner", (int)bannerType);

        NotionManager.instance.UseNotion(NotionType.SaveNotion);
    }
}
