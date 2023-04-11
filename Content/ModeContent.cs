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
    public UnityEvent changeLevelEvent;

    public LocalizationContent playTypeText;
    public LocalizationContent modeTypeText;

    [Title("UI")]
    public Image backgroundImg;
    public Image iconImg;

    public GameObject lockObj;

    Sprite[] iconImgArray;
    Sprite[] modeBackgroundImgArray;


    public ImageDataBase imageDataBase;

    private void Awake()
    {
        playTypeText.localizationName = gamePlayType.ToString();
        //modeTypeText.name = gameModeType.ToString();

        clickEvent.AddListener(() => { GameObject.FindWithTag("GameManager").GetComponent<GameManager>().ChoiceGameType(gamePlayType, gameModeType); });
        changeLevelEvent.AddListener(() => { GameObject.FindWithTag("GameManager").GetComponent<GameManager>().ChangeGameMode(gamePlayType); });

        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;

        iconImgArray = imageDataBase.GetIconArray();
        modeBackgroundImgArray = imageDataBase.GetModeBackgroundArray();

        if (lockObj != null) lockObj.SetActive(true);
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

    public void OnClickChangeLevel()
    {
        changeLevelEvent.Invoke();
    }

    public void Initialize(GamePlayType play, GameModeType mode)
    {
        backgroundImg.sprite = modeBackgroundImgArray[(int)mode];
        iconImg.sprite = iconImgArray[(int)play];

        gamePlayType = play;
        gameModeType = mode;

        playTypeText.localizationName = gamePlayType.ToString();
        modeTypeText.localizationName = gameModeType.ToString();

        playTypeText.ReLoad();
        modeTypeText.ReLoad();
    }

    public void UnLock()
    {
        lockObj.SetActive(false);
    }

    public void ChangeGameMode(GameModeType mode)
    {
        gameModeType = mode;

        modeTypeText.localizationName = gameModeType.ToString();
        modeTypeText.ReLoad();

        backgroundImg.sprite = modeBackgroundImgArray[(int)gameModeType];
    }
}
