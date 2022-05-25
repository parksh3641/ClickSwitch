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
    public ButtonActionContent buttonActionContent;


    [Title("GridTransform")]
    public Transform normalTransform;
    public Transform moleCatchTransform;
    public Transform filpCardTransform;
    public Transform buttonActionUpTransform;
    public Transform buttonActionDownTransform;


    [Title("UI")]
    public Text gameModeText;


    [Title("Corution")]
    public IEnumerator timerCoroutine;



    WaitForSeconds waitForSeconds = new WaitForSeconds(1);
    WaitForSeconds waitForHalfSeconds = new WaitForSeconds(0.5f);
    WaitForSeconds waitForMoleCatchSeconds;
    WaitForSeconds waitForMoleNextSeconds;



    [Title("Value")]
    private int nowIndex = 0; //현재 값
    private int setIndex = 1; //세팅용 값
    private int countIndex = 0; //카운팅 값

    private int moleIndex = 0; //두더지 위치 값

    private int filpCardIndex = 0; //세이브 값

    private int buttonActionNumber = 0;
    private int buttonActionLevelIndex = 0;
    private int buttonActionIndex = 0;

    [Title("bool")]
    private bool isActive = false;

    [Title("List")]
    private Queue<int> numberList = new Queue<int>();
    private List<NormalContent> normalContentList = new List<NormalContent>();
    private List<NormalContent> moleCatchContentList = new List<NormalContent>();
    private List<NormalContent> filpCardList = new List<NormalContent>();
    private List<ButtonActionContent> buttonActionUpList = new List<ButtonActionContent>();
    private List<NormalContent> buttonActionDownList = new List<NormalContent>();

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

        for (int i = 0; i < 6; i++)
        {
            ButtonActionContent content = Instantiate(buttonActionContent);
            content.transform.localPosition = new Vector3(0, 0, 0);
            content.transform.localScale = new Vector3(1, 1, 1);
            content.transform.parent = buttonActionUpTransform;
            content.gameObject.SetActive(false);
            buttonActionUpList.Add(content);
        }

        for (int i = 0; i < 6; i++)
        {
            NormalContent content = Instantiate(normalContent);
            content.transform.localPosition = new Vector3(0, 0, 0);
            content.transform.localScale = new Vector3(1, 1, 1);
            content.transform.parent = buttonActionDownTransform;
            content.gameObject.SetActive(false);
            buttonActionDownList.Add(content);
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

        normalContentList[0].NormalFirst();

        countIndex = setIndex;
    }

    private void CreateMoleRandom()
    {
        for (int i = 0; i < moleCatchContentList.Count; i++)
        {
            moleCatchContentList[i].MoleReset();
            moleCatchContentList[i].gameObject.SetActive(false);
            moleCatchContentList[i].gameObject.SetActive(true);
        }

        waitForMoleCatchSeconds = new WaitForSeconds(ValueManager.instance.GetMoleCatchTimer());
        waitForMoleNextSeconds = new WaitForSeconds(ValueManager.instance.GetMoleNextTimer());
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

    private void CreateButtonActionRandom()
    {
        for(int i = 0; i < buttonActionUpList.Count; i ++)
        {
            buttonActionUpList[i].gameObject.SetActive(false);
        }

        //ShuffleList(buttonActionUpList);

        numberList.Clear();

        int number = 0;

        for (int i = 0; i < buttonActionLevelIndex; i++)
        {
            number = Random.Range(0, buttonActionUpList.Count);
            numberList.Enqueue(number);
            buttonActionUpList[i].Initialize(number);
            buttonActionUpList[i].gameObject.SetActive(true);
        }

        buttonActionNumber = 0;

        buttonActionIndex = numberList.Dequeue();
    }

    #endregion

    #region Button
    public void OnGameStartButton() //게임 시작 버튼
    {
        if (PlayfabManager.instance.isActive) PlayfabManager.instance.GetTitleInternalData(gamePlayType.ToString(), InitializeGame);
    }

    public void OpenGameMenuButton() //게임 시작전 모드 선택 창 열기
    {
        uiManager.OpenMenu();

        if (PlayfabManager.instance.isActive) PlayfabManager.instance.GetServerTime(SetModeContent);
    }

    private void SetModeContent(System.DateTime time)
    {
        for (int i = 0; i < modeContentArray.Length; i++)
        {
            modeContentArray[i].SetNextEventTime(time);
        }
    }

    public void ChoiceGameType(int number) //모드 선택 창에서 옵션 선택
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
            case 3:
                gamePlayType = GamePlayType.GameChoice4;

                break;
            case 4:
                gamePlayType = GamePlayType.GameChoice5;

                break;
            case 5:
                gamePlayType = GamePlayType.GameChoice6;

                break;
            case 6:
                gamePlayType = GamePlayType.GameChoice7;

                break;
            case 7:
                gamePlayType = GamePlayType.GameChoice8;

                break;
        }

        GameStateManager.instance.GamePlayType = gamePlayType;

        gameModeText.text = LocalizationManager.instance.GetString(gamePlayType.ToString());

        uiManager.CloseMenu();
    }

    #endregion

    #region Game
    public void InitializeGame(bool check)
    {
        if (!check)
        {
            Debug.Log("해당 게임모드가 열려있지 않습니다.");
            return;
        }

        uiManager.OpenGamePlayUI(gamePlayType);

        switch (gamePlayType)
        {
            case GamePlayType.GameChoice1:

                nowIndex = 0;
                setIndex = 1;
                countIndex = 0;

                for (int i = 0; i < normalContentList.Count; i++)
                {
                    normalContentList[i].Initialize(gamePlayType);
                }

                CreateUnDuplicateRandom();

                break;
            case GamePlayType.GameChoice2:

                for (int i = 0; i < moleCatchContentList.Count; i++)
                {
                    moleCatchContentList[i].Initialize(gamePlayType);
                }

                CreateMoleRandom();

                break;
            case GamePlayType.GameChoice3:

                for (int i = 0; i < filpCardList.Count; i++)
                {
                    filpCardList[i].Initialize(gamePlayType);
                }

                filpCardIndex = -1;
                CreateFilpCardRandom();

                break;
            case GamePlayType.GameChoice4:

                nowIndex = 0;
                buttonActionLevelIndex = 1;
                buttonActionIndex = 0;

                for (int i = 0; i < buttonActionDownList.Count; i++)
                {
                    buttonActionDownList[i].Initialize(gamePlayType);
                    buttonActionDownList[i].ButtonActionReset(i);
                    buttonActionDownList[i].gameObject.SetActive(true);
                }

                CreateButtonActionRandom();

                break;
            case GamePlayType.GameChoice5:
                break;
            case GamePlayType.GameChoice6:
                break;
            case GamePlayType.GameChoice7:
                break;
            case GamePlayType.GameChoice8:
                break;
        }

        eGameStart();
    }

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

            PlusScore(20);
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

        if(filpCardIndex == -1)
        {
            filpCardIndex = index;

            action?.Invoke(0);

            saveAction = action;
        }
        else
        {
            if(filpCardIndex == index)
            {
                Debug.Log("Success");
                action?.Invoke(1);
                saveAction?.Invoke(1);

                PlusScore(10);

                filpCardIndex = -1;
                nowIndex++;

                if(nowIndex >= filpCardList.Count / 2)
                {
                    Debug.Log("Card Reset");

                    filpCardIndex = -1;
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

                filpCardIndex = -1;

                MinusScore(5);
            }
        }
    }

    public void CheckButtonAction(int index, System.Action<bool> action)
    {
        if (buttonActionIndex.Equals(index))
        {
            Debug.Log("Success");
            action?.Invoke(true);

            PlusScore(10);

            if (numberList.Count == 0)
            {
                if(buttonActionLevelIndex < 6)
                {
                    buttonActionLevelIndex += 1;
                }

                CreateButtonActionRandom();
            }
            else
            {
                buttonActionUpList[buttonActionNumber].OnCheck();
                buttonActionNumber++;

                buttonActionIndex = numberList.Dequeue();
            }
        }
        else
        {
            Debug.Log("Failure");
            action?.Invoke(false);

            MinusScore(5);
        }

    }

    #endregion

    #region Event
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
            case GamePlayType.GameChoice4:
                break;
            case GamePlayType.GameChoice5:
                break;
            case GamePlayType.GameChoice6:
                break;
            case GamePlayType.GameChoice7:
                break;
            case GamePlayType.GameChoice8:
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

        moleCatchContentList[0].SetMole();

        yield return waitForMoleCatchSeconds;

        moleIndex = -1;

        moleCatchContentList[0].MoleReset();

        yield return waitForMoleNextSeconds;

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
