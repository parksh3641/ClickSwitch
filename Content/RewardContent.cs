using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardContent : MonoBehaviour
{
    RewardClass rewardClass;

    [Title("Main")]
    public RectTransform main;
    Image mainBackground;

    [Space]
    public Image icon;
    public Text countText;

    [Space]
    [Title("Banner")]
    public GameObject bannerObject;
    public Image bannerIcon;
    public Text nickNameText;


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

        lockObject.SetActive(true);
        checkMark.SetActive(false);
        bannerObject.SetActive(false);
    }

    public void Initialize(ProgressManager manager, RewardClass _rewardClass)
    {
        progressManager = manager;

        rewardClass = _rewardClass;

        countText.text = "x" + rewardClass.count.ToString();

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

                break;
            case RewardType.Banner:
                icon.gameObject.SetActive(false);
                mainBackground.sprite = rankArray[3];

                main.sizeDelta = new Vector2(350, 200);
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

                break;
        }
        UnLock();
    }

    public void InitState()
    {

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
            progressManager.Receive(rewardClass);
        }
    }
}
