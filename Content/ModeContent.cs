using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ModeContent : MonoBehaviour
{
    public GamePlayType gamePlayType;
    public GameModeType gameModeType;

    public UnityEvent clickEvent;

    public LocalizationContent playTypeText;
    public LocalizationContent modeTypeText;

    [Title("UI")]
    public Image backgroundImg;
    public Image iconImg;

    Sprite[] iconImgArray;
    Sprite[] modeBackgroundImgArray;


    public ImageDataBase imageDataBase;

    private void Awake()
    {
        playTypeText.name = gamePlayType.ToString();
        modeTypeText.name = gameModeType.ToString();

        clickEvent.AddListener(() => { GameObject.FindWithTag("GameManager").GetComponent<GameManager>().ChoiceGameType(gamePlayType, gameModeType); });

        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;

        iconImgArray = imageDataBase.GetIconArray();
        modeBackgroundImgArray = imageDataBase.GetModeBackgroundArray();
    }

    private void Start()
    {
        backgroundImg.sprite = modeBackgroundImgArray[(int)gameModeType];
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

    public void Initialize(GamePlayType play, GameModeType mode)
    {
        backgroundImg.sprite = modeBackgroundImgArray[(int)mode];
        iconImg.sprite = iconImgArray[(int)play];

        gamePlayType = play;
        gameModeType = mode;

        playTypeText.name = gamePlayType.ToString();
        modeTypeText.name = gameModeType.ToString();

        playTypeText.ReLoad();
        modeTypeText.ReLoad();
    }
}
