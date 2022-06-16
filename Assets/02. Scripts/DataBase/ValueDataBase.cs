using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ValueDataBase", menuName = "ScriptableObjects/ValueDataBase")]
public class ValueDataBase : ScriptableObject
{
    [SerializeField]
    private float adCoolTime = 0;

    [SerializeField]
    private float readyTime = 0;

    [SerializeField]
    private float gamePlayTime = 0;

    [SerializeField]
    private float comboTime = 0;

    [SerializeField]
    private float moleNextTime = 0;

    [SerializeField]
    private float moleCatchTime = 0;

    [SerializeField]
    private float filpCardRememberTime = 0;

    [SerializeField]
    private float clockAddTime = 0;

    public void Initialize()
    {
        adCoolTime = 0;

        readyTime = 0;
        gamePlayTime = 0;
        comboTime = 0;
        moleNextTime = 0;
        moleCatchTime = 0;
        filpCardRememberTime = 0;

        clockAddTime = 0;
    }

    public float AdCoolTime
    {
        get
        {
            return adCoolTime;
        }
        set
        {
            adCoolTime = value;
        }
    }


    public float ReadyTime
    {
        get
        {
            return readyTime;
        }
        set
        {
            readyTime = value;
        }
    }

    public float GamePlayTime
    {
        get
        {
            return gamePlayTime;
        }
        set
        {
            gamePlayTime = value;
        }
    }

    public float ComboTime
    {
        get
        {
            return comboTime;
        }
        set
        {
            comboTime = value;
        }
    }

    public float MoleNextTime
    {
        get
        {
            return moleNextTime;
        }
        set
        {
            moleNextTime = value;
        }
    }

    public float MoleCatchTime
    {
        get
        {
            return moleCatchTime;
        }
        set
        {
            moleCatchTime = value;
        }
    }

    public float FilpCardRememberTime
    {
        get
        {
            return filpCardRememberTime;
        }
        set
        {
            filpCardRememberTime = value;
        }
    }

    public float ClockAddTime
    {
        get
        {
            return clockAddTime;
        }
        set
        {
            clockAddTime = value;
        }
    }
}
