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
    Vibration
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
    Facebook
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
    Click
}

public enum ShopType
{
    RemoveAds = 0,
    Coin1000
}

public enum ItemType
{
    Clock = 0,
    Shield,
    Combo,
    Exp,
    Slow
}

public enum IconType
{
    Default_0,
    Default_1,
    Default_2
}

public enum AdType
{
    CoinX2,
    TryCount
}