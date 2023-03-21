using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class TrophyData
{
    public GamePlayType gamePlayType = GamePlayType.GameChoice1;
    public bool isActive = false;
    public int number = 0;
    public string date = "";
}

public class TrophyManager : MonoBehaviour
{
    public GameObject trophyView;

    public TrophyContent trophyContent;
    public RectTransform trophyContentTransform;

    public Text plusScoreText;


    PlayerDataBase playerDataBase;

    List<TrophyContent> trophyContentList = new List<TrophyContent>();



    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;

        trophyView.SetActive(false);

        trophyContentTransform.anchoredPosition = new Vector2(0, -999);
    }

    public void Initialize()
    {
        for (int i = 0; i < 8; i++)
        {
            TrophyContent monster = Instantiate(trophyContent);
            monster.transform.parent = trophyContentTransform;
            monster.transform.position = Vector3.zero;
            monster.transform.rotation = Quaternion.identity;
            monster.transform.localScale = Vector3.one;

            trophyContentList.Add(monster);
        }
    }

    public void OpenTrophy()
    {
        if (!trophyView.activeSelf)
        {
            trophyView.SetActive(true);

            CheckTrophy();
        }
        else
        {
            trophyView.SetActive(false);
        }
    }

    void CheckTrophy()
    {
        for(int i = 0; i < trophyContentList.Count; i ++)
        {
            trophyContentList[i].Initialize(GamePlayType.GameChoice1 + i);
        }

        int plusScore = playerDataBase.GetTrophyHoldNumber();

        plusScoreText.text = LocalizationManager.instance.GetString("PlusScoreInfo") + " +" + (1f * plusScore).ToString() + "%";
    }
}
