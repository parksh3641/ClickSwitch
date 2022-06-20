using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager instance;

    public GameSettings gameSettings;

    [NonSerialized]
    public const string DEVICESETTINGFILENAME = "DeviceSetting.bin";

    public delegate void ChangeEvent(bool check);
    public static event ChangeEvent eMusic, eSfx;

    [Serializable]
    public class GameSettings
    {
        [Title("Developer")]
        public bool isLogin = false;

        [Space]
        [Title("GuestLogin")]
        public string playfabId = "";
        public string customId = "";
        public bool autoLogin = false;
        public LoginType login = LoginType.None;
        public string nickName = "";

        [Space]
        [Title("Language")]
        public LanguageType language = LanguageType.Default;

        [Space]
        [Title("Setting")]
        public GamePlayType gamePlayType = GamePlayType.GameChoice1;
        public bool music = true;
        public bool sfx = true;
        public bool watchAd = true;

        [Space]
        [Title("Item")]
        public bool clock = false;
        public bool shield = false;
    }

    #region Data

    public bool IsLogin
    {
        get
        {
            return gameSettings.isLogin;
        }
        set
        {
            gameSettings.isLogin = value;
            SaveFile();
        }
    }

    public string PlayfabId
    {
        get
        {
            return gameSettings.playfabId;
        }
        set
        {
            gameSettings.playfabId = value;
            SaveFile();
        }
    }

    public string CustomId
    {
        get
        {
            return gameSettings.customId;
        }
        set
        {
            gameSettings.customId = value;
            SaveFile();
        }
    }

    public string NickName
    {
        get
        {
            return gameSettings.nickName;
        }
        set
        {
            gameSettings.nickName = value;
            SaveFile();
        }
    }

    public LanguageType Language
    {
        get
        {
            return gameSettings.language;
        }
        set
        {
            gameSettings.language = value;
            SaveFile();
        }
    }
    public bool AutoLogin
    {
        get
        {
            return gameSettings.autoLogin;
        }
        set
        {
            gameSettings.autoLogin = value;
            SaveFile();
        }
    }

    public LoginType Login
    {
        get
        {
            return gameSettings.login;
        }
        set
        {
            gameSettings.login = value;
            SaveFile();
        }
    }

    public GamePlayType GamePlayType
    {
        get
        {
            return gameSettings.gamePlayType;
        }
        set
        {
            gameSettings.gamePlayType = value;
            SaveFile();
        }
    }

    public bool Music
    {
        get
        {
            return gameSettings.music;
        }
        set
        {
            gameSettings.music = value;
            eMusic(value);
            SaveFile();
        }
    }

    public bool Sfx
    {
        get
        {
            return gameSettings.sfx;
        }
        set
        {
            gameSettings.sfx = value;
            eSfx(value);
            SaveFile();
        }
    }

    public bool WatchAd
    {
        get
        {
            return gameSettings.watchAd;
        }
        set
        {
            gameSettings.watchAd = value;
            SaveFile();
        }
    }

    public bool Clock
    {
        get
        {
            return gameSettings.clock;
        }
        set
        {
            gameSettings.clock = value;
            SaveFile();
        }
    }

    public bool Shield
    {
        get
        {
            return gameSettings.shield;
        }
        set
        {
            gameSettings.shield = value;
            SaveFile();
        }
    }


    #endregion

    private void Awake()
    {
        instance = this;

        LoadData();
    }
    private void LoadData()
    {
        try
        {
            string stjs = FileIO.LoadData(DEVICESETTINGFILENAME, true);

            if (!string.IsNullOrEmpty(stjs))
            {
                gameSettings = JsonUtility.FromJson<GameSettings>(stjs);

                gameSettings.clock = false;
                gameSettings.shield = false;
            }
            else
            {
                gameSettings = new GameSettings();
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Load Error \n" + e.Message);
        }
    }

    public void SaveFile()
    {
        try
        {
            string str = JsonUtility.ToJson(gameSettings);
            FileIO.SaveData(DEVICESETTINGFILENAME, str, true);
        }
        catch (Exception e)
        {
            Debug.LogError("Save Error \n" + e.Message);
        }
    }
}
