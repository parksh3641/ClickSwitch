using Firebase.Analytics;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GamePlayType gamePlayType;
    public GameModeType gameModeType;

    [Title("Mode")]
    public ModeContent modeContent;


    [Title("Prefab")]
    public NormalContent normalContent;
    public ButtonActionContent buttonActionContent;


    [Title("GridTransform")]
    public Transform normalTransform;
    public Transform moleCatchTransform;
    public Transform filpCardTransform;
    public Transform buttonActionUpTransform;
    public Transform buttonActionDownTransform;
    public Transform fingerSnapTransform;


    [Title("UI")]
    public LocalizationContent gameModeText;

    [Title("TimingAction")]
    public NormalContent timingActionContent;
    public Image timingActionFillmount;
    public GameObject timingActionCheckRange;

    [Title("FingerSnap")]

    WaitForSeconds waitForSeconds = new WaitForSeconds(1);
    WaitForSeconds waitForHalfSeconds = new WaitForSeconds(0.5f);
    WaitForSeconds waitForSecSeconds = new WaitForSeconds(0.01f);
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

    private float timingActionValue = 0;
    private float timingActionPlus = 0;
    private float timingActionSpeed = 0;
    private float timingActionSaveSpeed = 0;

    private float timingActionCheckRange_1 = 0;
    private float timingActionCheckRange_2 = 0;

    private float timingActionRangePosX = 0;
    private bool timingActionMove = false;

    private int fingerSnapIndex = 0;

    [Title("bool")]
    private bool isActive = false;

    [Title("List")]
    private Queue<int> numberList = new Queue<int>();
    private List<NormalContent> normalContentList = new List<NormalContent>();
    private List<NormalContent> moleCatchContentList = new List<NormalContent>();
    private List<NormalContent> filpCardList = new List<NormalContent>();
    private List<ButtonActionContent> buttonActionUpList = new List<ButtonActionContent>();
    private List<NormalContent> buttonActionDownList = new List<NormalContent>();
    private List<NormalContent> fingerSnapList = new List<NormalContent>();

    [Title("Manager")]
    public UIManager uiManager;
    public SoundManager soundManager;
    public WarningController warningController;
    public TouchManager touchManager;


    public delegate void GameEvent();
    public static event GameEvent eGameStart, eGamePause, eGameEnd;

    public delegate void ScoreEvent(int number);
    public static event ScoreEvent PlusScore, MinusScore;

    private void Awake()
    {
        numberList.Clear();

        Time.timeScale = 1;

        for (int i = 0; i < 9; i ++)
        {
            NormalContent content = Instantiate(normalContent);
            content.transform.parent = normalTransform;
            content.transform.localPosition = Vector3.zero;
            content.transform.localScale = Vector3.one;
            content.gameObject.SetActive(false);
            normalContentList.Add(content);
        }

        for (int i = 0; i < 9; i++)
        {
            NormalContent content = Instantiate(normalContent);
            content.transform.parent = moleCatchTransform;
            content.transform.localPosition = Vector3.zero;
            content.transform.localScale = Vector3.one;
            content.gameObject.SetActive(false);
            moleCatchContentList.Add(content);
        }

        for (int i = 0; i < 16; i++)
        {
            NormalContent content = Instantiate(normalContent);
            content.transform.parent = filpCardTransform;
            content.transform.localPosition = Vector3.zero;
            content.transform.localScale = Vector3.one;
            content.gameObject.SetActive(false);
            filpCardList.Add(content);
        }

        for (int i = 0; i < 6; i++)
        {
            ButtonActionContent content = Instantiate(buttonActionContent);
            content.transform.parent = buttonActionUpTransform;
            content.transform.localPosition = Vector3.zero;
            content.transform.localScale = Vector3.one;
            content.gameObject.SetActive(false);
            buttonActionUpList.Add(content);
        }

        for (int i = 0; i < 6; i++)
        {
            NormalContent content = Instantiate(normalContent);
            content.transform.parent = buttonActionDownTransform;
            content.transform.localPosition = Vector3.zero;
            content.transform.localScale = Vector3.one;
            content.gameObject.SetActive(false);
            buttonActionDownList.Add(content);
        }

        for (int i = 0; i < 10; i++)
        {
            NormalContent content = Instantiate(normalContent);
            content.transform.parent = fingerSnapTransform;
            content.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 250);
            content.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 250);
            content.transform.localPosition = Vector3.zero;
            content.transform.localScale = Vector3.one;
            content.gameObject.SetActive(false);
            fingerSnapList.Add(content);
        }
    }

    private void Start()
    {
        gameModeText.name = GameStateManager.instance.GamePlayType.ToString();
        gameModeText.ReLoad();

        ChoiceGameType((int)GameStateManager.instance.GamePlayType);
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

        waitForMoleCatchSeconds = new WaitForSeconds(ValueManager.instance.GetMoleCatchTime());
        waitForMoleNextSeconds = new WaitForSeconds(ValueManager.instance.GetMoleNextTime());
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

    private void RandomFingerSnap()
    {
        fingerSnapIndex = Random.Range(0, 4);

        Debug.Log("방향 : " + fingerSnapIndex);

        fingerSnapList[nowIndex].transform.localPosition = Vector3.zero;
        fingerSnapList[nowIndex].FingerSnapReset(fingerSnapIndex);
        fingerSnapList[nowIndex].gameObject.SetActive(true);
    }

    #endregion

    #region Button
    public void OnGameStartButton() //게임 시작 버튼
    {
        if(!NetworkConnect.instance.CheckConnectInternet())
        {
            NotionManager.instance.UseNotion(NotionType.NetworkConnectNotion);
            return;
        }

        if (PlayfabManager.instance.isActive) PlayfabManager.instance.GetTitleInternalData(gamePlayType.ToString(), InitializeGame);
    }

    public void OpenGameMenuButton() //게임 시작전 모드 선택 창 열기
    {
        uiManager.OpenMenu();

        if (PlayfabManager.instance.isActive) PlayfabManager.instance.GetServerTime(SetModeContent);
    }

    private void SetModeContent(System.DateTime time)
    {
        modeContent.SetNextEventTime(time);
        modeContent.Initialize(GameModeType.Perfect, GamePlayType.GameChoice1);
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

        gameModeText.name = gamePlayType.ToString();
        gameModeText.ReLoad();

        uiManager.CloseMenu();
    }

    #endregion

    #region Game
    public void InitializeGame(bool check)
    {
        if (!check)
        {
            Debug.Log("해당 게임모드가 열려있지 않습니다.");
            NotionManager.instance.UseNotion(NotionType.GameModeRockNotion);
            return;
        }

        uiManager.OpenGamePlayUI(gamePlayType);

        FirebaseAnalytics.LogEvent(gamePlayType.ToString());

        if (PlayfabManager.instance.isActive) PlayfabManager.instance.CheckConsumeItem();

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

                countIndex = 0;

                for (int i = 0; i < moleCatchContentList.Count; i++)
                {
                    moleCatchContentList[i].Initialize(gamePlayType);
                }

                CreateMoleRandom();

                break;
            case GamePlayType.GameChoice3:

                nowIndex = 0;
                filpCardIndex = -1;

                for (int i = 0; i < filpCardList.Count; i++)
                {
                    filpCardList[i].Initialize(gamePlayType);
                }

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
                timingActionContent.Initialize(gamePlayType);

                timingActionPlus = 5;
                timingActionSpeed = 0.1f;
                timingActionSaveSpeed = timingActionSpeed;

                break;
            case GamePlayType.GameChoice6:

                nowIndex = 0;

                RandomFingerSnap();

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
            if(GameStateManager.instance.Shield)
            {
                Debug.Log("Defense Shield");

                GameStateManager.instance.Shield = false;
                soundManager.PlaySFX(GameSfxType.Shield);
                uiManager.UsedItem(ItemType.Shield);
            }
            else
            {
                Debug.Log("Failure");

                warningController.Hit();

                action(false);

                MinusScore(5);
            }
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
            if (GameStateManager.instance.Shield)
            {
                Debug.Log("Defense Shield");

                GameStateManager.instance.Shield = false;
                soundManager.PlaySFX(GameSfxType.Shield);
                uiManager.UsedItem(ItemType.Shield);
            }
            else
            {
                Debug.Log("Failure");

                warningController.Hit();

                action(false);
                countIndex = 0;

                MinusScore(5);
            }
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

                PlusScore(20);

                filpCardIndex = -1;
                nowIndex++;

                if(nowIndex >= filpCardList.Count / 2)
                {
                    filpCardIndex = -1;
                    nowIndex = 0;

                    Debug.Log("Reset");
                    soundManager.PlaySFX(GameSfxType.Success);
                    CreateFilpCardRandom();

                    StartCoroutine("FilpCardCorution");
                }
            }
            else
            {
                if (GameStateManager.instance.Shield)
                {
                    Debug.Log("Defense Shield");

                    GameStateManager.instance.Shield = false;
                    soundManager.PlaySFX(GameSfxType.Shield);
                    uiManager.UsedItem(ItemType.Shield);
                }
                else
                {
                    Debug.Log("Failure");

                    warningController.Hit();

                    action?.Invoke(2);
                    saveAction?.Invoke(2);

                    filpCardIndex = -1;

                    MinusScore(5);
                }
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

                Debug.Log("Reset");
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
            if (GameStateManager.instance.Shield)
            {
                Debug.Log("Defense Shield");

                GameStateManager.instance.Shield = false;
                soundManager.PlaySFX(GameSfxType.Shield);
                uiManager.UsedItem(ItemType.Shield);
            }
            else
            {
                Debug.Log("Failure");

                warningController.Hit();

                action?.Invoke(false);

                MinusScore(5);
            }
        }
    }


    public void CheckTimingAction()
    {
        if(timingActionValue < 100)
        {
            timingActionValue += timingActionPlus;
        }
        else
        {
            timingActionValue = 100f;
        }
    }

    void CheckTimingActionRange()
    {
        if(timingActionFillmount.fillAmount >= timingActionCheckRange_1 && timingActionFillmount.fillAmount <= timingActionCheckRange_2)
        {
            if (timingActionFillmount.fillAmount >= timingActionCheckRange_1 + 0.1f && timingActionFillmount.fillAmount <= timingActionCheckRange_2 - 0.05f)
            {
                Debug.Log("Perfect Success");

                PlusScore(10);
            }
            else
            {
                Debug.Log("Success");

                PlusScore(5);
            }

            if (timingActionSpeed < timingActionSaveSpeed * 4)
            {
                timingActionSpeed += timingActionSaveSpeed * 0.05f;
            }
        }
        else
        {
            if (GameStateManager.instance.Shield)
            {
                Debug.Log("Defense Shield");

                GameStateManager.instance.Shield = false;
                soundManager.PlaySFX(GameSfxType.Shield);
                uiManager.UsedItem(ItemType.Shield);
            }
            else
            {
                Debug.Log("Failure");

                warningController.Hit();

                MinusScore(5);

                timingActionSpeed = timingActionSaveSpeed;
            }
        }
    }

    void CheckFingerSnap(bool check)
    {
        if(check)
        {
            Debug.Log("Success");

            PlusScore(10);

            fingerSnapList[nowIndex].MoveFingerSnap(fingerSnapIndex);

            nowIndex++;

            if (nowIndex > fingerSnapList.Count - 1)
            {
                nowIndex = 0;
            }

            RandomFingerSnap();
        }
        else
        {
            if (GameStateManager.instance.Shield)
            {
                Debug.Log("Defense Shield");

                GameStateManager.instance.Shield = false;
                soundManager.PlaySFX(GameSfxType.Shield);
                uiManager.UsedItem(ItemType.Shield);
            }
            else
            {
                Debug.Log("Failure");

                warningController.Hit();

                MinusScore(10);
            }
        }
    }

    public void Failure()
    {
        if (GameStateManager.instance.Shield)
        {
            GameStateManager.instance.Shield = false;
            soundManager.PlaySFX(GameSfxType.Shield);
            uiManager.UsedItem(ItemType.Shield);
        }
        else
        {
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
                StartCoroutine("MoleCatchCoroution");
                break;
            case GamePlayType.GameChoice3:
                StartCoroutine("FilpCardCoroution");
                break;
            case GamePlayType.GameChoice4:
                break;
            case GamePlayType.GameChoice5:
                timingActionValue = 50;
                timingActionFillmount.fillAmount = 0.5f;

                StartCoroutine("TimingActionCoroution");
                StartCoroutine("MoveTimingActionRange");
                break;
            case GamePlayType.GameChoice6:
                StartCoroutine("CheckFingerSnapDirection");
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
        GameStateManager.instance.Clock = false;
        GameStateManager.instance.Shield = false;

        StopAllCoroutines();
    }

    #endregion

    #region Corution
    IEnumerator MoleCatchCoroution()
    {
        ShuffleList(moleCatchContentList);

        if(countIndex <= 10)
        {
            Debug.Log("두더지 잡기 속도 증가 : " + countIndex);
            waitForMoleCatchSeconds = new WaitForSeconds(ValueManager.instance.GetMoleCatchTime() - (0.025f * countIndex));
            waitForMoleNextSeconds = new WaitForSeconds(ValueManager.instance.GetMoleNextTime() - (0.05f * countIndex));
        }

        moleIndex = 0;

        moleCatchContentList[0].SetMole();

        yield return waitForMoleCatchSeconds;

        moleIndex = -1;

        moleCatchContentList[0].MoleReset();

        yield return waitForMoleNextSeconds;

        countIndex += 1;

        StartCoroutine(MoleCatchCoroution());
    }

    IEnumerator FilpCardCoroution()
    {
        isActive = false;
        eGamePause();

        for (int i = 0; i < filpCardList.Count; i ++ )
        {
            filpCardList[i].filpCardImg.enabled = true;
        }

        uiManager.WaitNotionUI(gamePlayType);

        yield return new WaitForSeconds(ValueManager.instance.GetFilpCardRememberTime());

        for (int i = 0; i < filpCardList.Count; i++)
        {
            filpCardList[i].filpCardImg.enabled = false;
        }

        isActive = true;
        eGamePause();
    }

    IEnumerator TimingActionCoroution()
    {
        float wait = 0;
        while(true)
        {
            wait += 0.01f;

            if(!timingActionMove)
            {
                timingActionRangePosX += timingActionSpeed * 3;
            }
            else
            {
                timingActionRangePosX -= timingActionSpeed * 3;
            }

            if (timingActionValue > 0)
            {
                timingActionValue -= timingActionSpeed;
            }

            timingActionFillmount.fillAmount = timingActionValue / 100f;

            timingActionCheckRange.transform.localPosition = new Vector3(timingActionRangePosX, 0, 0);

            timingActionCheckRange_1 = (timingActionCheckRange.transform.localPosition.x + 400) / 1000 ;
            timingActionCheckRange_2 = timingActionCheckRange_1 + 0.2f;

            if (wait > 0.3f)
            {
                wait = 0;
                CheckTimingActionRange();
            }

            yield return waitForSecSeconds;
        }
    }

    IEnumerator MoveTimingActionRange()
    {
        while (true)
        {
            if(Random.Range(0 , 2) == 0)
            {
                if(timingActionRangePosX < 360)
                {
                    timingActionMove = false;
                }
                else
                {
                    timingActionMove = true;
                }
            }
            else
            {
                if (timingActionRangePosX > -360)
                {
                    timingActionMove = true;
                }
                else
                {
                    timingActionMove = false;
                }
            }

            yield return new WaitForSeconds(2);
        }
    }

    IEnumerator CheckFingerSnapDirection()
    {
        while(true)
        {
            if (touchManager.direction != "")
            {
                switch (fingerSnapIndex)
                {
                    case 0:
                        if (touchManager.direction == "Left")
                        {
                            touchManager.direction = "";
                            CheckFingerSnap(true);
                        }
                        else
                        {
                            touchManager.direction = "";
                            CheckFingerSnap(false);
                        }
                        break;
                    case 1:
                        if (touchManager.direction == "Right")
                        {
                            touchManager.direction = "";
                            CheckFingerSnap(true);
                        }
                        else
                        {
                            touchManager.direction = "";
                            CheckFingerSnap(false);
                        }
                        break;
                    case 2:
                        if (touchManager.direction == "Down")
                        {
                            touchManager.direction = "";
                            CheckFingerSnap(true);
                        }
                        else
                        {
                            touchManager.direction = "";
                            CheckFingerSnap(false);
                        }
                        break;
                    case 3:
                        if (touchManager.direction == "Up")
                        {
                            touchManager.direction = "";
                            CheckFingerSnap(true);
                        }
                        else
                        {
                            touchManager.direction = "";
                            CheckFingerSnap(false);
                        }
                        break;
                }
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    #endregion
}
