using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueManager : MonoBehaviour
{
    public static ValueManager instance;

    ValueDataBase valueDataBase;

    private void Awake()
    {
        instance = this;

        if (valueDataBase == null) valueDataBase = Resources.Load("ValueDataBase") as ValueDataBase;
    }

    public float GetAdCoolTime()
    {
        return valueDataBase.AdCoolTime;
    }

    public float GetReadyTime()
    {
        return valueDataBase.ReadyTime;
    }

    public float GetGamePlayTime()
    {
        return valueDataBase.GamePlayTime;
    }

    public float GetComboTime()
    {
        return valueDataBase.ComboTime;
    }

    public float GetMoleNextTime()
    {
        return valueDataBase.MoleNextTime;
    }

    public float GetMoleCatchTime()
    {
        return valueDataBase.MoleCatchTime;
    }

    public float GetFilpCardRememberTime()
    {
        return valueDataBase.FilpCardRememberTime;
    }

    public float GetClockTime()
    {
        return valueDataBase.ClockAddTime;
    }
}
