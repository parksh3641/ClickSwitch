using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public GameObject upgradeView;

    public GameObject Lock;

    public UpgradeContent upgradeContent;
    public RectTransform upgradeTransform;

    public SoundManager soundManager;

    List<UpgradeContent> upgradeContentList = new List<UpgradeContent>();

    PlayerDataBase playerDataBase;
    UpgradeDataBase upgradeDataBase;

    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;
        if (upgradeDataBase == null) upgradeDataBase = Resources.Load("UpgradeDataBase") as UpgradeDataBase;

        upgradeView.SetActive(false);

        upgradeTransform.anchoredPosition = new Vector2(0, -999);
    }

    public void Initialize()
    {
        for(int i = 0; i < System.Enum.GetValues(typeof(UpgradeType)).Length; i ++)
        {
            UpgradeContent monster = Instantiate(upgradeContent);
            monster.transform.parent = upgradeTransform;
            monster.transform.position = Vector3.zero;
            monster.transform.rotation = Quaternion.identity;
            monster.transform.localScale = Vector3.one;

            monster.gameObject.SetActive(true);
            monster.Initialize(UpgradeType.StartTime + i, soundManager);

            upgradeContentList.Add(monster);
        }

        Lock.SetActive(false);

        if (playerDataBase.Level < 3)
        {
            Lock.SetActive(true);
        }
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
}
