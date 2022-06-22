using PlayFab.ClientModels;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingManager : MonoBehaviour
{
    public GameObject rankingView;
    public RankContent rankContentPrefab;
    public RankContent myRankContent;
    [Space]
    public GameObject[] scrollViewList;
    public RectTransform[] rankContentParent;
    [Space]
    [Title("TopMenu")]
    public Image[] topMenuImgArray;
    public Sprite[] topMenuSpriteArray;

    [Space]
    [Title("SubMenu")]
    public Image[] contentImgArray;
    public Sprite[] contentSpriteArray;

    private int topNumber = 0;
    private int openNumber = 0;
    private bool isActive = false;

    [Space]
    public List<RankContent> rankContentList = new List<RankContent>();

    public PlayerDataBase playerDataBase;

    private void Awake()
    {
        for(int i = 0; i < 100; i ++)
        {
            RankContent monster = Instantiate(rankContentPrefab) as RankContent;
            monster.name = "RankContent_" + i;
            monster.transform.position = Vector3.zero;
            monster.transform.parent = rankContentParent[0];
            monster.gameObject.SetActive(false);

            rankContentList.Add(monster);
        }

        rankingView.SetActive(false);

        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;
    }

    public void OpenRanking()
    {
        if(!rankingView.activeSelf)
        {
            rankingView.SetActive(true);

            topNumber = -1;
            ChangeTopMenu(0);

            openNumber = -1;
            OpenRankingView(0);
        }
        else
        {
            rankingView.SetActive(false);
        }
    }

    void OpenView(int number)
    {
        for(int i = 0; i < scrollViewList.Length; i ++)
        {
            scrollViewList[i].SetActive(false);
        }

        if(number != -1)
        {
            scrollViewList[number].SetActive(true);
        }
    }

    public void ChangeTopMenu(int number)
    {
        if (topNumber != number)
        {
            topNumber = number;

            for (int i = 0; i < topMenuImgArray.Length; i++)
            {
                topMenuImgArray[i].sprite = topMenuSpriteArray[0];
            }
            topMenuImgArray[number].sprite = topMenuSpriteArray[1];

            ChangeRankingView(openNumber);
        }
    }

    public void OpenRankingView(int number)
    {
        if (openNumber != number)
        {
            OpenView(-1);
            openNumber = number;

            for (int i = 0; i < contentImgArray.Length; i ++)
            {
                contentImgArray[i].sprite = contentSpriteArray[0];
            }
            contentImgArray[number].sprite = contentSpriteArray[1];

            ChangeRankingView(number);
        }
    }

    void ChangeRankingView(int number)
    {
        switch (number)
        {
            case 0:
                if (topNumber == 0)
                {
                    if (PlayfabManager.instance.isActive) PlayfabManager.instance.GetLeaderboarder("TotalScore", SetRanking);
                }
                else
                {
                    if (PlayfabManager.instance.isActive) PlayfabManager.instance.GetLeaderboarder("TotalCombo", SetRanking);
                }

                break;
            case 1:
                if (topNumber == 0)
                {
                    if (PlayfabManager.instance.isActive) PlayfabManager.instance.GetLeaderboarder("SpeedTouchScore", SetRanking);
                }
                else
                {
                    if (PlayfabManager.instance.isActive) PlayfabManager.instance.GetLeaderboarder("SpeedTouchCombo", SetRanking);
                }

                break;
            case 2:
                if (topNumber == 0)
                {
                    if (PlayfabManager.instance.isActive) PlayfabManager.instance.GetLeaderboarder("MoleCatchScore", SetRanking);
                }
                else
                {
                    if (PlayfabManager.instance.isActive) PlayfabManager.instance.GetLeaderboarder("MoleCatchScore", SetRanking);
                }

                break;
            case 3:
                if (topNumber == 0)
                {
                    if (PlayfabManager.instance.isActive) PlayfabManager.instance.GetLeaderboarder("FilpCardScore", SetRanking);
                }
                else
                {
                    if (PlayfabManager.instance.isActive) PlayfabManager.instance.GetLeaderboarder("FilpCardCombo", SetRanking);
                }

                break;
            case 4:
                if (topNumber == 0)
                {
                    if (PlayfabManager.instance.isActive) PlayfabManager.instance.GetLeaderboarder("ButtonActionScore", SetRanking);
                }
                else
                {
                    if (PlayfabManager.instance.isActive) PlayfabManager.instance.GetLeaderboarder("ButtonActionCombo", SetRanking);
                }

                break;
            case 5:
                if (topNumber == 0)
                {
                    if (PlayfabManager.instance.isActive) PlayfabManager.instance.GetLeaderboarder("TimingActionScore", SetRanking);
                }
                else
                {
                    if (PlayfabManager.instance.isActive) PlayfabManager.instance.GetLeaderboarder("TimingActionCombo", SetRanking);
                }

                break;
            case 6:
                if (topNumber == 0)
                {
                    if (PlayfabManager.instance.isActive) PlayfabManager.instance.GetLeaderboarder("FingerSnapScore", SetRanking);
                }
                else
                {
                    if (PlayfabManager.instance.isActive) PlayfabManager.instance.GetLeaderboarder("FingerSnapCombo", SetRanking);
                }

                break;
        }
    }

    public void SetRanking(GetLeaderboardResult result)
    {
        int index = 1;
        bool checkMy = false;
        string nickName = "";


        var curBoard = result.Leaderboard;
        var location = curBoard[index - 1].Profile.Locations[0].CountryCode.Value.ToString().ToLower();
        int num = 0;

        for(int i = 0; i < rankContentList.Count; i ++)
        {
            rankContentList[i].gameObject.SetActive(false);
        }

        foreach(PlayerLeaderboardEntry player in curBoard)
        {
            checkMy = false;

            if (player.DisplayName == null)
            {
                nickName = player.PlayFabId;
            }
            else
            {
                nickName = player.DisplayName;
            }

            if (player.PlayFabId.Equals(GameStateManager.instance.PlayfabId))
            {
                checkMy = true;

                myRankContent.InitState(index, location, nickName, player.StatValue, false);
            }
            else if(player.DisplayName != null)
            {
                if (player.DisplayName.Equals(GameStateManager.instance.NickName))
                {
                    checkMy = true;

                    myRankContent.InitState(index, location, nickName, player.StatValue, false);
                }
            }

            rankContentList[num].InitState(index, location, nickName, player.StatValue, checkMy);
            rankContentList[num].gameObject.SetActive(true);

            index++;
            num++;
        }

        rankContentParent[openNumber].anchoredPosition = new Vector2(0, -9999);
        OpenView(openNumber);
    }
}
