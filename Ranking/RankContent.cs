using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankContent : MonoBehaviour
{
    public GameObject frame;

    public Image banner;
    public Text indexText;
    public Image indexRankImg;
    public Sprite[] rankIconList;
    public Image iconImg;
    public Image countryImg;
    public Text nickNameText;
    public Text scoreText;

    ImageDataBase imageDataBase;

    private void Awake()
    {
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;

        indexText.text = "";
        nickNameText.text = "";
        scoreText.text = "";
    }

    public void InitState(int index, string country, string nickName, int score, bool checkMy)
    {
        if(index <= 3)
        {
            indexRankImg.enabled = true;
            indexRankImg.sprite = rankIconList[index - 1];
        }
        else
        {
            indexRankImg.enabled = false;
        }

        banner.sprite = imageDataBase.GetBannerArray(BannerType.Banner_0);
        indexText.text = index.ToString();
        nickNameText.text = nickName;
        iconImg.sprite = imageDataBase.GetProfileIconArray(IconType.Icon_0);
        countryImg.sprite = Resources.Load<Sprite>("Country/" + country);
        scoreText.text = score.ToString();


        if (index == 999)
        {
            indexText.text = "-";
        }

        frame.SetActive(checkMy);
    }

    public void IconState(IconType type)
    {
        iconImg.sprite = imageDataBase.GetProfileIconArray(type);
    }

    public void BannerState(BannerType type)
    {
        banner.sprite = imageDataBase.GetBannerArray(type);
    }
}
