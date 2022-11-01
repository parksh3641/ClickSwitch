using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardContent : MonoBehaviour
{
    RewardClass rewardClass;
    public int index = 0;

    [Title("Main")]
    public RectTransform main;
    Image mainBackground;

    [Space]
    public Image icon;
    public Text countText;
    public LocalizationContent nameText;

    [Space]
    [Title("Banner")]
    public GameObject bannerObject;
    public Image bannerIcon;
    public Text nickNameText;

    [Space]
    [Title("Paid")]
    public GameObject paidEffect;
    public GameObject paidFocus;

    [Space]
    public GameObject lockObject;
    public GameObject checkMark;

    ProgressManager progressManager;

    ImageDataBase imageDataBase;

    Sprite[] rankArray;
    Sprite[] vcArray;
    Sprite[] itemArray;
    Sprite[] etcArray;
    Sprite[] iconArray;
    Sprite[] bannerArray;

    private void Awake()
    {
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;

        mainBackground = main.GetComponent<Image>();

        rankArray = imageDataBase.GetRankArray();
        vcArray = imageDataBase.GetVCArray();
        itemArray = imageDataBase.GetItemArray();
        etcArray = imageDataBase.GetETCArray();
        iconArray = imageDataBase.GetProfileIconArray();
        bannerArray = imageDataBase.GetBannerArray();

        paidEffect.gameObject.SetActive(false);
        paidFocus.SetActive(false);
        lockObject.SetActive(true);
        checkMark.SetActive(false);
        bannerObject.SetActive(false);
    }

    private void OnEnable()
    {
        if (nameText.name.Length > 0) nameText.ReLoad();
    }

    public void Initialize(ProgressManager manager, RewardClass _rewardClass, int number)
    {
        progressManager = manager;

        rewardClass = _rewardClass;

        index = number;

        countText.text = "x" + rewardClass.count.ToString();

        nameText.name = "";

        switch (rewardClass.rewardType)
        {
            case RewardType.Coin:
                icon.sprite = vcArray[0];
                mainBackground.sprite = rankArray[0];

                break;
            case RewardType.Crystal:
                icon.sprite = vcArray[1];
                mainBackground.sprite = rankArray[1];

                break;
            case RewardType.Clock:
                icon.sprite = itemArray[0];
                mainBackground.sprite = rankArray[0];

                break;
            case RewardType.Shield:
                icon.sprite = itemArray[1];
                mainBackground.sprite = rankArray[0];

                break;
            case RewardType.Combo:
                icon.sprite = itemArray[2];
                mainBackground.sprite = rankArray[0];

                break;
            case RewardType.Exp:
                icon.sprite = itemArray[3];
                mainBackground.sprite = rankArray[1];

                break;
            case RewardType.Slow:
                icon.sprite = itemArray[4];
                mainBackground.sprite = rankArray[1];

                break;
            case RewardType.IconBox:
                icon.sprite = etcArray[0];
                mainBackground.sprite = rankArray[1];

                break;
            case RewardType.Icon:
                icon.sprite = iconArray[(int)rewardClass.iconType];
                mainBackground.sprite = rankArray[2];

                countText.text = "";
                nameText.name = LocalizationManager.instance.GetString("Icon");
                break;
            case RewardType.Banner:
                icon.gameObject.SetActive(false);
                mainBackground.sprite = rankArray[2];

                main.sizeDelta = new Vector2(350, 200);
                paidFocus.GetComponent<RectTransform>().sizeDelta = new Vector2(360, 210);
                lockObject.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 200);

                bannerObject.SetActive(true);
                bannerIcon.sprite = bannerArray[(int)rewardClass.bannerType];

                if (GameStateManager.instance.NickName != null)
                {
                    nickNameText.text = GameStateManager.instance.NickName;
                }
                else
                {
                    nickNameText.text = GameStateManager.instance.CustomId;
                }

                countText.text = "";
                nameText.name = LocalizationManager.instance.GetString("Banner");
                break;
        }

        switch (rewardClass.rewardReceiveType)
        {
            case RewardReceiveType.Free:
                break;
            case RewardReceiveType.Paid:
                paidEffect.gameObject.SetActive(true);
                paidFocus.SetActive(true);
                break;
        }

        nameText.ReLoad();
    }

    public void UnLock()
    {
        lockObject.SetActive(false);
    }

    public void CheckMark(bool check)
    {
        checkMark.SetActive(check);
    }

    public void OnClick()
    {
        if(!lockObject.activeInHierarchy)
        {
            progressManager.ReceiveButton(rewardClass, index);

            lockObject.SetActive(true);
            CheckMark(true);
        }
    }
}
