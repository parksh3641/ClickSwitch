using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SystemManager : MonoBehaviour
{
    public GoogleSheetDownloader googleSheetDownloader;


    void Start()
    {
        if(Application.systemLanguage == SystemLanguage.Korean)
        {
            GameStateManager.instance.Language = LanguageType.Korean;
        }
        else if(Application.systemLanguage == SystemLanguage.Japanese)
        {
            GameStateManager.instance.Language = LanguageType.Japenese;
        }
        else if (Application.systemLanguage == SystemLanguage.Chinese)
        {
            GameStateManager.instance.Language = LanguageType.Chinese;
        }
        else
        {
            GameStateManager.instance.Language = LanguageType.English;
        }

        StartCoroutine(WaitCorution());
    }

    IEnumerator WaitCorution()
    {
        while(!googleSheetDownloader.isActive)
        {
            yield return new WaitForSeconds(0.01f);
        }

        SceneManager.LoadScene("MainScene");
    }
}
