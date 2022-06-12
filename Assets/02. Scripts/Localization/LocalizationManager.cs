using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager instance;

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

    public void ChangeLanguage(LanguageType type)
    {
        Debug.Log("Change Language : " + type);
        GameStateManager.instance.Language = type;

        for(int i = 0; i < localizationContentList.Count; i ++)
        {
            localizationContentList[i].ReLoad();
        }
    }

    public void CloseLanguage()
    {
        eChangeLanguage.Invoke();
    }
}
