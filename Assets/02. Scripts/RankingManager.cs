using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingManager : MonoBehaviour
{
    public GameObject rankingView;
    public RankContent rankContentPrefab;
    public RankContent myRankContent;
    [Space]
    public GameObject[] scrollViewList;
    public RectTransform[] rankContentParent;
    [Space]

    public int openNumber = 0;
    public bool isActive = false;

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
    }

    public void OpenRanking()
    {
        if(!rankingView.activeSelf)
        {
            rankingView.SetActive(true);

            OpenView(-1);
            openNumber = 0;

            if (PlayfabManager.instance.isActive) PlayfabManager.instance.GetLeaderboarder("Score", SetRanking);
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

    public void OpenRankingView(int number)
    {
        if (openNumber != number)
        {
            OpenView(-1);
            openNumber = number;

            switch (number)
            {
                case 0:
                    if (PlayfabManager.instance.isActive) PlayfabManager.instance.GetLeaderboarder("Score", SetRanking);

                    break;
                case 1:
                    if (PlayfabManager.instance.isActive) PlayfabManager.instance.GetLeaderboarder("Score", SetRanking);

                    break;
                case 2:
                    if (PlayfabManager.instance.isActive) PlayfabManager.instance.GetLeaderboarder("Score", SetRanking);

                    break;
                case 3:
                    if (PlayfabManager.instance.isActive) PlayfabManager.instance.GetLeaderboarder("Score", SetRanking);

                    break;
            }
        }
    }

    public void SetRanking(GetLeaderboardResult result)
    {
        int index = 1;
        bool checkMy = false;
        string nickName = "";


        var curBoard = result.Leaderboard;
        //var location = curBoard[index - 1].Profile.Locations[0].CountryCode.Value.ToString().ToLower();
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

            myRankContent.InitState(999, null, GameStateManager.instance.PlayfabId, playerDataBase.BestScore, false);

            if (player.PlayFabId.Equals(GameStateManager.instance.PlayfabId))
            {
                checkMy = true;

                myRankContent.InitState(index, null, nickName, player.StatValue, false);
            }

            rankContentList[num].InitState(index, null, nickName, player.StatValue, checkMy);
            rankContentList[num].gameObject.SetActive(true);

            index++;
            num++;
        }

        rankContentParent[openNumber].anchoredPosition = new Vector2(0, -9999);
        OpenView(openNumber);
    }
}
