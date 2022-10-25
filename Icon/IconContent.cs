using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconContent : MonoBehaviour
{
    public IconType iconType;
    IconClass iconClass;

    public Image icon;

    public Image fillAmount;
    public Text countText;

    public int count = 0;



    public GameObject lockObject;
    public GameObject checkMark;

    IconManager iconManager;

    ShopDataBase shopDataBase;
    ImageDataBase imageDataBase;

    private void Awake()
    {
        if (shopDataBase == null) shopDataBase = Resources.Load("ShopDataBase") as ShopDataBase;
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;

        lockObject.SetActive(true);
        checkMark.SetActive(false);

        fillAmount.fillAmount = 0f;
    }

    public void Initialize(IconManager manager, IconType type)
    {
        iconManager = manager;
        iconType = type;

        icon.sprite = imageDataBase.GetProfileIconArray(iconType);

        InitState();
    }

    public void InitState()
    {
        iconClass = shopDataBase.GetIconState(iconType);

        if((int)iconClass.iconType >= 19)
        {
            if (iconClass.count >= 1)
            {
                UnLock();
            }
            else
            {
                fillAmount.fillAmount = iconClass.count / 1.0f;
                countText.text = iconClass.count + " / 1";
            }

        }
        else
        {
            if (iconClass.count >= 5)
            {
                UnLock();
            }
            else
            {
                fillAmount.fillAmount = iconClass.count / 5.0f;
                countText.text = iconClass.count + " / 5";
            }
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
        if(!lockObject.activeSelf)
        {
            iconManager.UseIcon(iconType);
        }
    }
}
