using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MailContent : MonoBehaviour
{
    public UnityEvent unityEvent;

    public Image backgroundImg;
    public Image icon;
    public Text numberText;

    Sprite[] vcArray;
    Sprite[] itemArray;
    Sprite[] etcArray;
    Sprite[] rankArray;

    ItemType itemType = ItemType.Clock;

    ImageDataBase imageDataBase;

    private void Awake()
    {
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;

        vcArray = imageDataBase.GetVCArray();
        itemArray = imageDataBase.GetItemArray();
        etcArray = imageDataBase.GetETCArray();
        rankArray = imageDataBase.GetRankArray();
    }


    public void Initialize(string name)
    {
        if(name.Contains("Coin"))
        {
            icon.sprite = vcArray[0];
            backgroundImg.sprite = rankArray[0];
        }
        else if(name.Contains("Crystal"))
        {
            icon.sprite = vcArray[1];
            backgroundImg.sprite = rankArray[1];
        }
        else if(name.Contains("IconBox"))
        {
            icon.sprite = etcArray[0];
            backgroundImg.sprite = rankArray[1];
        }
        else
        {
            for(int i = 0; i < System.Enum.GetValues(typeof(ItemType)).Length; i ++)
            {
                if(name.Contains((itemType + i).ToString()))
                {
                    icon.sprite = itemArray[(int)itemType + i];
                }
            }
        }

        string[] strArray = name.Split('_');

        numberText.text = "x" + strArray[1];
    }

    public void OnClick()
    {
        unityEvent.Invoke();

        gameObject.SetActive(false);
    }
}
