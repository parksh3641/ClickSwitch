using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconManager : MonoBehaviour
{
    public GameObject iconView;

    public IconContent iconContent;
    public Transform iconContentTransform;


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
        IconType iconType = IconType.Default_0;

        for(int i = 0; i < iconContentList.Count; i ++)
        {
            if (imageDataBase.GetProfileIconArray(iconType))
            {
                iconContentList[i].UnLock(this, iconType);
            }

            iconType++;
        }

        iconType = IconType.Default_0;

        profileIcon.sprite = imageDataBase.GetProfileIconArray(iconType + playerDataBase.Icon);
    }

    public void UseIcon(IconType type)
    {
        profileIcon.sprite = imageDataBase.GetProfileIconArray(type);

        playerDataBase.Icon = (int)type;

        if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdatePlayerStatisticsInsert("Icon", (int)type);

        OpenIcon();
    }
}
