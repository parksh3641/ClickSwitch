//using Google.Play.Review;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviewManager : MonoBehaviour
{
    public GameObject appReviewView;

    private void Awake()
    {
        appReviewView.SetActive(false);
    }

    public void OpenReview()
    {
        appReviewView.SetActive(true);

        GameStateManager.instance.InAppReview = true;
    }

    public void CloseReview()
    {
        appReviewView.SetActive(false);
    }

    public void OpenURL()
    {
        appReviewView.SetActive(false);

#if UNITY_ANDROID
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.unity3d.toucharcade");
#elif UNITY_IOS
        Application.OpenURL("https://apps.apple.com/us/app/gosu-of-touch-tap-arcade/id1637056029");
#else
        Application.OpenURL("https://apps.apple.com/us/app/gosu-of-touch-tap-arcade/id1637056029");
#endif
    }
}
