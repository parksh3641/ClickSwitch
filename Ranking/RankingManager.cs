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
    public RectTransform rankContentParent;
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
    private bool isDelay = false;

    [Space]
    List<RankContent> rankContentList = new List<RankContent>();

    PlayerDataBase playerDataBase;

    private void Awake()
    {
        for(int i = 0; i < 100; i ++)
        {
            RankContent monster = Instantiate(rankContentPrefab) as RankContent;
            monster.name = "RankContent_" + i;
            monster.transform.position = Vector3.zero;
            monster.transform.localScale = Vector3.one;
            monster.transform.parent = rankContentParent;
            monster.gameObject.SetActive(false);

            rankContentList.Add(monster);
        }

        rankingView.SetActive(false);

        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;

        topNumber = -1;
        openNumber = -1;
    }

    public void OpenRanking()
    {
        if(!rankingView.activeSelf)
        {
            rankingView.SetActive(true);

            if(topNumber == -1)
            {
                topNumber = 0;
                topMenuImgArray[0].sprite = topMenuSpriteArray[1];

                openNumber = 0;
                contentImgArray[0].sprite = contentSpriteArray[1];

                ChangeRankingView(0);
            }
        }
        else
        {
            rankingView.SetActive(false);
        }
    }

    public void ChangeTopMenu(int number)
    {
        if (isDelay) return;

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
        if (isDelay) return;

        if (openNumber != number)
        {
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
                    if (PlayfabManager.instance.isActive) PlayfabManager.instance.GetLeaderboarder("DragActionScore", SetRanking);
                }
                else
                {
                    if (PlayfabManager.instance.isActive) PlayfabManager.instance.GetLeaderboarder("DragActionCombo", SetRanking);
                }

                break;
        }

        isDelay = true;
    }

    public void SetRanking(GetLeaderboardResult result)
    {
        int index = 1;
        bool isMine = false;
        bool isCheck = false;
        string nickName = "";


        var curBoard = result.Leaderboard;
        int num = 0;

        for(int i = 0; i < rankContentList.Count; i ++)
        {
            rankContentList[i].transform.localScale = Vector3.one;
            rankContentList[i].gameObject.SetActive(false);
        }

        foreach(PlayerLeaderboardEntry player in curBoard)
        {
            var location = curBoard[num].Profile.Locations[0].CountryCode.Value.ToString().ToLower();

            isMine = false;

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
                isMine = true;
                isCheck = true;

                myRankContent.InitState(index, location, nickName, player.StatValue, false);
            }
            else if(player.DisplayName != null)
            {
                if (player.DisplayName.Equals(GameStateManager.instance.NickName))
                {
                    isMine = true;
                    isCheck = true;

                    myRankContent.InitState(index, location, nickName, player.StatValue, false);
                }
            }

            rankContentList[num].InitState(index, location, nickName, player.StatValue, isMine);
            rankContentList[num].gameObject.SetActive(true);

            index++;
            num++;
        }

        if(!isCheck)
        {
            PlayfabManager.instance.GetPlayerProfile(GameStateManager.instance.PlayfabId, CheckCountry);
        }

        if (PlayfabManager.instance.isActive) PlayfabManager.instance.GetLeaderboarder("Icon", SetIcon);

        rankContentParent.anchoredPosition = new Vector2(0, -9999);
    }

    void CheckCountry(string code)
    {
        int number = 0;
        switch (openNumber)
        {
            case 0:
                if (topNumber == 0)
                {
                    number = playerDataBase.TotalScore;
                }
                else
                {
                    number = playerDataBase.TotalCombo;
                }
                break;
            case 1:
                if (topNumber == 0)
                {
                    number = playerDataBase.BestSpeedTouchScore;
                }
                else
                {
                    number = playerDataBase.BestSpeedTouchCombo;
                }
                break;
            case 2:
                if (topNumber == 0)
                {
                    number = playerDataBase.BestMoleCatchScore;
                }
                else
                {
                    number = playerDataBase.BestMoleCatchCombo;
                }
                break;
            case 3:
                if (topNumber == 0)
                {
                    number = playerDataBase.BestFilpCardScore;
                }
                else
                {
                    number = playerDataBase.BestFilpCardCombo;
                }
                break;
            case 4:
                if (topNumber == 0)
                {
                    number = playerDataBase.BestButtonActionScore;
                }
                else
                {
                    number = playerDataBase.BestButtonActionCombo;
                }
                break;
            case 5:
                if (topNumber == 0)
                {
                    number = playerDataBase.BestTimingActionScore;
                }
                else
                {
                    number = playerDataBase.BestTimingActionCombo;
                }
                break;
            case 6:
                if (topNumber == 0)
                {
                    number = playerDataBase.BestDragActionScore;
                }
                else
                {
                    number = playerDataBase.BestDragActionCombo;
                }
                break;
        }

        myRankContent.InitState(999, code, GameStateManager.instance.NickName, number, false);
    }

    public void SetIcon(GetLeaderboardResult result)
    {
        var curBoard = result.Leaderboard;

        foreach (PlayerLeaderboardEntry player in curBoard)
        {
            for(int i = 0; i < rankContentList.Count; i ++)
            {
                if (rankContentList[i].nickNameText.text.Equals(player.DisplayName) ||
                    rankContentList[i].nickNameText.text.Equals(player.PlayFabId))
                {
                    if(player.StatValue > 0)
                    {
                        rankContentList[i].IconState((IconType)player.StatValue);
                    }

                    continue;
                }
            }
        }

        myRankContent.IconState((IconType)playerDataBase.Icon);

        Invoke("Delay", 0.5f);
    }

    void Delay()
    {
        isDelay = false;
    }
}
