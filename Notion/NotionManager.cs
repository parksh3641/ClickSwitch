using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotionManager : MonoBehaviour
{
    public static NotionManager instance;

    [Title("MainNotion")]
    public Notion notion;


    public NotionColor[] notionColor;


    [System.Serializable]
    public class NotionColor
    {
        [Space]
        public NotionType notionType;
        public ColorType colorType;
        [Space]
        public EffectType effectType;
    }
    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        notion.gameObject.SetActive(false);
    }

    void SetColor(ColorType type)
    {
        switch (type)
        {
            case ColorType.White:
                notion.txt.color = new Color(1, 1, 1);
                break;
            case ColorType.Red:
                notion.txt.color = new Color(1, 0, 0);
                break;
            case ColorType.Orange:
                notion.txt.color = new Color(1, 255f / 150f, 0);
                break;
            case ColorType.Yellow:
                notion.txt.color = new Color(1, 1, 0);
                break;
            case ColorType.Green:
                notion.txt.color = new Color(0, 1, 0);
                break;
            case ColorType.SkyBlue:
                notion.txt.color = new Color(0, 1, 1);
                break;
            case ColorType.Blue:
                notion.txt.color = new Color(0, 255f / 150f, 1);
                break;
            case ColorType.Purple:
                notion.txt.color = new Color(255f / 200f, 0, 1);
                break;
            case ColorType.Pink:
                notion.txt.color = new Color(1, 255f / 150f, 1);
                break;
            case ColorType.Black:
                notion.txt.color = new Color(0, 0, 0);
                break;
        }
    }
    public void UseNotion(NotionType type)
    {
        notion.gameObject.SetActive(false);

        foreach(var list in notionColor)
        {
            if (list.notionType.Equals(type))
            {
                notion.txt.text = LocalizationManager.instance.GetString(list.notionType.ToString());
                SetColor(list.colorType);
                SetEffect(list.effectType);
            }
        }    

        notion.gameObject.SetActive(true);
    }

    public void SetEffect(EffectType type)
    {
        switch (type)
        {
            case EffectType.Default:

                break;
            case EffectType.Vibration:
                notion.txt.gameObject.transform.DOPunchPosition(Vector3.left * 50, 0.5f);
                break;
        }
    }

    [Button]
    public void TestNotion()
    {
        UseNotion(NotionType.Test);
    }

}

public enum ColorType
{
    White = 0,
    Red,
    Orange,
    Yellow,
    Green,
    SkyBlue,
    Blue,
    Purple,
    Pink,
    Black
}

public enum EffectType
{
    Default = 0,
    Vibration
}

public enum NotionType
{
    Test,
    NickNameNotion1,
    NickNameNotion2,
    NickNameNotion3,
    NickNameNotion4,
    NickNameNotion5,
    NickNameNotion6,
    NetworkConnectNotion,
    GameModeRockNotion,
    SuccessBuyItem,
    FailBuyItem,
    FailEventTry
}