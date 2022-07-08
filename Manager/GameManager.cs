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

    public GameObject eventWatchAdView;

    [Title("Prefab")]
    public NormalContent normalContent;
    public ButtonActionContent buttonActionContent;


    [Title("GridTransform")]
    public Transform normalTransform;
    public Transform moleCatchTransform;
    public Transform filpCardTransform;
    public Transform buttonActionUpTransform;
    public Transform buttonActionDownTransform;
    public Transform dragActionTransform;


    [Title("GameStartButton")]
    public LocalizationContent gameModeText;
    public Image gameModeBackground;
    public GameObject tryCountView;
    public Text tryCountText;
    Sprite[] modeBackgroundImgArray;


    [Title("TimingAction")]
    public NormalContent timingActionContent;
    public Image timingActionFillmount;
    public GameObject timingActionCheckRange;


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

    private int dragActionIndex = 0;

    [Title("bool")]
    private bool isActive = false;

    [Title("List")]
    private Queue<int> numberList = new Queue<int>();
    private List<NormalContent> normalContentList = new List<NormalContent>();
    private List<NormalContent> moleCatchContentList = new List<NormalContent>();
    private List<NormalContent> filpCardList = new List<NormalContent>();
    private List<ButtonActionContent> buttonActionUpList = new List<ButtonActionContent>();
    private List<NormalContent> buttonActionDownList = new List<NormalContent>();
    private List<NormalContent> drageActionList = new List<NormalContent>();

    [Title("Manager")]
    public UIManager uiManager;
    public SoundManager soundManager;
    public WarningController warningController;
    public TouchManager touchManager;

    ImageDataBase imageDataBase;


    public delegate void GameEvent();
    public static event GameEvent eGameStart, eGamePause, eGameEnd;

    public delegate void ScoreEvent(int number);
    public static event ScoreEvent PlusScore, MinusScore;

    private void Awake()
    {
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;
        modeBackgroundImgArray = imageDataBase.GetModeBackgroundArray();

        eventWatchAdView.SetActive(false);

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
            content.transform.parent = dragActionTransform;
            content.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 250);
            content.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 250);
            content.transform.localPosition = Vector3.zero;
            content.transform.localScale = Vector3.one;
            content.gameObject.SetActive(false);
            drageActionList.Add(content);
        }
    }

    private void Start()
    {
        gameModeText.name = GameStateManager.instance.GamePlayType.ToString();
        gameModeText.ReLoad();

        ChoiceGameType(GameStateManager.instance.GamePlayType, GameStateManager.instance.GameModeType);
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
        dragActionIndex = Random.Range(0, 4);

        drageActionList[nowIndex].transform.localPosition = Vector3.zero;
        drageActionList[nowIndex].FingerSnapReset(dragActionIndex);
        drageActionList[nowIndex].gameObject.SetActive(true);
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
    }

    public void ChoiceGameType(GamePlayType type, GameModeType mode) //모드 선택 창에서 옵션 선택
    {
        gamePlayType = type;

        gameModeBackground.sprite = modeBackgroundImgArray[(int)mode];

        GameStateManager.instance.GamePlayType = gamePlayType;
        GameStateManager.instance.GameModeType = mode;

        gameModeText.name = gamePlayType.ToString();
        gameModeText.ReLoad();

        uiManager.CloseMenu();

        tryCountView.SetActive(false);

        if (mode == GameModeType.Perfect)
        {
            tryCountView.SetActive(true);
            tryCountText.text = GameStateManager.instance.TryCount.ToString();
        }
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

        if(GameStateManager.instance.GameModeType == GameModeType.Perfect)
        {
            if(GameStateManager.instance.TryCount <= 0)
            {
                if (!GameStateManager.instance.EventWatchAd)
                {
                    eventWatchAdView.SetActive(true);
                    return;
                }
                else
                {
                    NotionManager.instance.UseNotion(NotionType.FailEventTry);
                    return;
                }
            }
        }

        GameStateManager.instance.Fail = false;

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

                for (int i = 0; i < drageActionList.Count; i++)
                {
                    drageActionList[i].gameObject.SetActive(false);
                }

                nowIndex = 0;

                RandomFingerSnap();

                break;
        }

        eGameStart();
    }

    public void CheckNumber(int index, System.Action<bool> action)
    {
        if(nowIndex + 1 == index)
        {
            soundManager.PlaySFX(GameSfxType.Click);

            action(true);

            PlusScore(10);
            nowIndex++;

            if(nowIndex >= countIndex - 1)
            {
                CreateUnDuplicateRandom();
            }
        }
        else
        {
            if(GameStateManager.instance.Shield)
            {
                GameStateManager.instance.Shield = false;
                soundManager.PlaySFX(GameSfxType.Shield);
                uiManager.UsedItem(ItemType.Shield);
            }
            else
            {
                soundManager.PlaySFX(GameSfxType.Fail);

                warningController.Hit();

                action(false);

                MinusScore(20);

                if (!GameStateManager.instance.Fail) GameStateManager.instance.Fail = true;
            }
        }
    }

    bool mole = false;

    public void CheckMole(int index, System.Action<bool> action)
    {
        if(index == moleIndex)
        {
            soundManager.PlaySFX(GameSfxType.Click);

            action(true);

            PlusScore(30);

            mole = true;
        }
        else
        {
            if (GameStateManager.instance.Shield)
            {
                GameStateManager.instance.Shield = false;
                soundManager.PlaySFX(GameSfxType.Shield);
                uiManager.UsedItem(ItemType.Shield);
            }
            else
            {
                soundManager.PlaySFX(GameSfxType.Fail);

                warningController.Hit();

                action(false);
                countIndex = 0;

                MinusScore(20);

                if (!GameStateManager.instance.Fail) GameStateManager.instance.Fail = true;
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
                soundManager.PlaySFX(GameSfxType.Click);

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

                    StartCoroutine("FilpCardCoroution");
                }
            }
            else
            {
                if (GameStateManager.instance.Shield)
                {
                    GameStateManager.instance.Shield = false;
                    soundManager.PlaySFX(GameSfxType.Shield);
                    uiManager.UsedItem(ItemType.Shield);
                }
                else
                {
                    soundManager.PlaySFX(GameSfxType.Fail);

                    warningController.Hit();

                    action?.Invoke(2);
                    saveAction?.Invoke(2);

                    filpCardIndex = -1;

                    MinusScore(10);

                    if(!GameStateManager.instance.Fail) GameStateManager.instance.Fail = true;
                }
            }
        }
    }

    public void CheckButtonAction(int index, System.Action<bool> action)
    {
        if (buttonActionIndex.Equals(index))
        {
            soundManager.PlaySFX(GameSfxType.Click);

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
                GameStateManager.instance.Shield = false;
                soundManager.PlaySFX(GameSfxType.Shield);
                uiManager.UsedItem(ItemType.Shield);
            }
            else
            {
                soundManager.PlaySFX(GameSfxType.Fail);

                warningController.Hit();

                action?.Invoke(false);

                MinusScore(20);

                if (!GameStateManager.instance.Fail) GameStateManager.instance.Fail = true;
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
            soundManager.PlaySFX(GameSfxType.Click);

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

                soundManager.PlaySFX(GameSfxType.Fail);

                warningController.Hit();

                MinusScore(20);

                timingActionSpeed = timingActionSaveSpeed;
            }
        }
    }

    void CheckFingerSnap(bool check)
    {
        if(check)
        {
            soundManager.PlaySFX(GameSfxType.Click);

            PlusScore(10);

            drageActionList[nowIndex].MoveFingerSnap(dragActionIndex);

            nowIndex++;

            if (nowIndex > drageActionList.Count - 1)
            {
                nowIndex = 0;
            }

            RandomFingerSnap();
        }
        else
        {
            if (GameStateManager.instance.Shield)
            {
                GameStateManager.instance.Shield = false;
                soundManager.PlaySFX(GameSfxType.Shield);
                uiManager.UsedItem(ItemType.Shield);
            }
            else
            {
                soundManager.PlaySFX(GameSfxType.Fail);

                warningController.Hit();

                MinusScore(20);
            }
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
        }

        if (GameStateManager.instance.Slow) Time.timeScale = 0.9f;
    }

    public void OnGamePause()
    {
        eGamePause();
    }

    public void OnGameEnd()
    {
        eGameEnd();

        Time.timeScale = 1;

        GameStateManager.instance.Clock = false;
        GameStateManager.instance.Shield = false;
        GameStateManager.instance.Combo = false;
        GameStateManager.instance.Exp = false;
        GameStateManager.instance.Slow = false;

        tryCountText.text = GameStateManager.instance.TryCount.ToString();

        StopAllCoroutines();
    }

    public void SuccessWatchAd()
    {
        NotionManager.instance.UseNotion(NotionType.AddTryCount);

        GameStateManager.instance.EventWatchAd = true;
        GameStateManager.instance.TryCount += 1;
        tryCountText.text = GameStateManager.instance.TryCount.ToString();

        eventWatchAdView.SetActive(false);
    }

    public void CloseEventWatchAd()
    {
        eventWatchAdView.SetActive(false);
    }

    #endregion

    #region Corution
    IEnumerator MoleCatchCoroution()
    {
        ShuffleList(moleCatchContentList);

        if(countIndex <= 5)
        {
            Debug.Log("두더지 잡기 속도 증가 : " + countIndex);
            waitForMoleCatchSeconds = new WaitForSeconds(ValueManager.instance.GetMoleCatchTime() - (0.05f * countIndex));
            waitForMoleNextSeconds = new WaitForSeconds(ValueManager.instance.GetMoleNextTime() - (0.1f * countIndex));
        }

        mole = false;

        moleIndex = 0;

        moleCatchContentList[0].SetMole();

        yield return waitForMoleCatchSeconds;

        if(!mole)
        {
            countIndex = 0;
            MinusScore(0);
        }

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
                switch (dragActionIndex)
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
