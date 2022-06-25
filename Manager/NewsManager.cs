using PlayFab.ClientModels;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewsManager : MonoBehaviour
{
    public GameObject newsView;

    public NewsContent newsContent;
    public RectTransform newsContentTransform;

    [Title("Read More")]
    public GameObject infoView;
    public Text infoTitleText;
    public Text infoBodyText;

    int countNews = 0;

    public List<NewsContent> newsContentList = new List<NewsContent>();
    private List<TitleNewsItem> newsInfoList = new List<TitleNewsItem>();
    private void Awake()
    {
        newsView.SetActive(false);
        infoView.SetActive(false);

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
            newsContentList[i].InitState(i, item[i].Title, item[i].Timestamp);
            newsContentList[i].newsManager = this;
            newsInfoList.Add(item[i]);
            newsContentList[i].gameObject.SetActive(true);
        }

        newsContentTransform.anchoredPosition = new Vector3(0, -999, 0);
    }

    public void OpenReadMore(int number, string title)
    {
        infoView.SetActive(true);

        infoTitleText.text = title;
        infoBodyText.text = newsInfoList[number].Body;
        infoBodyText.rectTransform.anchoredPosition = new Vector3(0, -305f, 0);
    }

    public void CloseReadMore()
    {
        infoView.SetActive(false);
    }
}
