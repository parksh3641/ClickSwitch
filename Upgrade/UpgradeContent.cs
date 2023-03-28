using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeContent : MonoBehaviour
{
    public UpgradeType upgradeType = UpgradeType.StartTime;

    public Image icon;

    public Text levelText;

    public LocalizationContent titleText;

    public Image valueIcon;
    public Text valueText;
    public Text nextValueText;

    public Text upgradeValueText;
    public GameObject maxObj;

    private int level = 0;
    private int upgradeValue = 0;
    private int maxLevel = 60;

    SoundManager soundManager;

    PlayerDataBase playerDataBase;
    ImageDataBase imageDataBase;
    Sprite[] upgradeIcon;
    Sprite[] vcArray;

    UpgradeDataBase upgradeDataBase;
    UpgradeInformation upgradeInformation = new UpgradeInformation();

    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;
        if (upgradeDataBase == null) upgradeDataBase = Resources.Load("UpgradeDataBase") as UpgradeDataBase;

        if (upgradeIcon == null) upgradeIcon = imageDataBase.GetUpgradeArray();
        if (vcArray == null) vcArray = imageDataBase.GetVCArray();
    }

    public void Initialize(UpgradeType type, SoundManager manager)
    {
        upgradeType = type;
        soundManager = manager;

        icon.sprite = upgradeIcon[(int)type];
        valueIcon.sprite = vcArray[0];

        level = playerDataBase.GetLevel(type);

        //if(playerDataBase.Level + 1 >= maxLevel)
        //{
        //    levelText.text = "Lv. " + level + "/" + maxLevel.ToString();
        //}
        //else
        //{
        //    levelText.text = "Lv. " + level + "/" + (playerDataBase.Level + 1).ToString();
        //}

        levelText.text = "Lv. " + level + "/" + maxLevel.ToString();

        titleText.localizationName = type.ToString();
        titleText.ReLoad();

        string unit = "%";

        switch (type)
        {
            case UpgradeType.StartTime:
                upgradeInformation = upgradeDataBase.StartTime;
                unit = "s";
                break;
            case UpgradeType.Critical:
                upgradeInformation = upgradeDataBase.Critical;
                break;
            case UpgradeType.Burning:
                upgradeInformation = upgradeDataBase.Burning;
                break;
            case UpgradeType.AddExp:
                upgradeInformation = upgradeDataBase.AddExp;
                break;
            case UpgradeType.AddGold:
                upgradeInformation = upgradeDataBase.AddGold;
                break;
            case UpgradeType.ComboTime:
                upgradeInformation = upgradeDataBase.ComboTime;
                unit = "s";
                break;
            case UpgradeType.ComboCritical:
                upgradeInformation = upgradeDataBase.ComboCritical;
                break;
            case UpgradeType.AddScore:
                upgradeInformation = upgradeDataBase.AddScore;

                valueIcon.sprite = vcArray[1];
                break;
            default:
                upgradeInformation = upgradeDataBase.StartTime;
                break;
        }

        valueText.text = (upgradeInformation.value + (upgradeInformation.addValue * level)).ToString() + unit;

        upgradeValue = upgradeInformation.price + (upgradeInformation.addPrice * level);

        if (level >= maxLevel)
        {
            nextValueText.text = "-";
            upgradeValueText.text = "";
            maxObj.SetActive(true);
        }
        else
        {
            nextValueText.text = (upgradeInformation.value + (upgradeInformation.addValue * (level + 1))).ToString() + unit;
            upgradeValueText.text = upgradeValue.ToString();
            maxObj.SetActive(false);
        }
    }

    public void UpgradeButton()
    {
        if (upgradeType != UpgradeType.AddScore)
        {
            if (playerDataBase.Coin <= upgradeValue || upgradeValue == 0)
            {
                NotionManager.instance.UseNotion(NotionType.LowCoinNotion);
                return;
            }
        }
        else
        {
            if (playerDataBase.Crystal <= upgradeValue || upgradeValue == 0)
            {
                NotionManager.instance.UseNotion(NotionType.LowCrystalNotion);
                return;
            }
        }

        switch (upgradeType)
        {
            case UpgradeType.StartTime:
                //if (playerDataBase.StartTimeLevel >= maxLevel || playerDataBase.StartTimeLevel >= (playerDataBase.Level + 1))
                //{
                //    NotionManager.instance.UseNotion(NotionType.UpgradeMax);
                //    return;
                //}

                if (playerDataBase.StartTimeLevel >= maxLevel)
                {
                    NotionManager.instance.UseNotion(NotionType.UpgradeMax);
                    return;
                }

                playerDataBase.StartTimeLevel += 1;

                if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdatePlayerStatisticsInsert("StartTimeLevel", playerDataBase.StartTimeLevel);
                break;
            case UpgradeType.Critical:
                //if (playerDataBase.CriticalLevel >= maxLevel || playerDataBase.CriticalLevel >= (playerDataBase.Level + 1))
                //{
                //    NotionManager.instance.UseNotion(NotionType.UpgradeMax);
                //    return;
                //}

                if (playerDataBase.CriticalLevel >= maxLevel)
                {
                    NotionManager.instance.UseNotion(NotionType.UpgradeMax);
                    return;
                }

                playerDataBase.CriticalLevel += 1;

                if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdatePlayerStatisticsInsert("CriticalLevel", playerDataBase.CriticalLevel);
                break;
            case UpgradeType.Burning:
                //if (playerDataBase.BurningLevel >= maxLevel || playerDataBase.BurningLevel >= (playerDataBase.Level + 1))
                //{
                //    NotionManager.instance.UseNotion(NotionType.UpgradeMax);
                //    return;
                //}

                if (playerDataBase.BurningLevel >= maxLevel)
                {
                    NotionManager.instance.UseNotion(NotionType.UpgradeMax);
                    return;
                }

                playerDataBase.BurningLevel += 1;

                if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdatePlayerStatisticsInsert("BurningLevel", playerDataBase.BurningLevel);
                break;
            case UpgradeType.AddExp:
                //if (playerDataBase.AddExpLevel >= maxLevel || playerDataBase.AddExpLevel >= (playerDataBase.Level + 1))
                //{
                //    NotionManager.instance.UseNotion(NotionType.UpgradeMax);
                //    return;
                //}

                if (playerDataBase.AddExpLevel >= maxLevel)
                {
                    NotionManager.instance.UseNotion(NotionType.UpgradeMax);
                    return;
                }

                playerDataBase.AddExpLevel += 1;

                if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdatePlayerStatisticsInsert("AddExpLevel", playerDataBase.AddExpLevel);
                break;
            case UpgradeType.AddGold:
                //if (playerDataBase.AddGoldLevel >= maxLevel || playerDataBase.AddGoldLevel >= (playerDataBase.Level + 1))
                //{
                //    NotionManager.instance.UseNotion(NotionType.UpgradeMax);
                //    return;
                //}

                if (playerDataBase.AddGoldLevel >= maxLevel)
                {
                    NotionManager.instance.UseNotion(NotionType.UpgradeMax);
                    return;
                }

                playerDataBase.AddGoldLevel += 1;

                if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdatePlayerStatisticsInsert("AddGoldLevel", playerDataBase.AddGoldLevel);
                break;
            case UpgradeType.ComboTime:
                //if (playerDataBase.ComboTimeLevel >= maxLevel || playerDataBase.ComboTimeLevel >= (playerDataBase.Level + 1))
                //{
                //    NotionManager.instance.UseNotion(NotionType.UpgradeMax);
                //    return;
                //}

                if (playerDataBase.ComboTimeLevel >= maxLevel)
                {
                    NotionManager.instance.UseNotion(NotionType.UpgradeMax);
                    return;
                }

                playerDataBase.ComboTimeLevel += 1;

                if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdatePlayerStatisticsInsert("ComboTimeLevel", playerDataBase.ComboTimeLevel);
                break;
            case UpgradeType.ComboCritical:
                //if (playerDataBase.ComboCriticalLevel >= maxLevel || playerDataBase.ComboCriticalLevel >= (playerDataBase.Level + 1))
                //{
                //    NotionManager.instance.UseNotion(NotionType.UpgradeMax);
                //    return;
                //}

                if (playerDataBase.ComboCriticalLevel >= maxLevel)
                {
                    NotionManager.instance.UseNotion(NotionType.UpgradeMax);
                    return;
                }

                playerDataBase.ComboCriticalLevel += 1;

                if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdatePlayerStatisticsInsert("ComboCriticalLevel", playerDataBase.ComboCriticalLevel);
                break;
            case UpgradeType.AddScore:
                //if (playerDataBase.AddScoreLevel >= maxLevel || playerDataBase.AddScoreLevel >= (playerDataBase.Level + 1))
                //{
                //    NotionManager.instance.UseNotion(NotionType.UpgradeMax);
                //    return;
                //}

                if (playerDataBase.AddScoreLevel >= maxLevel)
                {
                    NotionManager.instance.UseNotion(NotionType.UpgradeMax);
                    return;
                }

                playerDataBase.AddScoreLevel += 1;

                if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdatePlayerStatisticsInsert("AddScoreLevel", playerDataBase.AddScoreLevel);
                break;
            default:
                return;
        }

        if (upgradeType != UpgradeType.AddScore)
        {
            if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdateSubtractCurrency(MoneyType.Coin, upgradeValue);
        }
        else
        {
            if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdateSubtractCurrency(MoneyType.Crystal, upgradeValue);
        }

        soundManager.PlaySFX(GameSfxType.LevelUp);

        NotionManager.instance.UseNotion(NotionType.UpgradeSuccess);

        CheckUpgrade();
    }

    public void CheckUpgrade()
    {
        Initialize(upgradeType, soundManager);
    }
}
