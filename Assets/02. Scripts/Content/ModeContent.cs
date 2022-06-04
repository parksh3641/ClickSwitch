using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ModeContent : MonoBehaviour
{
    public GameModeType gameModeType;
    public GamePlayType gamePlayType;

    public UnityEvent clickEvent;

    public LocalizationContent titleText;
    public Text nextEventText;

    [Title("UI")]
    public Image backgroundImg;
    public Image iconImg;

    public Sprite[] backgroundImgArray;
    public Sprite[] iconImgArray;

    DateTime serverTime;

    public ImageDataBase imageDataBase;

    private void Awake()
    {
        titleText.name = gamePlayType.ToString();
        nextEventText.text = "";

        clickEvent.AddListener(() => { GameObject.FindWithTag("GameManager").GetComponent<GameManager>().ChoiceGameType((int)gamePlayType); });

        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;

        iconImgArray = imageDataBase.GetIconArray();
    }

    private void Start()
    {
        backgroundImg.sprite = backgroundImgArray[(int)gameModeType];
        iconImg.sprite = iconImgArray[(int)gamePlayType];
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void OnClick()
    {
        clickEvent.Invoke();
    }

    public void SetNextEventTime(DateTime time)
    {
        serverTime = time;

        StartCoroutine(RemainTimerCourtion());
    }

    IEnumerator RemainTimerCourtion()
    {
        serverTime = serverTime.AddSeconds(-1);

        nextEventText.text = serverTime.ToString("hh:mm:ss");

        yield return new WaitForSeconds(1f);

        StartCoroutine(RemainTimerCourtion());
    }

}
