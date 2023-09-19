using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressManager : MonoBehaviour
{
    public GameObject progressView;
    public GameObject showVCView;
    public GameObject sPurchaseView;

    public Text totalScoreText;
    public Text levelText;
    public Image fillAmount;

    [Space]
    [Title("Purchase")]
    public LocalizationContent priceText;
    public GameObject[] purchaseObj;

    string platform = "";

    public ProgressContent progressContent;
    public RectTransform progressContentTransform;

    public SoundManager soundManager;

    List<ProgressContent> progressContentList = new List<ProgressContent>();

    Dictionary<string, string> playerData = new Dictionary<string, string>();
    List<string> rewardID = new List<string>();

    PlayerDataBase playerDataBase;
    ProgressDataBase progressDataBase;
    ShopDataBase shopDataBase;

    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;
        if (progressDataBase == null) progressDataBase = Resources.Load("ProgressDataBase") as ProgressDataBase;
        if (shopDataBase == null) shopDataBase = Resources.Load("ShopDataBase") as ShopDataBase;

        progressView.SetActive(false);
        sPurchaseView.SetActive(false);

        progressContentTransform.anchoredPosition = new Vector2(0, 9999);
    }

    public void Initialize()
    {
        for(int i = 0; i < progressDataBase.freeRewardList.Count; i ++)
        {
            ProgressContent monster = Instantiate(progressContent);
            monster.name = "ProgressContent_" + i;
            monster.transform.position = Vector3.zero;
            monster.transform.localScale = Vector3.one;
            monster.transform.SetParent(progressContentTransform);

            monster.Initialize(i, this, progressDataBase.freeRewardList[i], progressDataBase.paidRewardList[i]);

            monster.gameObject.SetActive(true);

            progressContentList.Add(monster);
        }
    }

    public void OpenProgress()
    {
        if (!progressView.activeSelf)
        {
            progressView.SetActive(true);
            showVCView.SetActive(true);

            CheckProgress();
        }
        else
        {
            SaveProgress();

            progressView.SetActive(false);
            showVCView.SetActive(false);
        }
    }

    public void OpenPurchaseView()
    {
#if UNITY_ANDROID || UNITY_EDITOR
        platform = "_AOS";
#endif
        platform = "_IOS";

        if (!sPurchaseView.activeSelf)
        {
            sPurchaseView.SetActive(true);

            priceText.localizationName = "ShopPaidProgress" + platform;
            priceText.ReLoad();
        }
        else
        {
            sPurchaseView.SetActive(false);
        }
    }

    public void CheckProgress()
    {
        for(int i = 0; i < progressContentList.Count; i ++)
        {
            progressContentList[i].transform.localScale = Vector3.one;
        }

        CheckPurchaseButton();

        int score = playerDataBase.TotalScore;
        int level = score / 200;
        int goal = (level + 1) * 200;

        if (level >= 29) level = 29;

        totalScoreText.text = playerDataBase.TotalScore + " / " + goal.ToString();

        levelText.text = (level + 2).ToString();

        fillAmount.fillAmount = score * 1.0f / goal;

        for (int i = 0; i < level + 1; i ++)
        {
            if(playerDataBase.GetProgress(progressDataBase.freeRewardList[i].rewardReceiveType, i) == false)
            {
                progressContentList[i].UnLockFree();
            }
            else
            {
                progressContentList[i].CheckMarkFree();
            }

            if (playerDataBase.PaidProgress)
            {
                if (playerDataBase.GetProgress(progressDataBase.paidRewardList[i].rewardReceiveType, i) == false)
                {
                    progressContentList[i].UnLockPaid();
                }
                else
                {
                    progressContentList[i].CheckMarkPaid();
                }
            }
        }
    }

    public void CheckPurchaseButton()
    {
        purchaseObj[0].SetActive(false);
        purchaseObj[1].SetActive(false);

        if (!playerDataBase.PaidProgress)
        {
            purchaseObj[0].SetActive(true);
        }
        else
        {
            purchaseObj[1].SetActive(true);
        }
    }

    public void ReceiveButton(RewardClass rewardClass, int number)
    {
        switch (rewardClass.rewardType) //보상 주기
        {
            case RewardType.Coin:
                if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdateAddCurrency(MoneyType.Coin, rewardClass.count);
                break;
            case RewardType.Crystal:
                if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdateAddCurrency(MoneyType.Crystal, rewardClass.count);
                break;
            case RewardType.Clock:
                ReceiveItem(rewardClass);
                break;
            case RewardType.Shield:
                ReceiveItem(rewardClass);
                break;
            case RewardType.Combo:
                ReceiveItem(rewardClass);
                break;
            case RewardType.Exp:
                ReceiveItem(rewardClass);
                break;
            case RewardType.Slow:
                ReceiveItem(rewardClass);
                break;
            case RewardType.IconBox:
                if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdatePlayerStatisticsInsert("IconBox", rewardClass.count);
                break;
            case RewardType.Icon:
                shopDataBase.AddIconAll(rewardClass.iconType);

                if (PlayfabManager.instance.isActive) PlayfabManager.instance.GrantItemsToUser(rewardClass.iconType.ToString(), "Icon");
                break;
            case RewardType.Banner:
                shopDataBase.SetBanner(rewardClass.bannerType, 1);

                if (PlayfabManager.instance.isActive) PlayfabManager.instance.GrantItemsToUser(rewardClass.bannerType.ToString(), "Banner");
                break;
        }

        playerDataBase.UpdateProgress(rewardClass.rewardReceiveType, number);

        NotionManager.instance.UseNotion(NotionType.ReceiveNotion);

        soundManager.PlaySFX(GameSfxType.Success);
    }

    void ReceiveItem(RewardClass rewardClass)
    {
        rewardID.Clear();

        rewardID.Add((rewardClass.rewardType).ToString());

        playerDataBase.SetItemCount((ItemType)Enum.Parse(typeof(ItemType), rewardClass.rewardType.ToString()), rewardClass.count);

        for (int i = 0; i < rewardClass.count; i ++)
        {
            if (PlayfabManager.instance.isActive) PlayfabManager.instance.GrantItemToUser("Shop", rewardID);
        }
    }

    private void OnApplicationQuit()
    {
        SaveProgress();
    }

    public void SaveProgress()
    {
        playerData.Clear();

        string freeProgress = playerDataBase.FreeProgressData;

        if(freeProgress.Length < 29)
        {
            freeProgress = "000000000000000000000000000000";
        }

        playerData.Add("FreeProgress" , freeProgress);
        if (PlayfabManager.instance.isActive) PlayfabManager.instance.SetPlayerData(playerData);


        playerData.Clear();

        string paidProgress = playerDataBase.PaidProgressData;

        if (paidProgress.Length < 29)
        {
            paidProgress = "000000000000000000000000000000";
        }

        playerData.Add("PaidProgress", paidProgress);
        if (PlayfabManager.instance.isActive) PlayfabManager.instance.SetPlayerData(playerData);

        Debug.Log("Save Progress");
    }

    public void BuyPaidProgress()
    {
        CheckProgress();

        sPurchaseView.SetActive(false);
    }
}
