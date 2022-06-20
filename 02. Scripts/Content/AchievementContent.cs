using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementContent : MonoBehaviour
{
    public Image mainIcon;
    public Image perfectIcon;

    public Sprite[] iconArray;

    public ImageDataBase imageDataBase;

    private void Awake()
    {
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;

        perfectIcon.gameObject.SetActive(true);

        iconArray = imageDataBase.GetIconArray();
    }

    public void Initialized(GamePlayType type)
    {
        mainIcon.sprite = iconArray[(int)type];
    }

    public void SetPerfectMode(bool check)
    {
        perfectIcon.gameObject.SetActive(check);
    }
}
