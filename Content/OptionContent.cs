using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionContent : MonoBehaviour
{
    public OptionType optionType;

    public UnityEvent eChangeLanguage;
    public UnityEvent eLogout;
    public UnityEvent eGoogleLink;
    public UnityEvent eAppleLink;
    public UnityEvent eFacebookLogout;
    public UnityEvent eRestorePurchases;
    public UnityEvent eRepair;

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

                buttonText.localizationName = "Change";
                buttonText.ReLoad();

                break;
            case OptionType.Logout:
                iconImg.sprite = loginList[(int)GameStateManager.instance.Login - 1];
                iconText.text = GameStateManager.instance.Login.ToString();

                buttonImg.sprite = buttonList[1];

                buttonText.localizationName = "DeleteAccount";
                buttonText.ReLoad();

                button.SetActive(true);

                switch (GameStateManager.instance.Login)
                {
                    case LoginType.None:
                        break;
                    case LoginType.Guest:

                        buttonImg.sprite = buttonList[0];

                        switch (Application.platform)
                        {
                            case RuntimePlatform.Android:
                                buttonText.localizationName = "GoogleLink";
                                break;
                            case RuntimePlatform.IPhonePlayer:
                                buttonText.localizationName = "AppleLink";
                                break;
                            default:
                                button.SetActive(false);
                                break;
                        }
                        buttonText.ReLoad();

                        break;
                    case LoginType.Google:
                        break;
                    case LoginType.Facebook:
                        break;
                    case LoginType.Apple:
                        break;
                }

                break;
            case OptionType.Vibration:
                OnVibration();
                break;
            case OptionType.SleepMode:
                OnSleepMode();
                break;
            case OptionType.RestorePurchases:
                iconImg.sprite = iconList[6];

                buttonText.localizationName = "RestorePurchases";
                buttonText.ReLoad();
                break;
            case OptionType.Repair:
                iconImg.sprite = iconList[7];

                buttonText.localizationName = "Confirm";
                buttonText.ReLoad();
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

                switch (GameStateManager.instance.Login)
                {
                    case LoginType.None:
                        break;
                    case LoginType.Guest:
                        switch (Application.platform)
                        {
                            case RuntimePlatform.Android:
                                eGoogleLink.Invoke();
                                break;
                            case RuntimePlatform.IPhonePlayer:
                                eAppleLink.Invoke();
                                break;
                            default:
                                eLogout.Invoke();
                                break;
                        }
                        break;
                    case LoginType.Google:
                        eLogout.Invoke();
                        break;
                    case LoginType.Facebook:
                        eFacebookLogout.Invoke();
                        break;
                    case LoginType.Apple:
                        eLogout.Invoke();
                        break;
                }

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
            case OptionType.SleepMode:
                if (GameStateManager.instance.SleepMode)
                {
                    GameStateManager.instance.SleepMode = false;

                    Application.targetFrameRate = 60;
                }
                else
                {
                    GameStateManager.instance.SleepMode = true;

                    Application.targetFrameRate = 30;
                }

                OnSleepMode();
                break;
            case OptionType.RestorePurchases:
                eRestorePurchases.Invoke();
                break;
            case OptionType.Repair:
                eRepair.Invoke();             
                break;
        }
    }

    public void OnBGM()
    {
        if (GameStateManager.instance.Music)
        {
            iconImg.sprite = iconList[0];
            buttonImg.sprite = buttonList[0];
            buttonText.localizationName = "ON";
            buttonText.ReLoad();

        }
        else
        {
            iconImg.sprite = iconList[1];
            buttonImg.sprite = buttonList[1];
            buttonText.localizationName = "OFF";
            buttonText.ReLoad();
        }
    }

    public void OnSFX()
    {
        if (GameStateManager.instance.Sfx)
        {
            iconImg.sprite = iconList[2];
            buttonImg.sprite = buttonList[0];
            buttonText.localizationName = "ON";
            buttonText.ReLoad();
        }
        else
        {
            iconImg.sprite = iconList[3];
            buttonImg.sprite = buttonList[1];
            buttonText.localizationName = "OFF";
            buttonText.ReLoad();
        }
    }

    public void OnVibration()
    {
        if (GameStateManager.instance.Vibration)
        {
            iconImg.sprite = iconList[4];
            buttonImg.sprite = buttonList[0];
            buttonText.localizationName = "ON";
            buttonText.ReLoad();
        }
        else
        {
            iconImg.sprite = iconList[4];
            buttonImg.sprite = buttonList[1];
            buttonText.localizationName = "OFF";
            buttonText.ReLoad();
        }
    }

    public void OnSleepMode()
    {
        if (GameStateManager.instance.SleepMode)
        {
            iconImg.sprite = iconList[5];
            buttonImg.sprite = buttonList[0];
            buttonText.localizationName = "ON";
            buttonText.ReLoad();
        }
        else
        {
            iconImg.sprite = iconList[5];
            buttonImg.sprite = buttonList[1];
            buttonText.localizationName = "OFF";
            buttonText.ReLoad();
        }
    }

    public void SuccessLink(LoginType type)
    {
        iconImg.sprite = loginList[(int)GameStateManager.instance.Login - 1];
        iconText.text = GameStateManager.instance.Login.ToString();

        buttonImg.sprite = buttonList[1];

        buttonText.localizationName = "DeleteAccount";
        buttonText.ReLoad();
    }
}
