using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AchievementData
{
    public GamePlayType achievementType = GamePlayType.GameChoice1;
    public List<string> achievementList = new List<string>();
}
public class AchievementManager : MonoBehaviour
{
    AchievementData achievementContent;

    







    public PlayerDataBase playerDataBase;

    private Dictionary<string, string> playerData = new Dictionary<string, string>();

    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;

        OnReset();
    }

    public void AddAchieveContentData(int number) //매개변수는 뭘로 주어야되고
    {
        //데이터베이스에서 로드

        //값 변경


        //데이터베이스에 저장

        //save to playfab 진행
    }

    public void OnReset()
    {
        achievementContent.achievementType = GamePlayType.GameChoice1;

        achievementContent.achievementList.Clear();
    }

    public void SaveToPlayfab()
    {
        Debug.Log("Save to Playfab");
        playerData.Add(achievementContent.achievementType.ToString(), JsonUtility.ToJson(achievementContent));

        if (PlayfabManager.instance.isActive) PlayfabManager.instance.SetPlayerData(playerData);
    }



}
