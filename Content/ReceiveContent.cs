using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReceiveContent : MonoBehaviour
{
    public RewardType rewardType = RewardType.Coin;

    public Image mainBackground;
    public Image icon;
    public Text countText;

    ImageDataBase imageDataBase;

    Sprite[] rankArray;
    Sprite[] vcArray;
    Sprite[] itemArray;
    Sprite[] etcArray;

    private void Awake()
    {
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;

        rankArray = imageDataBase.GetRankArray();
        vcArray = imageDataBase.GetVCArray();
        itemArray = imageDataBase.GetItemArray();
        etcArray = imageDataBase.GetETCArray();
    }

    public void Initialize(RewardType type, int count)
    {
        switch (type)
        {
            case RewardType.Coin:
                icon.sprite = vcArray[0];
                mainBackground.sprite = rankArray[0];

                break;
            case RewardType.Crystal:
                icon.sprite = vcArray[1];
                mainBackground.sprite = rankArray[1];

                break;
            case RewardType.Clock:
                icon.sprite = itemArray[0];
                mainBackground.sprite = rankArray[0];

                break;
            case RewardType.Shield:
                icon.sprite = itemArray[1];
                mainBackground.sprite = rankArray[0];

                break;
            case RewardType.Combo:
                icon.sprite = itemArray[2];
                mainBackground.sprite = rankArray[0];

                break;
            case RewardType.Exp:
                icon.sprite = itemArray[3];
                mainBackground.sprite = rankArray[1];

                break;
            case RewardType.Slow:
                icon.sprite = itemArray[4];
                mainBackground.sprite = rankArray[1];

                break;
            case RewardType.IconBox:
                icon.sprite = etcArray[0];
                mainBackground.sprite = rankArray[1];

                break;
            case RewardType.Experience:
                icon.sprite = etcArray[1];
                mainBackground.sprite = rankArray[1];
                break;
        }

        countText.text = "x" + count.ToString();
    }
}
