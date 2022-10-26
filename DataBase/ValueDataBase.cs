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
    private float gameChoice1Normal = 0;
    [SerializeField]
    private float gameChoice1Hard = 0;


    [Space]
    [SerializeField]
    private float gameChoice2Perfect = 0;
    [SerializeField]
    private float gameChoice2Normal = 0;
    [SerializeField]
    private float gameChoice2Hard = 0;


    [Space]
    [SerializeField]
    private float gameChoice3Perfect = 0;
    [SerializeField]
    private float gameChoice3Normal = 0;
    [SerializeField]
    private float gameChoice3Hard = 0;


    [Space]
    [SerializeField]
    private float gameChoice4Perfect = 0;
    [SerializeField]
    private float gameChoice4Normal = 0;
    [SerializeField]
    private float gameChoice4Hard = 0;


    [Space]
    [SerializeField]
    private float gameChoice5Perfect = 0;
    [SerializeField]
    private float gameChoice5Normal = 0;
    [SerializeField]
    private float gameChoice5Hard = 0;


    [Space]
    [SerializeField]
    private float gameChoice6Perfect = 0;
    [SerializeField]
    private float gameChoice6Normal = 0;
    [SerializeField]
    private float gameChoice6Hard = 0;


    [Space]
    [SerializeField]
    private float gameChoice7Perfect = 0;
    [SerializeField]
    private float gameChoice7Normal = 0;
    [SerializeField]
    private float gameChoice7Hard = 0;


    [Space]
    [SerializeField]
    private float gameChoice8Perfect = 0;
    [SerializeField]
    private float gameChoice8Normal = 0;
    [SerializeField]
    private float gameChoice8Hard = 0;



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
        gameChoice1Normal = 0;
        gameChoice1Hard = 0;

        gameChoice2Perfect = 0;
        gameChoice2Normal = 0;
        gameChoice2Hard = 0;

        gameChoice3Perfect = 0;
        gameChoice3Normal = 0;
        gameChoice3Hard = 0;

        gameChoice4Perfect = 0;
        gameChoice4Normal = 0;
        gameChoice4Hard = 0;

        gameChoice5Perfect = 0;
        gameChoice5Normal = 0;
        gameChoice5Hard = 0;

        gameChoice6Perfect = 0;
        gameChoice6Normal = 0;
        gameChoice6Hard = 0;

        gameChoice7Perfect = 0;
        gameChoice7Normal = 0;
        gameChoice7Hard = 0;

        gameChoice8Perfect = 0;
        gameChoice8Normal = 0;
        gameChoice8Hard = 0;

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
    public float GameChoice1Normal
    {
        get
        {
            return gameChoice1Normal;
        }
        set
        {
            gameChoice1Normal = value;
        }
    }
    public float GameChoice1Hard
    {
        get
        {
            return gameChoice1Hard;
        }
        set
        {
            gameChoice1Hard = value;
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
    public float GameChoice2Normal
    {
        get
        {
            return gameChoice2Normal;
        }
        set
        {
            gameChoice2Normal = value;
        }
    }
    public float GameChoice2Hard
    {
        get
        {
            return gameChoice2Hard;
        }
        set
        {
            gameChoice2Hard = value;
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
    public float GameChoice3Normal
    {
        get
        {
            return gameChoice3Normal;
        }
        set
        {
            gameChoice3Normal = value;
        }
    }
    public float GameChoice3Hard
    {
        get
        {
            return gameChoice3Hard;
        }
        set
        {
            gameChoice3Hard = value;
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
            gameChoice4Perfect = value;
        }
    }
    public float GameChoice4Normal
    {
        get
        {
            return gameChoice4Normal;
        }
        set
        {
            gameChoice4Normal = value;
        }
    }
    public float GameChoice4Hard
    {
        get
        {
            return gameChoice4Hard;
        }
        set
        {
            gameChoice4Hard = value;
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
            gameChoice5Perfect = value;
        }
    }
    public float GameChoice5Normal
    {
        get
        {
            return gameChoice5Normal;
        }
        set
        {
            gameChoice5Normal = value;
        }
    }
    public float GameChoice5Hard
    {
        get
        {
            return gameChoice5Hard;
        }
        set
        {
            gameChoice5Hard = value;
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
            gameChoice6Perfect = value;
        }
    }
    public float GameChoice6Normal
    {
        get
        {
            return gameChoice6Normal;
        }
        set
        {
            gameChoice6Normal = value;
        }
    }
    public float GameChoice6Hard
    {
        get
        {
            return gameChoice6Hard;
        }
        set
        {
            gameChoice6Hard = value;
        }
    }

    public float GameChoice7Perfect
    {
        get
        {
            return gameChoice7Perfect;
        }
        set
        {
            gameChoice7Perfect = value;
        }
    }
    public float GameChoice7Normal
    {
        get
        {
            return gameChoice7Normal;
        }
        set
        {
            gameChoice7Normal = value;
        }
    }
    public float GameChoice7Hard
    {
        get
        {
            return gameChoice7Hard;
        }
        set
        {
            gameChoice7Hard = value;
        }
    }

    public float GameChoice8Perfect
    {
        get
        {
            return gameChoice8Perfect;
        }
        set
        {
            gameChoice8Perfect = value;
        }
    }
    public float GameChoice8Normal
    {
        get
        {
            return gameChoice8Normal;
        }
        set
        {
            gameChoice8Normal = value;
        }
    }
    public float GameChoice8Hard
    {
        get
        {
            return gameChoice8Hard;
        }
        set
        {
            gameChoice8Hard = value;
        }
    }
}
