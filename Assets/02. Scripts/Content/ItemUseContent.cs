using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUseContent : MonoBehaviour
{
    public ItemType itemType = ItemType.Clock;

    public Image icon;


    public void Initialize(Sprite sprite)
    {
        icon.color = new Color(1, 1, 1, 0.5f);
        icon.sprite = sprite;
    }

    public void UseItem()
    {
        icon.color = new Color(1, 1, 1, 1);
    }


    public void UsedItem()
    {
        icon.color = new Color(1, 1, 1, 0.5f);
    }
}
