using Sirenix.OdinInspector;
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

    [SerializeField]
    private float comboAddTime = 0;

    [SerializeField]
    private float defaultExp = 0;

    [SerializeField]
    private float addExp = 0;

    [Space]
    [Title("Perfect")]
    [SerializeField]
    private float gameChoice1Perfect = 0;
    [SerializeField]
    private float gameChoice2Perfect = 0;
    [SerializeField]
    private float gameChoice3Perfect = 0;
    [SerializeField]
    private float gameChoice4Perfect = 0;
    [SerializeField]
    private float gameChoice5Perfect = 0;
    [SerializeField]
    private float gameChoice6Perfect = 0;

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
        comboAddTime = 0;

        defaultExp = 0;
        addExp = 0;

        gameChoice1Perfect = 0;
        gameChoice2Perfect = 0;
        gameChoice3Perfect = 0;
        gameChoice4Perfect = 0;
        gameChoice5Perfect = 0;
        gameChoice6Perfect = 0;
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

    public float ComboAddTime
    {
        get
        {
            return comboAddTime;
        }
        set
        {
            comboAddTime = value;
        }
    }

    public float DefaultExp
    {
        get
        {
            return defaultExp;
        }
        set
        {
            defaultExp = value;
        }
    }

    public float AddExp
    {
        get
        {
            return addExp;
        }
        set
        {
            addExp = value;
        }
    }

    public float GameChoice1Perfect
    {
        get
        {
            return gameChoice1Perfect;
        }
        set
        {
            gameChoice1Perfect = value;
        }
    }

    public float GameChoice2Perfect
    {
        get
        {
            return gameChoice2Perfect;
        }
        set
        {
            gameChoice2Perfect = value;
        }
    }

    public float GameChoice3Perfect
    {
        get
        {
            return gameChoice3Perfect;
        }
        set
        {
            gameChoice3Perfect = value;
        }
    }

    public float GameChoice4Perfect
    {
        get
        {
            return gameChoice4Perfect;
        }
        set
        {
            gameChoice5Perfect = value;
        }
    }

    public float GameChoice5Perfect
    {
        get
        {
            return gameChoice5Perfect;
        }
        set
        {
            gameChoice6Perfect = value;
        }
    }

    public float GameChoice6Perfect
    {
        get
        {
            return gameChoice6Perfect;
        }
        set
        {
            gameChoice1Perfect = value;
        }
    }
}
