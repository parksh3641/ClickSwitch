using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UpgradeInformation
{
    public int price = 0;
    public int addPrice = 0;
    public float value = 0;
    public float addValue = 0;
}

[CreateAssetMenu(fileName = "UpgradeDataBase", menuName = "ScriptableObjects/UpgradeDataBase")]
public class UpgradeDataBase : ScriptableObject
{
    public UpgradeInformation startTime;

    public UpgradeInformation critical;

    public UpgradeInformation burning;

    public UpgradeInformation addExp;

    public UpgradeInformation addGold;

    public UpgradeInformation comboTime;

    public UpgradeInformation comboCritical;

    public UpgradeInformation addScore;

    public void Initialize()
    {
        startTime = new UpgradeInformation();
        critical = new UpgradeInformation();
        burning = new UpgradeInformation();
        addExp = new UpgradeInformation();
        addGold = new UpgradeInformation();
        comboTime = new UpgradeInformation();
        comboCritical = new UpgradeInformation();
        addScore = new UpgradeInformation();
    }

    public UpgradeInformation StartTime
    {
        get
        {
            return startTime;
        }
        set
        {
            startTime = value;
        }
    }

    public UpgradeInformation Critical
    {
        get
        {
            return critical;
        }
        set
        {
            critical = value;
        }
    }

    public UpgradeInformation Burning
    {
        get
        {
            return burning;
        }
        set
        {
            burning = value;
        }
    }

    public UpgradeInformation AddExp
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

    public UpgradeInformation AddGold
    {
        get
        {
            return addGold;
        }
        set
        {
            addGold = value;
        }
    }

    public UpgradeInformation ComboTime
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

    public UpgradeInformation ComboCritical
    {
        get
        {
            return comboCritical;
        }
        set
        {
            comboCritical = value;
        }
    }

    public UpgradeInformation AddScore
    {
        get
        {
            return addScore;
        }
        set
        {
            addScore = value;
        }
    }

    public float GetValue(UpgradeType type, int level)
    {
        float value = 0;
        switch (type)
        {
            case UpgradeType.StartTime:
                value = startTime.value + (startTime.addValue * level);
                break;
            case UpgradeType.Critical:
                value = critical.value + (critical.addValue * level);
                break;
            case UpgradeType.Burning:
                value = burning.value + (burning.addValue * level);
                break;
            case UpgradeType.AddExp:
                value = addExp.value + (addExp.addValue * level);
                break;
            case UpgradeType.AddGold:
                value = addGold.value + (addGold.addValue * level);
                break;
            case UpgradeType.ComboTime:
                value = comboTime.value + (comboTime.addValue * level);
                break;
            case UpgradeType.ComboCritical:
                value = comboCritical.value + (comboCritical.addValue * level);
                break;
            case UpgradeType.AddScore:
                value = addScore.value + (addScore.addValue * level);
                break;
        }

        return value;
    }
}
