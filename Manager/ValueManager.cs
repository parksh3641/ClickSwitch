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

    public float GetClockAddTime()
    {
        return valueDataBase.ClockAddTime;
    }

    public float GetComboAddTime()
    {
        return valueDataBase.ComboAddTime;
    }

    public float GetDefaultExp()
    {
        return valueDataBase.DefaultExp;
    }

    public float GetAddExp()
    {
        return valueDataBase.AddExp;
    }

    public float GetPerfectClearScore(GamePlayType type)
    {
        float clearScore = 0;

        switch (type)
        {
            case GamePlayType.GameChoice1:
                clearScore = valueDataBase.GameChoice1Perfect;
                break;
            case GamePlayType.GameChoice2:
                clearScore = valueDataBase.GameChoice2Perfect;
                break;
            case GamePlayType.GameChoice3:
                clearScore = valueDataBase.GameChoice3Perfect;
                break;
            case GamePlayType.GameChoice4:
                clearScore = valueDataBase.GameChoice4Perfect;
                break;
            case GamePlayType.GameChoice5:
                clearScore = valueDataBase.GameChoice5Perfect;
                break;
            case GamePlayType.GameChoice6:
                clearScore = valueDataBase.GameChoice6Perfect;
                break;
        }

        return clearScore;
    }

    public float GetNormalClearScore(GamePlayType type)
    {
        float clearScore = 0;

        switch (type)
        {
            case GamePlayType.GameChoice1:
                clearScore = valueDataBase.GameChoice1Normal;
                break;
            case GamePlayType.GameChoice2:
                clearScore = valueDataBase.GameChoice2Normal;
                break;
            case GamePlayType.GameChoice3:
                clearScore = valueDataBase.GameChoice3Normal;
                break;
            case GamePlayType.GameChoice4:
                clearScore = valueDataBase.GameChoice4Normal;
                break;
            case GamePlayType.GameChoice5:
                clearScore = valueDataBase.GameChoice5Normal;
                break;
            case GamePlayType.GameChoice6:
                clearScore = valueDataBase.GameChoice6Normal;
                break;
        }

        return clearScore;
    }

    public float GetHardClearScore(GamePlayType type)
    {
        float clearScore = 0;

        switch (type)
        {
            case GamePlayType.GameChoice1:
                clearScore = valueDataBase.GameChoice1Hard;
                break;
            case GamePlayType.GameChoice2:
                clearScore = valueDataBase.GameChoice2Hard;
                break;
            case GamePlayType.GameChoice3:
                clearScore = valueDataBase.GameChoice3Hard;
                break;
            case GamePlayType.GameChoice4:
                clearScore = valueDataBase.GameChoice4Hard;
                break;
            case GamePlayType.GameChoice5:
                clearScore = valueDataBase.GameChoice5Hard;
                break;
            case GamePlayType.GameChoice6:
                clearScore = valueDataBase.GameChoice6Hard;
                break;
        }

        return clearScore;
    }

}
