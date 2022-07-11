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
    [ShowInInspector]
    private UpgradeInformation startTime;

    [ShowInInspector]
    private UpgradeInformation critical;

    [ShowInInspector]
    private UpgradeInformation burning;

    [ShowInInspector]
    private UpgradeInformation addExp;

    public void Initialize()
    {
        startTime = new UpgradeInformation();
        critical = new UpgradeInformation();
        burning = new UpgradeInformation();
        addExp = new UpgradeInformation();
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
}
