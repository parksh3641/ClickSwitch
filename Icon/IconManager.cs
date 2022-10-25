using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class IconClass
{
    public IconType iconType = IconType.Icon_0;
    public int count = 0;
}

public class IconManager : MonoBehaviour
{
    IconType iconType = IconType.Icon_0;

    public GameObject iconView;

    public IconContent iconContent;
    public RectTransform iconContentTransform;


    public Image mainIcon;
    public Image profileIcon;

    public Text plusScoreText;

    public GameObject saveLockObject;


    public List<IconContent> iconContentList = new List<IconContent>();

    PlayerDataBase playerDataBase;
    ImageDataBase imageDataBase;
    ShopDataBase shopDataBase;

    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;
        if (shopDataBase == null) shopDataBase = Resources.Load("ShopDataBase") as ShopDataBase;

        for (int i = 0; i < System.Enum.GetValues(typeof(IconType)).Length; i ++)
        {
            IconContent monster = Instantiate(iconContent);
            monster.transform.parent = iconContentTransform;
            monster.transform.position = Vector3.zero;
            monster.transform.rotation = Quaternion.identity;
            monster.transform.localScale = Vector3.one;

            monster.gameObject.SetActive(true);

            iconContentList.Add(monster);
        }

        iconView.SetActive(false);
        saveLockObject.SetActive(true);

        iconContentTransform.anchoredPosition = new Vector2(0, -9999);
    }

    public void OpenIcon()
    {
        if (!iconView.activeSelf)
        {
            iconView.SetActive(true);

            CheckMyIcon();
        }
        else
        {
            iconView.SetActive(false);
        }
    }

    public void CheckMyIcon()
    {
        for (int i = 0; i < iconContentList.Count; i++)
        {
            iconContentList[i].CheckMark(false);
        }

        iconContentList[playerDataBase.Icon].CheckMark(true);

        int plusScore = shopDataBase.GetIconHoldNumber();

        plusScoreText.text = LocalizationManager.instance.GetString("PlusScoreInfo") + " +" + (0.5f * plusScore).ToString() + "%";

        Initialize();
    }

    public void Initialize()
    {
        IconType iconType = IconType.Icon_0;

        for(int i = 0; i < iconContentList.Count; i ++)
        {
            if (imageDataBase.GetProfileIconArray(iconType))
            {
                iconContentList[i].Initialize(this, iconType);

                if (i < 3)
                {
                    iconContentList[i].UnLock();
                }
            }

            iconType++;
        }

        iconType = IconType.Icon_0;

        mainIcon.sprite = imageDataBase.GetProfileIconArray(iconType + playerDataBase.Icon);
        profileIcon.sprite = imageDataBase.GetProfileIconArray(iconType + playerDataBase.Icon);
    }

    public void UseIcon(IconType type)
    {
        if ((int)type == playerDataBase.Icon)
        {
            saveLockObject.SetActive(true);
        }
        else
        {
            saveLockObject.SetActive(false);
        }

        for (int i = 0; i < iconContentList.Count; i++)
        {
            iconContentList[i].CheckMark(false);
        }

        iconContentList[(int)type].CheckMark(true);

        iconType = type;
    }

    public void SaveIcon()
    {
        playerDataBase.Icon = (int)iconType;

        mainIcon.sprite = imageDataBase.GetProfileIconArray(iconType);
        profileIcon.sprite = imageDataBase.GetProfileIconArray(iconType);

        for (int i = 0; i < iconContentList.Count; i++)
        {
            iconContentList[i].CheckMark(false);
        }

        iconContentList[(int)iconType].CheckMark(true);

        saveLockObject.SetActive(true);

        if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdatePlayerStatisticsInsert("Icon", (int)iconType);

        NotionManager.instance.UseNotion(NotionType.SaveNotion);
    }
}
