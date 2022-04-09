using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager instance;

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

    public string GetLocalization(string name)
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

    [Button]
    public void ChangeKorean()
    {
        ChangeLanguage(LanguageType.Korean);
    }

    [Button]
    public void ChangeEnglish()
    {
        ChangeLanguage(LanguageType.English);
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
}
