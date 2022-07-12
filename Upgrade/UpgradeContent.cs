using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeContent : MonoBehaviour
{
    UpgradeType upgradeType = UpgradeType.StartTime;

    public Image icon;

    public Text levelText;

    public LocalizationContent titleText;

    public Text valueText;
    public Text nextValueText;

    public Text upgradeValueText;

    private int level = 0;
    private int upgradeValue = 0;

    SoundManager soundManager;

    PlayerDataBase playerDataBase;
    ImageDataBase imageDataBase;
    Sprite[] upgradeIcon;

    UpgradeDataBase upgradeDataBase;
    UpgradeInformation upgradeInformation = new UpgradeInformation();


    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;

        upgradeIcon = imageDataBase.GetUpgradeArray();

        if (upgradeDataBase == null) upgradeDataBase = Resources.Load("UpgradeDataBase") as UpgradeDataBase;
    }

    public void Initialize(UpgradeType type, SoundManager manager)
    {
        upgradeType = type;
        soundManager = manager;

        icon.sprite = upgradeIcon[(int)type];

        level = playerDataBase.GetLevel(type);
        levelText.text = "Lv. " + level.ToString();

        titleText.name = type.ToString();
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
        }

        valueText.text = (upgradeInformation.value + (upgradeInformation.addValue * level)).ToString() + unit;

        upgradeValue = upgradeInformation.price + (upgradeInformation.addPrice * level);

        if (level >= 10)
        {
            nextValueText.text = "-";
            upgradeValueText.text = "Max";
        }
        else
        {
            nextValueText.text = (upgradeInformation.value + (upgradeInformation.addValue * (level + 1))).ToString() + unit;
            upgradeValueText.text = upgradeValue.ToString();
        }
    }

    public void UpgradeButton()
    {
        if (playerDataBase.Coin >= upgradeValue)
        {
            switch (upgradeType)
            {
                case UpgradeType.StartTime:
                    if(playerDataBase.StartTimeLevel >= 10)
                    {
                        NotionManager.instance.UseNotion(NotionType.UpgradeMax);
                        return;
                    }

                    playerDataBase.StartTimeLevel += 1;

                    if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdatePlayerStatisticsInsert("StartTimeLevel", playerDataBase.StartTimeLevel);
                    break;
                case UpgradeType.Critical:
                    if (playerDataBase.CriticalLevel >= 10)
                    {
                        NotionManager.instance.UseNotion(NotionType.UpgradeMax);
                        return;
                    }
                    playerDataBase.CriticalLevel += 1;

                    if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdatePlayerStatisticsInsert("CriticalLevel", playerDataBase.CriticalLevel);
                    break;
                case UpgradeType.Burning:
                    if (playerDataBase.BurningLevel >= 10)
                    {
                        NotionManager.instance.UseNotion(NotionType.UpgradeMax);
                        return;
                    }
                    playerDataBase.BurningLevel += 1;

                    if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdatePlayerStatisticsInsert("BurningLevel", playerDataBase.BurningLevel);
                    break;
                case UpgradeType.AddExp:
                    if (playerDataBase.AddExpLevel >= 10)
                    {
                        NotionManager.instance.UseNotion(NotionType.UpgradeMax);
                        return;
                    }
                    playerDataBase.AddExpLevel += 1;

                    if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdatePlayerStatisticsInsert("AddExpLevel", playerDataBase.AddExpLevel);
                    break;
                default:
                    return;
                    break;
            }

            if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdateSubtractCurrency(MoneyType.Coin, upgradeValue);

            soundManager.PlaySFX(GameSfxType.Success);

            NotionManager.instance.UseNotion(NotionType.UpgradeSuccess);

            CheckUpgrade();
        }
        else
        {
            NotionManager.instance.UseNotion(NotionType.LowCoinNotion);
        }
    }

    void CheckUpgrade()
    {
        Initialize(upgradeType, soundManager);
    }
}
