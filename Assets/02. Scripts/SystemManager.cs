using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SystemManager : MonoBehaviour
{

    void Start()
    {
        if(Application.systemLanguage == SystemLanguage.Korean)
        {
            GameStateManager.instance.Language = LanguageType.Korean;
        }
        else
        {
            GameStateManager.instance.Language = LanguageType.English;
        }

        SceneManager.LoadScene("MainScene");
    }
}
