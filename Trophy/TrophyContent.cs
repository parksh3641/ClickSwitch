using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrophyContent : MonoBehaviour
{
    public GamePlayType gamePlayType = GamePlayType.GameChoice1;

    TrophyData trophyData = new TrophyData();

    public Image icon;

    public GameObject information;
    public GameObject locked;

    [Title("Text")]
    public LocalizationContent titleText;
    public Text numberText;
    public Text dateText;


    Sprite[] iconArray;

    PlayerDataBase playerDataBase;
    ImageDataBase imageDataBase;

    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;

        iconArray = imageDataBase.GetIconArray();

        numberText.text = "";
        dateText.text = "";

        locked.SetActive(true);
        information.SetActive(false);
    }

    public void Initialize(GamePlayType type)
    {
        gamePlayType = type;

        titleText.name = type.ToString();
        titleText.ReLoad();

        icon.sprite = iconArray[(int)gamePlayType];

        trophyData = playerDataBase.GetTrophyData(type);

        locked.SetActive(!trophyData.isActive);

        numberText.text = trophyData.number.ToString();
        dateText.text = trophyData.date.ToString();
    }

    public void OpenInformation()
    {
        if (locked.activeInHierarchy) return;

        if(!information.activeInHierarchy)
        {
            information.SetActive(true);
        }
        else
        {
            information.SetActive(false);
        }
    }
}
