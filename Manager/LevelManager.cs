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

    public GameObject levelupView;

    public Text levelUpText; //다음 레벨
    public Text coinUpText; //코인 획득량

    private int level = 0;
    private int exp = 0;

    private float defaultExp = 1000;
    private float addExp = 100;

    public PlayerDataBase playerDataBase;
    public SoundManager soundManager;


    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;

        levelupView.SetActive(false);
    }

    public void Initialize()
    {
        profileLevelText.text = "Lv." + (playerDataBase.Level + 1);

        profileFillamount.fillAmount = playerDataBase.Experience / CheckNeedExp();
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

        levelUpText.text = (level + 1).ToString();
        coinUpText.text = LocalizationManager.instance.GetString("Bouns") + " + " + (level + 1) + "%";

        soundManager.PlaySFX(GameSfxType.LevelUp);

    }

    public void CloseLevelView()
    {
        levelupView.SetActive(false);
    }
}
