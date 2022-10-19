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
        public bool appleInAppPurchase = false;

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
        public bool playGame = false;
        public GamePlayType gamePlayType = GamePlayType.GameChoice1;
        public GameModeType gameModeType = GameModeType.Easy;
        public bool music = true;
        public bool sfx = true;
        public bool vibration = true;
        public bool sleepMode = false;

        [Space]
        [Title("Item")]
        public bool clock = false;
        public bool shield = false;
        public bool combo = false;
        public bool exp = false;
        public bool slow = false;

        [Space]
        [Title("Game Event")]
        public bool fail = false;
        public int tryCount = 2;

        [Space]
        [Title("Ad")]
        public bool watchAd = true;
        public bool eventWatchAd = false;

        [Space]
        [Title("Daily")]
        public bool dailyReward = false;
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

    public bool AppleInAppPurchase
    {
        get
        {
            return gameSettings.appleInAppPurchase;
        }
        set
        {
            gameSettings.appleInAppPurchase = value;
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

    public bool PlayGame
    {
        get
        {
            return gameSettings.playGame;
        }
        set
        {
            gameSettings.playGame = value;
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

    public GameModeType GameModeType
    {
        get
        {
            return gameSettings.gameModeType;
        }
        set
        {
            gameSettings.gameModeType = value;
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

    public bool Vibration
    {
        get
        {
            return gameSettings.vibration;
        }
        set
        {
            gameSettings.vibration = value;
            SaveFile();
        }
    }

    public bool SleepMode
    {
        get
        {
            return gameSettings.sleepMode;
        }
        set
        {
            gameSettings.sleepMode = value;
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

    public bool Combo
    {
        get
        {
            return gameSettings.combo;
        }
        set
        {
            gameSettings.combo = value;
            SaveFile();
        }
    }

    public bool Exp
    {
        get
        {
            return gameSettings.exp;
        }
        set
        {
            gameSettings.exp = value;
            SaveFile();
        }
    }

    public bool Slow
    {
        get
        {
            return gameSettings.slow;
        }
        set
        {
            gameSettings.slow = value;
            SaveFile();
        }
    }

    public int TryCount
    {
        get
        {
            return gameSettings.tryCount;
        }
        set
        {
            gameSettings.tryCount = value;
            SaveFile();
        }
    }

    public bool EventWatchAd
    {
        get
        {
            return gameSettings.eventWatchAd;
        }
        set
        {
            gameSettings.eventWatchAd = value;
            SaveFile();
        }
    }

    public bool DailyReward
    {
        get
        {
            return gameSettings.dailyReward;
        }
        set
        {
            gameSettings.dailyReward = value;
            SaveFile();
        }
    }

    public bool Fail
    {
        get
        {
            return gameSettings.fail;
        }
        set
        {
            gameSettings.fail = value;
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
