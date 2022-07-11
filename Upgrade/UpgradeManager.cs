using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public GameObject upgradeView;


    PlayerDataBase playerDataBase;
    UpgradeDataBase upgradeDataBase;

    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;
        if (upgradeDataBase == null) upgradeDataBase = Resources.Load("UpgradeDataBase") as UpgradeDataBase;

        upgradeView.SetActive(false);
    }

    public void Initialize()
    {

    }

    public void OpenUpgrade()
    {
        if (!upgradeView.activeSelf)
        {
            upgradeView.SetActive(true);

        }
        else
        {
            upgradeView.SetActive(false);
        }
    }

    void CheckUpgrade()
    {

    }
}
