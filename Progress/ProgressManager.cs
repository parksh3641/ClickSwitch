using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    public GameObject progressView;
    public GameObject showVCView;

    public ProgressContent progressContent;
    public RectTransform progressContentTransform;


    List<ProgressContent> progressContentList = new List<ProgressContent>();

    Dictionary<string, string> playerData = new Dictionary<string, string>();


    PlayerDataBase playerDataBase;
    ProgressDataBase progressDataBase;

    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;
        if (progressDataBase == null) progressDataBase = Resources.Load("ProgressDataBase") as ProgressDataBase;

        progressView.SetActive(false);

        progressContentTransform.anchoredPosition = new Vector2(0, 9999);
    }

    public void Initialize()
    {
        for(int i = 0; i < progressDataBase.freeRewardList.Count; i ++)
        {
            ProgressContent monster = Instantiate(progressContent);
            monster.name = "ProgressContent" + i;
            monster.transform.position = Vector3.zero;
            monster.transform.localScale = Vector3.one;
            monster.transform.parent = progressContentTransform;

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

    public void CheckProgress()
    {

    }

    public void Receive(RewardClass rewardClass)
    {

    }

    public void SaveProgress()
    {
        playerData.Clear();

        string freeProgress = playerDataBase.FreeProgress;

        if(freeProgress.Length < 1)
        {
            freeProgress = "000000000000000000000000000000";
        }

        playerData.Add("FreeProgress" , freeProgress);
        if (PlayfabManager.instance.isActive) PlayfabManager.instance.SetPlayerData(playerData);


        playerData.Clear();

        string paidProgress = playerDataBase.FreeProgress;

        if (paidProgress.Length < 1)
        {
            paidProgress = "000000000000000000000000000000";
        }

        playerData.Add("PaidProgress", paidProgress);
        if (PlayfabManager.instance.isActive) PlayfabManager.instance.SetPlayerData(playerData);

        Debug.Log("Save Progress");
    }
}
