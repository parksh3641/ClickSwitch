using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockManager : MonoBehaviour
{
    public GameObject lockView; //뭐가 열였는지 알려줌

    public GameObject[] lockIcon;


    [Title("Lock")]
    public GameObject[] menuIcon;

    [Title("Money Plus")]
    public GameObject[] moneyPlusIcon;

    [Title("GameEventMode")]
    public GameObject[] gameEventMode;
    public RectTransform scrollView;

    private int level = 0;

    PlayerDataBase playerDataBase;

    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;

        lockView.SetActive(false);
    }

    [Button]
    public void Test()
    {
        level += 1;
        Initialize();
    }

    public void Initialize()
    {
        for (int i = 0; i < lockIcon.Length; i++)
        {
            lockIcon[i].SetActive(false);
        }

        for (int i = 0; i < menuIcon.Length; i++)
        {
            menuIcon[i].SetActive(false);
        }

        moneyPlusIcon[0].SetActive(false);
        moneyPlusIcon[1].SetActive(false);

        gameEventMode[0].SetActive(true);
        gameEventMode[1].SetActive(false);

        scrollView.offsetMax = new Vector2(0, -140);

        level = playerDataBase.Level;

        if (level >= 1)
        {
            menuIcon[0].SetActive(true); //퀘스트

            if(playerDataBase.LockTutorial == 0)
            {
                lockView.SetActive(true);

                lockIcon[0].SetActive(true);

                playerDataBase.LockTutorial = 1;

                if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdatePlayerStatisticsInsert("LockTutorial", playerDataBase.LockTutorial);
            }
        }

        if (level >= 2)
        {
            menuIcon[1].SetActive(true); //강화

            if (playerDataBase.LockTutorial == 1)
            {
                lockView.SetActive(true);

                lockIcon[1].SetActive(true);

                playerDataBase.LockTutorial = 2;

                if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdatePlayerStatisticsInsert("LockTutorial", playerDataBase.LockTutorial);
            }
        }

        if (level >= 3)
        {
            menuIcon[2].SetActive(true); //상점
            menuIcon[3].SetActive(true); //진척도

            moneyPlusIcon[0].SetActive(true);
            moneyPlusIcon[1].SetActive(true);

            if (playerDataBase.LockTutorial == 2)
            {
                lockView.SetActive(true);

                lockIcon[2].SetActive(true);
                lockIcon[3].SetActive(true);

                playerDataBase.LockTutorial = 3;

                if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdatePlayerStatisticsInsert("LockTutorial", playerDataBase.LockTutorial);
            }
        }

        if (level >= 4)
        {
            menuIcon[4].SetActive(true); //트로피

            gameEventMode[0].SetActive(false); //게임 이벤트모드
            gameEventMode[1].SetActive(true);

            scrollView.offsetMax = new Vector2(0, -400);

            if (playerDataBase.LockTutorial == 3)
            {
                lockView.SetActive(true);

                lockIcon[4].SetActive(true);

                playerDataBase.LockTutorial = 4;

                if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdatePlayerStatisticsInsert("LockTutorial", playerDataBase.LockTutorial);
            }
        }

        if (level >= 5)
        {

        }

        if (level >= 6)
        {

        }

        if (level >= 7)
        {

        }
    }

    public void CloseLockView()
    {
        lockView.SetActive(false);
    }
}
