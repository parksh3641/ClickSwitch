using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class OptionContent : MonoBehaviour
{
    public OptionType optionType;

    public UnityEvent eChangeLanguage;
    public UnityEvent eLogout;

    public Image iconImg;
    public Text iconText;

    public GameObject button;
    public Image buttonImg;
    public LocalizationContent buttonText;

    [Title("SpriteList")]
    public Sprite[] iconList;
    public Sprite[] buttonList;
    Sprite[] languageList;
    public Sprite[] loginList;

    ImageDataBase imageDataBase;

    private void Awake()
    {
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;

        languageList = imageDataBase.GetCountryArray();
    }

    private void Start()
    {
        InitState();
    }

    public void InitState()
    {
        switch (optionType)
        {
            case OptionType.Music:
                OnBGM();
                break;
            case OptionType.SFX:
                OnSFX();
                break;
            case OptionType.Language:
                iconImg.sprite = languageList[(int)GameStateManager.instance.Language - 1];
                iconText.text = GameStateManager.instance.Language.ToString();

                buttonImg.sprite = buttonList[0];

                buttonText.name = "Change";
                buttonText.ReLoad();
                buttonText.TextColor(new Color(39 / 255f, 220 / 255f, 149 / 255f));

                break;
            case OptionType.Logout:
                iconImg.sprite = loginList[(int)GameStateManager.instance.Login - 1];
                iconText.text = GameStateManager.instance.Login.ToString();

                buttonImg.sprite = buttonList[1];

                buttonText.name = "Logout";
                buttonText.ReLoad();
                buttonText.TextColor(new Color(225 / 255f, 34 / 255f, 12 / 255f));

                if (GameStateManager.instance.Login == LoginType.Google)
                {
                    button.SetActive(true);
                }
                else
                {
                    button.SetActive(false);
                }
                break;
            case OptionType.Vibration:
                buttonText.name = "Vibration";
                buttonText.ReLoad();

                OnVibration();
                break;
        }
    }

    public void OnClick()
    {
        switch (optionType)
        {
            case OptionType.Music:
                if (GameStateManager.instance.Music)
                {
                    GameStateManager.instance.Music = false;
                }
                else
                {
                    GameStateManager.instance.Music = true;
                }

                OnBGM();
                break;
            case OptionType.SFX:
                if (GameStateManager.instance.Sfx)
                {
                    GameStateManager.instance.Sfx = false;
                }
                else
                {
                    GameStateManager.instance.Sfx = true;
                }

                OnSFX();
                break;
            case OptionType.Language:
                eChangeLanguage.Invoke();

                break;
            case OptionType.Logout:
                eLogout.Invoke();

                break;
            case OptionType.Vibration:
                if (GameStateManager.instance.Vibration)
                {
                    GameStateManager.instance.Vibration = false;
                }
                else
                {
                    GameStateManager.instance.Vibration = true;
                }

                OnVibration();
                break;
        }
    }

    public void OnBGM()
    {
        if (GameStateManager.instance.Music)
        {
            iconImg.sprite = iconList[0];
            buttonImg.sprite = buttonList[0];
            buttonText.name = "ON";
            buttonText.ReLoad();
            buttonText.TextColor(new Color(39 / 255f, 220 / 255f, 149 / 255f));

        }
        else
        {
            iconImg.sprite = iconList[1];
            buttonImg.sprite = buttonList[1];
            buttonText.name = "OFF";
            buttonText.ReLoad();
            buttonText.TextColor(new Color(225 / 255f, 34 / 255f, 12 / 255f));
        }
    }

    public void OnSFX()
    {
        if (GameStateManager.instance.Sfx)
        {
            iconImg.sprite = iconList[2];
            buttonImg.sprite = buttonList[0];
            buttonText.name = "ON";
            buttonText.ReLoad();
            buttonText.TextColor(new Color(39 / 255f, 220 / 255f, 149 / 255f));
        }
        else
        {
            iconImg.sprite = iconList[3];
            buttonImg.sprite = buttonList[1];
            buttonText.name = "OFF";
            buttonText.ReLoad();
            buttonText.TextColor(new Color(225 / 255f, 34 / 255f, 12 / 255f));
        }
    }

    public void OnVibration()
    {
        if (GameStateManager.instance.Vibration)
        {
            iconImg.sprite = iconList[4];
            buttonImg.sprite = buttonList[0];
            buttonText.name = "ON";
            buttonText.ReLoad();
            buttonText.TextColor(new Color(39 / 255f, 220 / 255f, 149 / 255f));
        }
        else
        {
            iconImg.sprite = iconList[4];
            buttonImg.sprite = buttonList[1];
            buttonText.name = "OFF";
            buttonText.ReLoad();
            buttonText.TextColor(new Color(225 / 255f, 34 / 255f, 12 / 255f));
        }
    }
}
