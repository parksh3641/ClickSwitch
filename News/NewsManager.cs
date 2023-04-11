using PlayFab.ClientModels;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewsManager : MonoBehaviour
{
    public GameObject newsView;

    public GameObject alarm;
    public GameObject mainAlarm;

    public NewsContent newsContent;
    public RectTransform newsContentTransform;

    public Font normalFont;
    public Font bengaliFont;

    [Title("Read More")]
    public GameObject infoView;
    public Text infoTitleText;
    public Text infoBodyText;

    int countNews = 0;

    public List<NewsContent> newsContentList = new List<NewsContent>();
    private List<TitleNewsItem> newsInfoList = new List<TitleNewsItem>();

    PlayerDataBase playerDataBase;

    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;

        newsView.SetActive(false);
        infoView.SetActive(false);

        alarm.SetActive(false);
        mainAlarm.SetActive(false);

        newsContentList.Clear();

        for(int i = 0; i < 10; i ++)
        {
            NewsContent monster = Instantiate(newsContent);
            monster.transform.parent = newsContentTransform;
            monster.transform.position = Vector3.zero;
            monster.transform.rotation = Quaternion.identity;
            monster.transform.localScale = Vector3.one;

            monster.gameObject.SetActive(false);

            newsContentList.Add(monster);
        }
    }

    public void Initialize()
    {
        if(playerDataBase.NewsAlarm > 0)
        {
            alarm.SetActive(true);
            mainAlarm.SetActive(true);
        }
    }

    public void OpenNews()
    {
        if (!newsView.activeSelf)
        {
            newsView.SetActive(true);

            for(int i = 0; i < newsContentList.Count; i++)
            {
                newsContentList[i].gameObject.SetActive(false);
            }

            if (PlayfabManager.instance.isActive) PlayfabManager.instance.ReadTitleNews(ReadTitleNews);
        }
        else
        {
            newsView.SetActive(false);
        }
    }

    public void ReadTitleNews(List<TitleNewsItem> item)
    {
        countNews = item.Count - 1;

        for (int i = 0; i < newsContentList.Count; i++)
        {
            newsContentList[i].gameObject.SetActive(false);
        }

        newsInfoList.Clear();

        for (int i = 0; i < item.Count; i++)
        {
            if (GameStateManager.instance.Language == LanguageType.Bengali)
            {
                newsContentList[i].InitState(i, item[i].Title, item[i].Timestamp,bengaliFont);
            }
            else
            {
                newsContentList[i].InitState(i, item[i].Title, item[i].Timestamp, normalFont);
            }
            newsContentList[i].newsManager = this;
            newsInfoList.Add(item[i]);
            newsContentList[i].gameObject.SetActive(true);
        }

        newsContentTransform.anchoredPosition = new Vector3(0, -999, 0);

        playerDataBase.NewsAlarm = 0;
        PlayfabManager.instance.UpdatePlayerStatisticsInsert("NewsAlarm", 0);

        alarm.SetActive(false);
        mainAlarm.SetActive(false);
    }

    public void OpenReadMore(int number, string title)
    {
        infoView.SetActive(true);

        if (GameStateManager.instance.Language == LanguageType.Bengali)
        {
            infoTitleText.font = bengaliFont;
            infoBodyText.font = bengaliFont;
        }
        else
        {
            infoTitleText.font = normalFont;
            infoBodyText.font = normalFont;
        }

        infoTitleText.text = title;
        infoBodyText.text = newsInfoList[number].Body;
        infoBodyText.rectTransform.anchoredPosition = new Vector3(0, -305f, 0);
    }

    public void CloseReadMore()
    {
        infoView.SetActive(false);
    }

    public void OpenDiscord()
    {
        Application.OpenURL("https://discord.gg/pE9yNASZ3P");
    }
}
