using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconContent : MonoBehaviour
{
    IconType iconType;

    public Image icon;

    public GameObject lockObject;

    public GameObject checkMark;

    IconManager iconManager;

    public ImageDataBase imageDataBase;

    private void Awake()
    {
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;

        lockObject.SetActive(true);
        checkMark.SetActive(false);
    }

    public void Initialize(IconManager manager,IconType type)
    {
        iconManager = manager;
        iconType = type;

        icon.sprite = imageDataBase.GetProfileIconArray(type);
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
