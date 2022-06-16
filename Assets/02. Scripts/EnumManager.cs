using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnumManager : MonoBehaviour
{

}

public enum GameModeType
{
    Normal,
    Hard,
    Expert,
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
    Logout
}

public enum LanguageType
{
    Korean = 0,
    English,
    Japenese,
    Chinese
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
    Success
}

public enum ShopType
{
    RemoveAds = 0,
    Coin1000
}

public enum ItemType
{
    Clock = 0,
    Shield
}