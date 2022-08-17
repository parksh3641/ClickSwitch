using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconBoxContent : MonoBehaviour
{
    public Image icon;
    public Text countText;

    private int count = 0;

    ImageDataBase imageDataBase;

    private void Awake()
    {
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;
    }

    private void OnDisable()
    {
        count = 0;
        countText.text = "x" + count.ToString();
    }

    public void Initialize(IconType type)
    {
        icon.sprite = imageDataBase.GetProfileIconArray(type);
    }

    public void AddCount()
    {
        count++;

        countText.text = "x" + count.ToString();
    }
}
