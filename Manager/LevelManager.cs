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
    }

    public void Initialize()
    {
        profileLevelText.text = "Lv." + (playerDataBase.Level + 1);

        profileFillamount.fillAmount = playerDataBase.Exp / CheckNeedExp();
    }

    public void CheckLevelUp(int getExp)
    {
        level = playerDataBase.Level;
        exp = playerDataBase.Exp + getExp;
        if (defaultExp == 0 || addExp == 0)
        {
            Debug.Log("레벨업 오류");
            return;
        }

        if(exp >= (defaultExp * CheckNeedExp()))
        {
            Debug.Log("레벨 업");

            OpenLevelView();

            playerDataBase.Level += 1;
            if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdatePlayerStatisticsInsert("Level", playerDataBase.Level);

            playerDataBase.Exp -= (int)CheckNeedExp();
            if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdatePlayerStatisticsInsert("Exp", playerDataBase.Exp);
        }
        else
        {
            Debug.Log("경험치 증가");

            playerDataBase.Exp += (int)CheckNeedExp();
            if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdatePlayerStatisticsInsert("Exp", playerDataBase.Exp);
        }

        Initialize();
    }

    float CheckNeedExp()
    {
        defaultExp = ValueManager.instance.GetDefaultExp();
        addExp = ValueManager.instance.GetAddExp();

        float needExp = defaultExp + ((level + 1) * addExp);

        return needExp;
    }

    public void OpenLevelView()
    {
        levelupView.SetActive(true);

        levelUpText.text = level.ToString();
        coinUpText.text = "+" + level + "%";

        soundManager.PlaySFX(GameSfxType.LevelUp);

    }

    public void CloseLevelView()
    {
        levelupView.SetActive(false);
    }
}
