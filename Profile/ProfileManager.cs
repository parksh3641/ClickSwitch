using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileManager : MonoBehaviour
{
    public GameObject profileView;

    [Title("Player")]
    public Text nickNameText;
    public Image iconImg;

    [Space]
    [Title("Text")]
    public Text plusScoreText;
    public Text plusExpText;
    public Text plusCoinText;
    public Text totalScoreText;
    public Text totalComboText;


    [Space]
    [Title("Infomation")]
    public ProfileContent profileContent;

    public RectTransform profileTransform;


    public List<ProfileContent> profileContentList = new List<ProfileContent>();

    ImageDataBase imageDataBase;
    Sprite[] iconArray;

    PlayerDataBase playerDataBase;
    ShopDataBase shopDataBase;
    UpgradeDataBase upgradeDataBase;

    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;
        if (shopDataBase == null) shopDataBase = Resources.Load("ShopDataBase") as ShopDataBase;
        if (upgradeDataBase == null) upgradeDataBase = Resources.Load("UpgradeDataBase") as UpgradeDataBase;

        iconArray = imageDataBase.GetIconArray();

        profileContentList.Clear();

        for (int i = 0; i < 6; i ++)
        {
            ProfileContent monster = Instantiate(profileContent);
            monster.transform.parent = profileTransform;
            monster.transform.position = Vector3.zero;
            monster.transform.localScale = new Vector3(1, 1, 1);
            monster.gameObject.SetActive(true);
            profileContentList.Add(monster);
        }

        plusScoreText.text = "0%";
        plusExpText.text = "0%";
        plusCoinText.text = "0%";
        totalScoreText.text = "0";
        totalComboText.text = "0";

        profileView.SetActive(false);

        profileTransform.anchoredPosition = new Vector2(0, -999);
    }

    public void OpenProfile()
    {
        if (!profileView.activeSelf)
        {
            profileView.SetActive(true);

            SetProfile();
        }
        else
        {
            profileView.SetActive(false);
        }
    }

    void SetProfile()
    {
        if(GameStateManager.instance.NickName != null)
        {
            nickNameText.text = GameStateManager.instance.NickName;
        }
        else
        {
            nickNameText.text = GameStateManager.instance.CustomId;
        }

        profileContentList[0].InitState(LocalizationManager.instance.GetString("GameChoice1"), playerDataBase.BestSpeedTouchScore, playerDataBase.BestSpeedTouchCombo, iconArray[0]);
        profileContentList[1].InitState(LocalizationManager.instance.GetString("GameChoice2"), playerDataBase.BestMoleCatchScore, playerDataBase.BestMoleCatchCombo, iconArray[1]);
        profileContentList[2].InitState(LocalizationManager.instance.GetString("GameChoice3"), playerDataBase.BestFilpCardScore, playerDataBase.BestFilpCardCombo, iconArray[2]);
        profileContentList[3].InitState(LocalizationManager.instance.GetString("GameChoice4"), playerDataBase.BestButtonActionScore, playerDataBase.BestButtonActionCombo, iconArray[3]);
        profileContentList[4].InitState(LocalizationManager.instance.GetString("GameChoice5"), playerDataBase.BestTimingActionScore, playerDataBase.BestTimingActionCombo, iconArray[4]);
        profileContentList[5].InitState(LocalizationManager.instance.GetString("GameChoice6"), playerDataBase.BestDragActionScore, playerDataBase.BestDragActionCombo, iconArray[5]);

        totalScoreText.text = playerDataBase.TotalScore.ToString();
        totalComboText.text = playerDataBase.TotalCombo.ToString();

        int plusScore = shopDataBase.GetIconHoldNumber();

        plusScoreText.text = (0.5f * plusScore).ToString() + "%";

        float plusExp = playerDataBase.AddExpLevel * upgradeDataBase.addExp.addValue;

        plusExpText.text = plusExp.ToString() + "%";

        int plusCoin = playerDataBase.Level + 1;

        if(plusCoin >= 30)
        {
            plusCoin = 30;
        }

        plusCoinText.text = plusCoin.ToString() + "%";
    }
}
