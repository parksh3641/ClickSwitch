using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewsContent : MonoBehaviour
{
    public int index = 0;

    public NewsManager newsManager;

    [Title("Title")]
    public Text titleText;
    string dateText;
    public void InitState(int number, string title, System.DateTime date, Font font)
    {
        titleText.font = font;

        index = number;

        switch (GameStateManager.instance.Language)
        {
            case LanguageType.Default:
                break;
            case LanguageType.Korean:
                dateText = date.ToString("yyyy/MM/dd");
                break;
            case LanguageType.English:
                dateText = date.ToString("MM/dd/yyyy");
                break;
            case LanguageType.Japanese:
                dateText = date.ToString("yyyy/MM/dd");
                break;
            case LanguageType.Chinese:
                dateText = date.ToString("yyyy/MM/dd");
                break;
            case LanguageType.Indian:
                dateText = date.ToString("dd/MM/yyyy");
                break;
            case LanguageType.Portuguese:
                dateText = date.ToString("dd/MM/yyyy");
                break;
            case LanguageType.Russian:
                dateText = date.ToString("dd/MM/yyyy");
                break;
            case LanguageType.German:
                dateText = date.ToString("dd/MM/yyyy");
                break;
            case LanguageType.Spanish:
                dateText = date.ToString("dd/MM/yyyy");
                break;
            case LanguageType.Arabic:
                dateText = date.ToString("dd/MM/yyyy");
                break;
            case LanguageType.Bengali:
                dateText = date.ToString("dd/MM/yyyy");
                break;
            default:
                dateText = date.ToString("MM/dd/yyyy");
                break;
        }

        titleText.text = title + " (" + dateText + ")";
    }

    public void OpenReadMore()
    {
        newsManager.OpenReadMore(index, titleText.text);
    }
}
