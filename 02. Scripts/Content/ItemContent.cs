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

    Sprite[] itemBackgroundArray;
    Sprite[] itemArray;


    private int count = 0;

    ItemManager ItemManager;
    ImageDataBase imageDataBase;


    private void Awake()
    {
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDatabase") as ImageDataBase;

        itemBackgroundArray = imageDataBase.GetItemBackgroundArray();
        itemArray = imageDataBase.GetItemArray();
    }

    public void Initialize(ItemManager manager, ItemType type, int number)
    {
        ItemManager = manager;
        itemType = type;
        count = number;

        background.sprite = itemBackgroundArray[(int)itemType];
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
