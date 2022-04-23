using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GamePlayType gamePlayType;

    [Title("Mode")]
    public ModeContent[] modeContentArray;


    [Title("Prefab")]
    public NormalContent normalContent;



    [Title("GridTransform")]
    public Transform normalTransform;
    public Transform moleCatchTransform;
    public Transform filpCardTransform;


    [Title("UI")]
    public Text gameModeText;


    [Title("Corution")]
    public IEnumerator timerCoroutine;



    WaitForSeconds waitForSeconds = new WaitForSeconds(1);
    WaitForSeconds waitForHalfSeconds = new WaitForSeconds(0.5f);
    WaitForSeconds waitForMoleCatchSeconds;



    [Title("Value")]
    private int nowIndex = 0; //현재 값
    private int setIndex = 1; //세팅용 값
    private int countIndex = 0; //카운팅 값

    private int moleIndex = 0; //두더지 위치 값

    private int saveIndex = 0; //세이브 값

    [Title("bool")]
    private bool isActive = false;

    [Title("List")]
    private List<int> numberList = new List<int>();
    private List<NormalContent> normalContentList = new List<NormalContent>();
    private List<NormalContent> moleCatchContentList = new List<NormalContent>();
    private List<NormalContent> filpCardList = new List<NormalContent>();


    [Title("Manager")]
    public UIManager uiManager;
    public SoundManager soundManager;


    public delegate void GameEvent();
    public static event GameEvent eGameStart, eGamePause, eGameEnd;

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
            content.transform.parent = normalTransform;
            content.gameObject.SetActive(false);
            normalContentList.Add(content);
        }

        for (int i = 0; i < 9; i++)
        {
            NormalContent content = Instantiate(normalContent);
            content.transform.localPosition = new Vector3(0, 0, 0);
            content.transform.localScale = new Vector3(1, 1, 1);
            content.transform.parent = moleCatchTransform;
            content.gameObject.SetActive(false);
            moleCatchContentList.Add(content);
        }

        for (int i = 0; i < 16; i++)
        {
            NormalContent content = Instantiate(normalContent);
            content.transform.localPosition = new Vector3(0, 0, 0);
            content.transform.localScale = new Vector3(1, 1, 1);
            content.transform.parent = filpCardTransform;
            content.gameObject.SetActive(false);
            filpCardList.Add(content);
        }
    }

    private void Start()
    {
        gameModeText.text = LocalizationManager.instance.GetString(gamePlayType.ToString());
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
            normalContentList[i].NormalReset(setIndex);
            normalContentList[i].gameObject.SetActive(false);
            normalContentList[i].gameObject.SetActive(true);

            setIndex++;
        }

        normalContentList[0].First();

        countIndex = setIndex;
    }

    private void CreateMoleRandom()
    {
        for (int i = 0; i < moleCatchContentList.Count; i++)
        {
            moleCatchContentList[i].OnReset();
            moleCatchContentList[i].gameObject.SetActive(false);
            moleCatchContentList[i].gameObject.SetActive(true);
        }

        waitForMoleCatchSeconds = new WaitForSeconds(ValueManager.instance.GetMoleTimer());
    }

    private void CreateFilpCardRandom()
    {
        ShuffleList(filpCardList);

        int j = 1;
        int k = 0;

        for(int i = 0; i < filpCardList.Count; i ++)
        {
            filpCardList[i].FilpCardReset(k);
            filpCardList[i].gameObject.SetActive(false);
            filpCardList[i].gameObject.SetActive(true);

            if (j % 2 == 0)
            {
                k++;
            }

            j++;
        }
    }

    #endregion

    #region Button
    public void OnGameStartButton() //게임 시작 버튼
    {
        if (PlayfabManager.instance.isActive) PlayfabManager.instance.GetTitleInternalData(gamePlayType.ToString(), GameStart);
    }

    public void GameStart(bool check)
    {
        if(!check)
        {
            Debug.Log("해당 게임모드가 열려있지 않습니다.");
            return;
        }

        uiManager.OpenGamePlayUI(gamePlayType);

        nowIndex = 0;
        setIndex = 1;
        countIndex = 0;

        for (int i = 0; i < normalContentList.Count; i++)
        {
            normalContentList[i].Initialize(gamePlayType);
        }

        for (int i = 0; i < moleCatchContentList.Count; i++)
        {
            moleCatchContentList[i].Initialize(gamePlayType);
        }

        for (int i = 0; i < filpCardList.Count; i++)
        {
            filpCardList[i].Initialize(gamePlayType);
        }

        switch (gamePlayType)
        {
            case GamePlayType.GameChoice1:
                CreateUnDuplicateRandom();
                break;
            case GamePlayType.GameChoice2:
                CreateMoleRandom();
                break;
            case GamePlayType.GameChoice3:
                saveIndex = -1;
                CreateFilpCardRandom();
                break;
        }

        eGameStart();
    }

    public void OpenGameMenuButton() //게임 시작전 모드 선택 창 열기
    {
        uiManager.OpenMenu();
    }

    public void OnSetGameType(int number) //모드 선택 창에서 옵션 선택
    {
        switch(number)
        {
            case 0:
                gamePlayType = GamePlayType.GameChoice1;

                break;
            case 1:
                gamePlayType = GamePlayType.GameChoice2;

                break;
            case 2:
                gamePlayType = GamePlayType.GameChoice3;

                break;
        }

        GameStateManager.instance.GamePlayType = gamePlayType;

        gameModeText.text = LocalizationManager.instance.GetString(gamePlayType.ToString());

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

    public void CheckMole(int index, System.Action<bool> action)
    {
        if(index == moleIndex)
        {
            Debug.Log("Success");
            action(true);

            PlusScore(10);
        }
        else
        {
            Debug.Log("Failure");
            action(false);

            MinusScore(5);
        }
    }

    System.Action<int> saveAction;

    public void CheckFilpCard(int index, bool isactive, System.Action<int> action)
    {
        if (!isActive || !isactive) return;

        Debug.Log("Click");

        if(saveIndex == -1)
        {
            Debug.Log("Choice");
            saveIndex = index;

            action?.Invoke(0);

            saveAction = action;
        }
        else
        {
            if(saveIndex == index)
            {
                Debug.Log("Success");
                action?.Invoke(1);
                saveAction?.Invoke(1);

                PlusScore(10);

                saveIndex = -1;
                nowIndex++;

                if(nowIndex >= filpCardList.Count / 2)
                {
                    Debug.Log("Card Reset");

                    saveIndex = -1;
                    nowIndex = 0;
                    CreateFilpCardRandom();
                    StartCoroutine("FilpCardCorution");
                }
            }
            else
            {
                Debug.Log("Failure");
                action?.Invoke(2);
                saveAction?.Invoke(2);

                saveIndex = -1;

                MinusScore(5);
            }
        }
    }

    public void OnGameStart()
    {
        switch (gamePlayType)
        {
            case GamePlayType.GameChoice1:

                break;
            case GamePlayType.GameChoice2:
                StartCoroutine("MoleCatchCorution");
                break;
            case GamePlayType.GameChoice3:
                StartCoroutine("FilpCardCorution");
                break;
        }
    }

    public void OnGamePause()
    {
        eGamePause();

    }

    public void OnGameEnd()
    {
        StopAllCoroutines();


    }

    #endregion

    #region Corution
    IEnumerator MoleCatchCorution()
    {
        ShuffleList(moleCatchContentList);

        moleIndex = 0;

        moleCatchContentList[0].First();

        yield return waitForHalfSeconds;

        moleIndex = -1;

        moleCatchContentList[0].OnReset();

        yield return waitForMoleCatchSeconds;

        StartCoroutine(MoleCatchCorution());
    }

    IEnumerator FilpCardCorution()
    {
        isActive = false;
        eGamePause();

        for (int i = 0; i < filpCardList.Count; i ++ )
        {
            filpCardList[i].filpCardImg.enabled = true;
        }

        yield return new WaitForSeconds(ValueManager.instance.GetCardTimer());

        for (int i = 0; i < filpCardList.Count; i++)
        {
            filpCardList[i].filpCardImg.enabled = false;
        }

        isActive = true;
        eGamePause();
    }

    #endregion
}
