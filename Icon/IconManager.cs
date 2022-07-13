using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconManager : MonoBehaviour
{
    public GameObject iconView;

    public IconContent iconContent;
    public RectTransform iconContentTransform;


    public Image profileIcon;


    public List<IconContent> iconContentList = new List<IconContent>();

    public ImageDataBase imageDataBase;
    public PlayerDataBase playerDataBase;

    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;

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

        iconContentTransform.anchoredPosition = new Vector2(0, -999);
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
                else
                {

                }
            }

            iconType++;
        }

        iconType = IconType.Icon_0;

        profileIcon.sprite = imageDataBase.GetProfileIconArray(iconType + playerDataBase.Icon);
    }

    public void UseIcon(IconType type)
    {
        profileIcon.sprite = imageDataBase.GetProfileIconArray(type);

        playerDataBase.Icon = (int)type;

        if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdatePlayerStatisticsInsert("Icon", (int)type);

        CheckMyIcon();
    }
}
