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
    GameChoice6,
    GameChoice7,
    GameChoice8
}

public enum OptionType
{
    Music = 0,
    SFX,
    Language,
    Logout,
    Vibration,
    SleepMode,
    RestorePurchases,
    Repair
}

public enum LanguageType
{
    Default = 0,
    Korean,
    English,
    Japanese,
    Chinese,
    Indian,
    Portuguese,
    Russian,
    German,
    Spanish,
    Arabic,
    Bengali,
    Indonesian,
    Italian,
    Dutch
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
    Oha,
    Three,
    Two,
    One,
    Go
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
    DailyShopReward,
    PaidProgress,
    Crystal400,
    Crystal500,
    Crystal600,
    StartPack1,
    StartPack2,
    StartPack3,
    CoinX2,
    ExpX2,
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
    Icon_23,
    Icon_24,
    Icon_25,
    Icon_26,
    Icon_27,
}

public enum BannerType
{
    Banner_0,
    Banner_1,
    Banner_2,
    Banner_3,
    Banner_4,
    Banner_5,
    Banner_6,
    Banner_7,
    Banner_8,
    Banner_9,
    Banner_10,
    Banner_11,
    Banner_12,
    Banner_13,
    Banner_14,
    Banner_15,
    Banner_16,
    Banner_17,
    Banner_18,
    Banner_19,
    Banner_20,
    Banner_21,
}

public enum AdType
{
    CoinX2,
    TryCount,
    ShopWatchAd,
    CoinRushTryCount,
    DailyMissonX2,
    Item,
}

public enum DailyMissionType
{
    QuestDoPlay,
    QuestScore,
    QuestCombo
}

public enum WeeklyMissionType
{
    GetScore,
    GetCombo,
    UseItem,
    GamePlay,
    DailyMissonClear,
    ChallengeCoinRush,
}

public enum AchievementType
{
    Login,
    TotalScore,
    TotalCombo,
    GamePlay,
    OpenBox,
    UserLevel,
    UseCrystal,
    UseItem,
    Upgrade,
    DailyMissonClear,
    ChallengeCoinRush
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
    IconBox,
    Icon,
    Banner,
}

public enum UpgradeType
{
    StartTime,
    Critical,
    Burning,
    AddExp,
    AddGold,
    ComboTime,
    ComboCritical,
    AddScore,
}

public enum RewardReceiveType
{
    Free,
    Paid
}