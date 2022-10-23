using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemContent : MonoBehaviour
{
    public ItemType itemType = ItemType.Clock;

    public Image background;
    public Image icon;
    public TextMeshProUGUI countText;

    public GameObject frame;

    Sprite[] rankArray;
    Sprite[] itemArray;


    private int count = 0;

    ItemManager ItemManager;
    ImageDataBase imageDataBase;


    private void Awake()
    {
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDatabase") as ImageDataBase;

        rankArray = imageDataBase.GetRankArray();
        itemArray = imageDataBase.GetItemArray();
    }

    public void Initialize(ItemManager manager, ItemType type, int number)
    {
        ItemManager = manager;
        itemType = type;
        count = number;

        switch (type)
        {
            case ItemType.Clock:
                background.sprite = rankArray[0];
                break;
            case ItemType.Shield:
                background.sprite = rankArray[0];
                break;
            case ItemType.Combo:
                background.sprite = rankArray[0];
                break;
            case ItemType.Exp:
                background.sprite = rankArray[1];
                break;
            case ItemType.Slow:
                background.sprite = rankArray[1];
                break;
        }
        icon.sprite = itemArray[(int)itemType];
        countText.text = count.ToString();

        frame.SetActive(false);
    }

    public void OnClick()
    {
        if (count <= 0) return;

        ItemManager.UseItem(itemType);

        if(frame.activeInHierarchy)
        {
            frame.SetActive(false);
        }
        else
        {
            frame.SetActive(true);
        }
    }

}
