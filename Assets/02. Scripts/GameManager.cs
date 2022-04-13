using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GamePlayType gamePlayType;


    [Title("Prefab")]
    public NormalContent normalContent;



    [Title("GridTransform")]
    public Transform normalContentTransform;



    [Title("Corution")]
    public IEnumerator timerCoroutine;



    WaitForSeconds waitForSeconds = new WaitForSeconds(1);



    [Title("Value")]
    private int nowIndex = 0; //현재 값
    private int setIndex = 1; //세팅용 값
    private int countIndex = 0; //카운팅 값


    [Title("List")]
    private List<int> numberList = new List<int>();
    private List<NormalContent> normalContentList = new List<NormalContent>();


    [Title("Manager")]
    public UIManager uiManager;
    public SoundManager soundManager;


    public delegate void GameEvent();
    public static event GameEvent GameStart, GamePause, GameEnd;

    public delegate void ScoreEvent(int number);
    public static event ScoreEvent PlusScore, MinusScore;

    private void Awake()
    {
        numberList.Clear();

        for (int i = 0; i < 9; i ++)
        {
            NormalContent content = Instantiate(normalContent);
            content.transform.localPosition = new Vector3(0, 0, 0);
            content.transform.localScale = new Vector3(1, 1, 1);
            content.transform.parent = normalContentTransform;
            content.gameObject.SetActive(false);
            normalContentList.Add(content);
        }
    }

    private void Start()
    {

    }

    private void OnApplicationFocus(bool focus)
    {
        
    }

    #region Setting
    private List<T> ShuffleList<T>(List<T> list)
    {
        int random1, random2;
        T temp;

        for (int i = 0; i < list.Count; ++i)
        {
            random1 = Random.Range(0, list.Count);
            random2 = Random.Range(0, list.Count);


            temp = list[random1];
            list[random1] = list[random2];
            list[random2] = temp;
        }

        return list;
    }

    private void CreateUnDuplicateRandom()
    {
        ShuffleList(normalContentList);

        for (int i = 0; i < normalContentList.Count; i++)
        {
            normalContentList[i].Reset(setIndex);
            normalContentList[i].gameObject.SetActive(false);
            normalContentList[i].gameObject.SetActive(true);

            setIndex++;
        }

        normalContentList[0].First();

        countIndex = setIndex;
    }

    #endregion

    #region Button
    public void OnGameStartButton() //게임 시작 버튼
    {
        uiManager.OpenGamePlayUI(gamePlayType);

        nowIndex = 0;
        setIndex = 1;
        countIndex = 0;

        CreateUnDuplicateRandom();

        GameStart();
    }

    public void OpenGameMenuButton() //게임 시작전 모드 선택 창 열기
    {
        uiManager.OpenMenu();
    }

    public void OnSetGameType1() //모드 선택 창에서 옵션 선택
    {
        gamePlayType = GamePlayType.Normal;

        uiManager.CloseMenu();
    }

    #endregion

    #region Game
    public void CheckNumber(int index, System.Action<bool> action)
    {
        if(nowIndex + 1 == index)
        {
            Debug.Log("Success");
            action(true);

            PlusScore(10);
            nowIndex++;

            if(nowIndex >= countIndex - 1)
            {
                Debug.Log("Reset");

                CreateUnDuplicateRandom();
            }
        }
        else
        {
            Debug.Log("Failure");
            action(false);

            MinusScore(5);
        }
    }

    public void OnGameStart()
    {
        GameStart();


    }

    public void OnGamePause()
    {
        GamePause();


    }

    public void OnGameEnd()
    {
        GameEnd();


    }

    #endregion

    #region Corution


    #endregion
}
