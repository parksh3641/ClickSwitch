using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager instance;

    public Font normalFont;
    public Font bengaliFont;

    public UnityEvent eChangeLanguage;

    public LocalizationDataBase localizationDataBase;

    public List<LocalizationContent> localizationContentList = new List<LocalizationContent>();


    private void Awake()
    {
        instance = this;

        localizationContentList.Clear();
    }

    public void AddContent(LocalizationContent content)
    {
        localizationContentList.Add(content);
    }

    public string GetString(string name)
    {
        string str = "";

        foreach (var item in localizationDataBase.localizationDatas)
        {
            if(name.Equals(item.key))
            {
                switch (GameStateManager.instance.Language)
                {
                    case LanguageType.Korean:
                        str = item.korean;
                        break;
                    case LanguageType.English:
                        str = item.english;
                        break;
                    case LanguageType.Japenese:
                        str = item.japanese;
                        break;
                    case LanguageType.Chinese:
                        str = item.chinese;
                        break;
                    case LanguageType.Indian:
                        str = item.indonesian;
                        break;
                    case LanguageType.Portuguese:
                        str = item.portuguese;
                        break;
                    case LanguageType.Russian:
                        str = item.russian;
                        break;
                    case LanguageType.German:
                        str = item.german;
                        break;
                    case LanguageType.Spanish:
                        str = item.spanish;
                        break;
                    case LanguageType.Arabic:
                        str = item.arabic;
                        break;
                    case LanguageType.Bengali:
                        str = item.bengali;
                        break;
                }
            }
        }

        if(str.Length == 0)
        {
            str = name;
        }

        return str;
    }

    public void ChangeKorean()
    {
        ChangeLanguage(LanguageType.Korean);

        eChangeLanguage.Invoke();
    }

    public void ChangeEnglish()
    {
        ChangeLanguage(LanguageType.English);

        eChangeLanguage.Invoke();
    }

    public void ChangeJapanese()
    {
        ChangeLanguage(LanguageType.Japenese);

        eChangeLanguage.Invoke();
    }

    public void ChangeChinese()
    {
        ChangeLanguage(LanguageType.Chinese);

        eChangeLanguage.Invoke();
    }

    public void ChangeIndian()
    {
        ChangeLanguage(LanguageType.Indian);

        eChangeLanguage.Invoke();
    }

    public void ChangePortuguese()
    {
        ChangeLanguage(LanguageType.Portuguese);

        eChangeLanguage.Invoke();
    }

    public void ChangeRussian()
    {
        ChangeLanguage(LanguageType.Russian);

        eChangeLanguage.Invoke();
    }

    public void ChangeGerman()
    {
        ChangeLanguage(LanguageType.German);

        eChangeLanguage.Invoke();
    }
    public void ChangeSpanish()
    {
        ChangeLanguage(LanguageType.Spanish);

        eChangeLanguage.Invoke();
    }
    public void ChangeArabic()
    {
        ChangeLanguage(LanguageType.Arabic);

        eChangeLanguage.Invoke();
    }
    public void ChangeBengali()
    {
        ChangeLanguage(LanguageType.Bengali);

        eChangeLanguage.Invoke();
    }

    public void ChangeLanguage(LanguageType type)
    {
        Debug.Log("Change Language : " + type);
        GameStateManager.instance.Language = type;

        string iso = "";

        switch (type)
        {
            case LanguageType.Default:
                break;
            case LanguageType.Korean:
                iso = "ko";
                break;
            case LanguageType.English:
                iso = "en";
                break;
            case LanguageType.Japenese:
                iso = "ja";
                break;
            case LanguageType.Chinese:
                iso = "en";
                break;
            case LanguageType.Indian:
                iso = "hi";
                break;
            case LanguageType.Portuguese:
                iso = "pt";
                break;
            case LanguageType.Russian:
                iso = "ru";
                break;
            case LanguageType.German:
                iso = "de";
                break;
            case LanguageType.Spanish:
                iso = "es";
                break;
            case LanguageType.Arabic:
                iso = "ar";
                break;
            case LanguageType.Bengali:
                iso = "bn";
                break;
            default:
                iso = "en";
                break;
        }

        if (PlayfabManager.instance.isActive) PlayfabManager.instance.SetProfileLanguage(iso);

        for(int i = 0; i < localizationContentList.Count; i ++)
        {
            if (type == LanguageType.Bengali)
            {
                localizationContentList[i].GetComponent<Text>().font = bengaliFont;
            }
            else
            {
                localizationContentList[i].GetComponent<Text>().font = normalFont;
            }

            localizationContentList[i].ReLoad();
        }
    }

    public void CloseLanguage()
    {
        eChangeLanguage.Invoke();
    }
}
