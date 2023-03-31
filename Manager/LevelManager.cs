using Firebase.Analytics;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public Text profileLevelText;
    public Image profileFillamount;

    public Text mainLevelText;
    public Image mainFillamount;

    public GameObject levelupView;
    public GameObject showVCView;

    public ButtonScaleAnimation button;
    public GameObject lockObj;

    public Text levelUpText; //다음 레벨
    public Text bounsInfoText; //코인 획득량
    public Text coinText;

    private int level = 0;
    private int exp = 0;
    private int coin = 0;

    private float defaultExp = 1000;
    private float addExp = 100;

    public bool adsActive = false;

    public PlayerDataBase playerDataBase;
    public SoundManager soundManager;


    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;

        levelupView.SetActive(false);
    }

    public void Initialize()
    {
        profileLevelText.text = "Lv." + (playerDataBase.Level + 1) + "   (" + playerDataBase.Experience + "/" + CheckNeedExp() + ")";
        profileFillamount.fillAmount = playerDataBase.Experience / CheckNeedExp();

        mainLevelText.text = "Lv." + (playerDataBase.Level + 1);
        mainFillamount.fillAmount = playerDataBase.Experience / CheckNeedExp();
    }

    public void CheckLevelUp(float getExp)
    {
        level = playerDataBase.Level;
        exp = playerDataBase.Experience;
        int plusExp = (int)getExp;
        if (defaultExp == 0 || addExp == 0)
        {
            Debug.Log("레벨업 오류");
            return;
        }

        if(exp + plusExp >= CheckNeedExp())
        {
            Debug.Log("레벨 업");

            OpenLevelView();

            playerDataBase.Level += 1;
            if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdatePlayerStatisticsInsert("Level", playerDataBase.Level);

            playerDataBase.Experience -= ((int)CheckNeedExp() - plusExp);
            if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdatePlayerStatisticsInsert("Exp", playerDataBase.Experience);

            FirebaseAnalytics.LogEvent("Level Up");
        }
        else
        {
            Debug.Log("경험치 증가");

            playerDataBase.Experience += plusExp;
            if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdatePlayerStatisticsInsert("Exp", playerDataBase.Experience);
        }

        Initialize();
    }

    float CheckNeedExp()
    {
        defaultExp = ValueManager.instance.GetDefaultExp();
        addExp = ValueManager.instance.GetAddExp();

        float needExp = defaultExp + (playerDataBase.Level * addExp);

        return needExp;
    }

    [Button]
    public void OpenLevelView()
    {
        levelupView.SetActive(true);
        showVCView.SetActive(true);

        lockObj.SetActive(false);

        adsActive = false;

        levelUpText.text = (level + 1).ToString();

        coin = 1000 + ((level + 1) * 300);

        coinText.text = coin.ToString();

        if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdateAddCurrency(MoneyType.Coin, coin);

        soundManager.PlaySFX(GameSfxType.LevelUp);

        if (level + 1 > 49) level = 49;
        bounsInfoText.text = LocalizationManager.instance.GetString("PlusCoinInfo") + " +" + (level + 1) + "%";
    }

    public void CloseLevelView()
    {
        levelupView.SetActive(false);
        showVCView.SetActive(false);
    }

    public void SuccessWatchAd()
    {
        lockObj.SetActive(true);

        button.StopAnim();

        adsActive = true;

        coinText.text = coin + " +" + coin.ToString();

        if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdateAddCurrency(MoneyType.Coin, coin);
    }
}
