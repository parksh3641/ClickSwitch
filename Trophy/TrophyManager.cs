using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TrophyData
{
    public GamePlayType gamePlayType = GamePlayType.GameChoice1;
    public bool isActive = false; //클리어여부
    public int number = 0; //달성 횟수
    public string date = ""; //달성 날짜
}

public class TrophyManager : MonoBehaviour
{
    public GameObject trophyView;

    public TrophyContent trophyContent;
    public Transform trophyContentTransform;


    List<TrophyContent> trophyContentList = new List<TrophyContent>();



    private void Awake()
    {

    }

    public void Initialize()
    {
        for (int i = 0; i < 6; i++)
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
    }
}
