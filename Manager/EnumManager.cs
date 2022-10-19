using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnumManager : MonoBehaviour
{

}

public enum GameModeType
{
    Easy,
    Normal,
    Hard,
    Perfect
}
public enum GamePlayType
{
    GameChoice1 = 0,
    GameChoice2,
    GameChoice3,
    GameChoice4,
    GameChoice5,
    GameChoice6
}

public enum OptionType
{
    Music = 0,
    SFX,
    Language,
    Logout,
    Vibration,
    SleepMode,
    RestorePurchases
}

public enum LanguageType
{
    Default = 0,
    Korean,
    English,
    Japenese,
    Chinese,
    Indian,
    Portuguese,
    Russian,
    German,
    Spanish,
    Arabic,
    Bengali
}

public enum LoginType
{
    None = 0,
    Guest,
    Google,
    Facebook,
    Apple
}

public enum MoneyType
{
    Coin = 0,
    Crystal
}

public enum GameBGMType
{
    Lobby = 0,
    Main,
    End
}

public enum GameSfxType
{
    Success,
    Shield,
    LevelUp,
    Fail,
    Click,
    GetMoney,
    BuyItem,
    BoxOpen,
    GameOver,
    Oha
}

public enum ShopType
{
    RemoveAds = 0,
    WatchAd,
    Coin1000,
    Coin2000,
    Coin3000,
    Crystal100,
    Crystal200,
    Crystal300,
    DailyReward,
    PaidReward
}

public enum ItemType
{
    Clock = 0,
    Shield,
    Combo,
    Exp,
    Slow
}

public enum ETCType
{
    IconBox = 0,
}

public enum IconType
{
    Icon_0,
    Icon_1,
    Icon_2,
    Icon_3,
    Icon_4,
    Icon_5,
    Icon_6,
    Icon_7,
    Icon_8,
    Icon_9,
    Icon_10,
    Icon_11,
    Icon_12,
    Icon_13,
    Icon_14,
    Icon_15,
    Icon_16,
    Icon_17,
    Icon_18,
    Icon_19,
    Icon_20,
    Icon_21,
    Icon_22,
}

public enum AdType
{
    CoinX2,
    TryCount,
    ShopWatchAd
}

public enum MissionType
{
    QuestDoPlay,
    QuestScore,
    QuestCombo
}

public enum RewardType
{
    Coin = 0,
    Crystal,
    Clock,
    Shield,
    Combo,
    Exp,
    Slow,
    IconBox
}

public enum UpgradeType
{
    StartTime,
    Critical,
    Burning,
    AddExp
}