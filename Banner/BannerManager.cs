using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class BannerClass
{
    public BannerType bannerType = BannerType.Banner_0;
    public int count = 0;
}

public class BannerManager : MonoBehaviour
{
    public GameObject bannerView;

    public Text nickNameText;

    private void Awake()
    {
        bannerView.SetActive(false);
    }

    private void Start()
    {
        
    }



    public void OpenBanner()
    {
        if (!bannerView.activeSelf)
        {
            bannerView.SetActive(true);

            CheckBanner();
        }
        else
        {
            bannerView.SetActive(false);
        }
    }

    public void Initialize()
    {

    }

    void CheckBanner()
    {
        if (GameStateManager.instance.NickName != null)
        {
            nickNameText.text = GameStateManager.instance.NickName;
        }
        else
        {
            nickNameText.text = GameStateManager.instance.CustomId;
        }
    }
}
