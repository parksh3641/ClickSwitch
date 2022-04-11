using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnumManager : MonoBehaviour
{

}

public enum GamePlayType
{
    Normal = 0
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
    English
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
    Gold = 0,
    Crystal
}