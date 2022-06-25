using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconContent : MonoBehaviour
{
    IconType iconType;

    public Image icon;

    public GameObject lockObject;

    IconManager iconManager;

    public ImageDataBase imageDataBase;

    private void Awake()
    {
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;

        lockObject.SetActive(true);
    }

    public void UnLock(IconManager manager,IconType type)
    {
        iconManager = manager;
        iconType = type;

        icon.sprite = imageDataBase.GetProfileIconArray(type);

        lockObject.SetActive(false);
    }

    public void OnClick()
    {
        if(!lockObject.activeSelf)
        {
            iconManager.UseIcon(iconType);
        }
    }
}
