using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventModeContent : ModeContent
{
    public LocalizationContent infomation;


    public void SetClearInformation(GamePlayType type, GameModeType mode)
    {
        infomation.name = type.ToString() + mode.ToString();
        infomation.ReLoad();
    }

}
