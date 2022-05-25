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


    [Title("Infomation")]
    public ProfileContent profileContent;

    public RectTransform profileTransform;


    public List<ProfileContent> profileContentList = new List<ProfileContent>();


    public PlayerDataBase playerDataBase;

    private void Awake()
    {
        profileContentList.Clear();

        for (int i = 0; i < 4; i ++)
        {
            ProfileContent monster = Instantiate(profileContent);
            monster.transform.parent = profileTransform;
            monster.transform.position = Vector3.zero;
            monster.transform.localScale = new Vector3(1, 1, 1);
            monster.gameObject.SetActive(true);
            profileContentList.Add(monster);
        }

        profileView.SetActive(false);

        profileTransform.anchoredPosition = new Vector2(0, -500);

        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;
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

        profileContentList[0].InitState(LocalizationManager.instance.GetString("GameChoice1"), playerDataBase.BestSpeedTouchScore, playerDataBase.BestSpeedTouchCombo);
        profileContentList[1].InitState(LocalizationManager.instance.GetString("GameChoice2"), playerDataBase.BestMoleCatchScore, playerDataBase.BestMoleCatchCombo);
        profileContentList[2].InitState(LocalizationManager.instance.GetString("GameChoice3"), playerDataBase.BestFilpCardScore, playerDataBase.BestFilpCardCombo);
        profileContentList[3].InitState(LocalizationManager.instance.GetString("GameChoice4"), playerDataBase.BestButtonActionScore, playerDataBase.BestButtonActionCombo);
    }
}
