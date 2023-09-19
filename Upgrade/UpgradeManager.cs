using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    public GameObject upgradeView;
    public GameObject showVCView;

    [Title("TopMenu")]
    public Image[] topMenuImgArray;
    public Sprite[] topMenuSpriteArray;
    public GameObject[] scrollView;

    private int topNumber = 0;

    public UpgradeContent upgradeContent;
    public RectTransform upgradeTransform;
    public RectTransform upgradeSpeicalTransform;

    public SoundManager soundManager;

    List<UpgradeContent> upgradeContentList = new List<UpgradeContent>();

    PlayerDataBase playerDataBase;
    UpgradeDataBase upgradeDataBase;

    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;
        if (upgradeDataBase == null) upgradeDataBase = Resources.Load("UpgradeDataBase") as UpgradeDataBase;

        upgradeView.SetActive(false);
        showVCView.SetActive(false);

        upgradeTransform.anchoredPosition = new Vector2(0, -999);

        topNumber = -1;
    }

    public void Initialize()
    {
        for(int i = 0; i < System.Enum.GetValues(typeof(UpgradeType)).Length; i ++)
        {
            UpgradeContent monster = Instantiate(upgradeContent);

            if(i < 7)
            {
                monster.transform.SetParent(upgradeTransform);
            }
            else
            {
                monster.transform.SetParent(upgradeSpeicalTransform);
            }

            monster.transform.position = Vector3.zero;
            monster.transform.rotation = Quaternion.identity;
            monster.transform.localScale = Vector3.one;

            monster.gameObject.SetActive(true);
            monster.Initialize(UpgradeType.StartTime + i, soundManager);

            if(i == 2)
            {
                monster.gameObject.SetActive(false);
            }

            upgradeContentList.Add(monster);
        }

    }

    public void OpenUpgrade()
    {
        if (!upgradeView.activeSelf)
        {
            upgradeView.SetActive(true);
            showVCView.SetActive(true);

            if (topNumber == -1)
            {
                ChangeTopMenu(0);

                for(int i = 0; i< upgradeContentList.Count; i ++)
                {
                    upgradeContentList[i].CheckUpgrade();
                }
            }

        }
        else
        {
            upgradeView.SetActive(false);
            showVCView.SetActive(false);
        }
    }

    public void ChangeTopMenu(int number)
    {
        if (topNumber != number)
        {
            topNumber = number;

            for (int i = 0; i < topMenuImgArray.Length; i++)
            {
                topMenuImgArray[i].sprite = topMenuSpriteArray[0];
                scrollView[i].SetActive(false);
            }
            topMenuImgArray[number].sprite = topMenuSpriteArray[1];
            scrollView[number].SetActive(true);
        }
    }
}
