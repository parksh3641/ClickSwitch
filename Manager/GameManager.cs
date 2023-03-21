using Firebase.Analytics;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GamePlayType gamePlayType;
    public GamePlayType saveGamePlayType;
    public GameModeType gameModeType;
    GameModeLevel level;

    [Space]
    public GameObject eventWatchAdView;

    [Title("ChangeGameMode")]
    public GameObject changeGameModeView;
    public GameObject hardGameModeLockObj;

    [Space]
    [Title("Prefab")]
    public NormalContent normalContent;
    public ButtonActionContent buttonActionContent;
    public LeftRightActionContent leftRightActionContent;

    [Space]
    [Title("GridTransform")]
    public Transform speedTouchEasyTransform;
    public Transform speedTouchNormalTransform;
    public Transform speedTouchHardTransform;
    [Space]
    public Transform moleCatchEasyTransform;
    public Transform moleCatchHardTransform;
    [Space]
    public Transform filpCardEasyTransform;
    public Transform filpCardNormalTransform;
    public Transform filpCardHardTransform;
    [Space]
    public Transform buttonActionUpTransform;

    public Transform buttonActionDownEasyTransform;
    public Transform buttonActionDownNormalTransform;
    public Transform buttonActionDownHardTransform;
    [Space]
    public Transform dragActionTransform;
    [Space]
    public Transform leftRightActionUpTransform;

    [Space]
    [Title("GameStartButton")]
    public Image icon;
    public Text playText;
    public LocalizationContent gameModeText;

    public Image gameModeBackground;

    public GameObject tryCountView;
    public Text tryCountText;
    public GameObject tryCountZeroView;
    public GameObject[] tryCountButton;

    public LocalizationContent levelText;

    Sprite[] iconArray;
    Sprite[] modeBackgroundImgArray;

    [Space]
    [Title("TimingAction")]
    public NormalContent timingActionButton1;
    public NormalContent timingActionButton2;

    public Image timingActionFillmount;
    public GameObject timingActionCheckRange;
    public GameObject[] timingActionVector;


    [Space]
    [Title("LeftRightAction")]
    public NormalContent leftRightActionButton1;
    public NormalContent leftRightActionButton2;


    [Space]
    [Title("CoinRushAction")]
    public GameObject coinRushLockObject;
    public GameObject plusCoinObject;
    public Text questionText;
    public InputField answerInputText;
    public Text plusCoinText;


    WaitForSeconds waitForSeconds = new WaitForSeconds(1);
    WaitForSeconds waitForHalfSeconds = new WaitForSeconds(0.5f);
    WaitForSeconds waitForSecSeconds = new WaitForSeconds(0.01f);
    WaitForSeconds waitForMoleCatchSeconds;
    WaitForSeconds waitForMoleNextSeconds;
    WaitForSeconds waitForFilpCardSeconds;


    [Space]
    [Title("Value")]
    private int nowIndex = 0;
    private int setIndex = 1;
    private int countIndex = 0;

    private bool moleClone = false;

    private int filpCardIndex = 0;

    private int buttonActionNumber = 0;
    private int buttonActionLevelIndex = 0;
    private int buttonActionIndex = 0;

    [Header("TimingAction")]
    private float timingActionValue = 0;
    private float timingActionPlus = 0;
    private float timingActionSpeed = 0;
    private float timingActionSaveSpeed = 0;
    private float timingActionRange = 0;

    private float timingActionCheckRange_1 = 0;
    private float timingActionCheckRange_2 = 0;

    private float timingActionRangePosX = 0;
    private bool timingActionMove = false;

    [Header("DragAction")]
    private int dragActionIndex = 0;

    [Header("LeftRightAction")]
    private int leftRightActionIndex = 0;
    private int leftRightActionNumber = 0;

    [Header("CoinRush")]
    private int coinRushMax = 0;
    private int coinRushNumber = 0;
    private int coinRushPasscode = 0;

    private float critical = 0;

    [Title("bool")]
    private bool isActive = false;

    [Title("List")]
    private Queue<int> numberList = new Queue<int>();

    private List<NormalContent> speedTouchEasyList = new List<NormalContent>();
    private List<NormalContent> speedTouchNormalList = new List<NormalContent>();
    private List<NormalContent> speedTouchHardList = new List<NormalContent>();

    private List<NormalContent> moleCatchEasyList = new List<NormalContent>();
    private List<NormalContent> moleCatchHardList = new List<NormalContent>();

    private List<NormalContent> filpCardEasyList = new List<NormalContent>();
    private List<NormalContent> filpCardNormalList = new List<NormalContent>();
    private List<NormalContent> filpCardHardList = new List<NormalContent>();


    private List<ButtonActionContent> buttonActionUpList = new List<ButtonActionContent>();
    private List<NormalContent> buttonActionDownEasyList = new List<NormalContent>();
    private List<NormalContent> buttonActionDownNormalList = new List<NormalContent>();

    private List<NormalContent> dragActionList = new List<NormalContent>();

    private List<LeftRightActionContent> leftRightActionUpList = new List<LeftRightActionContent>();


    Dictionary<string, string> dicData = new Dictionary<string, string>();

    private List<NormalContent> targetContentList = new List<NormalContent>();

    [Title("Manager")]
    public UIManager uiManager;
    public SoundManager soundManager;
    public TouchManager touchManager;
    public PlayerDataBase playerDataBase;
    public UpgradeDataBase upgradeDataBase;

    ImageDataBase imageDataBase;


    public delegate void GameEvent();
    public static event GameEvent eGameStart, eGamePause, eGameEnd;

    public delegate void ScoreEvent(int number);
    public static event ScoreEvent PlusScore, MinusScore, PlusCoin;

    private void Awake()
    {
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;
        modeBackgroundImgArray = imageDataBase.GetModeBackgroundArray();
        iconArray = imageDataBase.GetIconArray();

        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;
        if (upgradeDataBase == null) upgradeDataBase = Resources.Load("UpgradeDataBase") as UpgradeDataBase;


        eventWatchAdView.SetActive(false);
        changeGameModeView.SetActive(false);
        hardGameModeLockObj.SetActive(true);

        numberList.Clear();

        Screen.sleepTimeout = SleepTimeout.SystemSetting;
        Time.timeScale = 1;

        for (int i = 0; i < 9; i ++)
        {
            NormalContent content = Instantiate(normalContent);
            content.transform.parent = speedTouchEasyTransform;
            content.transform.localPosition = Vector3.zero;
            content.transform.localScale = Vector3.one;
            content.gameObject.SetActive(false);
            speedTouchEasyList.Add(content);
        }

        for (int i = 0; i < 16; i++)
        {
            NormalContent content = Instantiate(normalContent);
            content.transform.parent = speedTouchNormalTransform;
            content.transform.localPosition = Vector3.zero;
            content.transform.localScale = Vector3.one;
            content.gameObject.SetActive(false);
            speedTouchNormalList.Add(content);
        }

        for (int i = 0; i < 25; i++)
        {
            NormalContent content = Instantiate(normalContent);
            content.transform.parent = speedTouchHardTransform;
            content.transform.localPosition = Vector3.zero;
            content.transform.localScale = Vector3.one;
            content.gameObject.SetActive(false);
            speedTouchHardList.Add(content);
        }

        for (int i = 0; i < 9; i++)
        {
            NormalContent content = Instantiate(normalContent);
            content.transform.parent = moleCatchEasyTransform;
            content.transform.localPosition = Vector3.zero;
            content.transform.localScale = Vector3.one;
            content.gameObject.SetActive(false);
            moleCatchEasyList.Add(content);
        }

        for (int i = 0; i < 16; i++)
        {
            NormalContent content = Instantiate(normalContent);
            content.transform.parent = moleCatchHardTransform;
            content.transform.localPosition = Vector3.zero;
            content.transform.localScale = Vector3.one;
            content.gameObject.SetActive(false);
            moleCatchHardList.Add(content);
        }

        for (int i = 0; i < 16; i++)
        {
            NormalContent content = Instantiate(normalContent);
            content.transform.parent = filpCardEasyTransform;
            content.transform.localPosition = Vector3.zero;
            content.transform.localScale = Vector3.one;
            content.gameObject.SetActive(false);
            filpCardEasyList.Add(content);
        }

        for (int i = 0; i < 25; i++)
        {
            NormalContent content = Instantiate(normalContent);
            content.transform.parent = filpCardNormalTransform;
            content.transform.localPosition = Vector3.zero;
            content.transform.localScale = Vector3.one;
            content.gameObject.SetActive(false);
            filpCardNormalList.Add(content);
        }

        for (int i = 0; i < 36; i++)
        {
            NormalContent content = Instantiate(normalContent);
            content.transform.parent = filpCardHardTransform;
            content.transform.localPosition = Vector3.zero;
            content.transform.localScale = Vector3.one;
            content.gameObject.SetActive(false);
            filpCardHardList.Add(content);
        }

        for (int i = 0; i < 12; i++)
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
            content.transform.parent = buttonActionDownEasyTransform;
            content.transform.localPosition = Vector3.zero;
            content.transform.localScale = Vector3.one;
            content.gameObject.SetActive(false);
            buttonActionDownEasyList.Add(content);
        }

        for (int i = 0; i < 8; i++)
        {
            NormalContent content = Instantiate(normalContent);
            content.transform.parent = buttonActionDownNormalTransform;
            content.transform.localPosition = Vector3.zero;
            content.transform.localScale = Vector3.one;
            content.gameObject.SetActive(false);
            buttonActionDownNormalList.Add(content);
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
            dragActionList.Add(content);
        }
    }

    private void Start()
    {
        gameModeText.localizationName = GameStateManager.instance.GamePlayType.ToString();
        gameModeText.ReLoad();

        ChoiceGameType(GameStateManager.instance.GamePlayType, GameStateManager.instance.GameModeType);

        if (GameStateManager.instance.SleepMode)
        {
            Application.targetFrameRate = 30;
        }
        else
        {
            Application.targetFrameRate = 60;
        }

        GameStateManager.instance.Clock = false;
        GameStateManager.instance.Shield = false;
        GameStateManager.instance.Combo = false;
        GameStateManager.instance.Exp = false;
        GameStateManager.instance.Slow = false;
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
        ShuffleList(targetContentList);

        for (int i = 0; i < targetContentList.Count; i++)
        {
            targetContentList[i].NormalReset(setIndex);
            targetContentList[i].gameObject.SetActive(false);
            targetContentList[i].gameObject.SetActive(true);

            setIndex++;
        }

        targetContentList[0].SpeedTouchFirst();

        countIndex = setIndex;
    }

    private void CreateMoleRandom()
    {
        for (int i = 0; i < targetContentList.Count; i++)
        {
            targetContentList[i].MoleReset();
            targetContentList[i].gameObject.SetActive(false);
            targetContentList[i].gameObject.SetActive(true);
        }

        waitForMoleCatchSeconds = new WaitForSeconds(ValueManager.instance.GetMoleCatchTime());
        waitForMoleNextSeconds = new WaitForSeconds(ValueManager.instance.GetMoleNextTime());
    }

    private void CreateFilpCardRandom()
    {
        ShuffleList(targetContentList);

        int j = 1;
        int k = 0;

        for(int i = 0; i < targetContentList.Count; i ++)
        {
            targetContentList[i].FilpCardReset(k);
            targetContentList[i].gameObject.SetActive(false);
            targetContentList[i].gameObject.SetActive(true);

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

        numberList.Clear();

        int number = 0;
        int maxNumber = 0;

        switch (gameModeType)
        {
            case GameModeType.Easy:
                maxNumber = 6;
                break;
            case GameModeType.Normal:
                maxNumber = 8;
                break;
            case GameModeType.Hard:
                maxNumber = 8;
                break;
            case GameModeType.Perfect:
                maxNumber = 6;
                break;
        }

        for (int i = 0; i < buttonActionLevelIndex; i++)
        {
            number = Random.Range(0, maxNumber);
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

        dragActionList[nowIndex].transform.localPosition = Vector3.zero;
        dragActionList[nowIndex].FingerSnapReset(dragActionIndex);

        switch (gameModeType)
        {
            case GameModeType.Easy:
                dragActionList[nowIndex].FingerSnapBackground(dragActionIndex, false);
                break;
            case GameModeType.Normal:
                break;
            case GameModeType.Hard:
                dragActionList[nowIndex].FingerSnapBackground(dragActionIndex, true);
                break;
            case GameModeType.Perfect:
                dragActionList[nowIndex].FingerSnapBackground(dragActionIndex, false);
                break;
        }

        dragActionList[nowIndex].gameObject.SetActive(true);
    }

    private void CreateLeftRightActionRandom()
    {
        for(int i = 0; i < leftRightActionUpList.Count; i ++)
        {
            Destroy(leftRightActionUpList[i].gameObject);
        }

        leftRightActionUpList.Clear();

        for (int i = 0; i < 8; i++)
        {
            LeftRightActionContent content = Instantiate(leftRightActionContent);
            content.transform.parent = leftRightActionUpTransform;
            content.transform.localPosition = Vector3.zero;
            content.transform.localScale = Vector3.one;
            content.gameObject.SetActive(false);
            leftRightActionUpList.Add(content);
        }

        numberList.Clear();

        int number = 0;

        for (int i = leftRightActionUpList.Count - 1; i > -1; i--)
        {
            number = Random.Range(0, 2);
            numberList.Enqueue(number);
            leftRightActionUpList[i].Initialize(number);

            switch (gameModeType)
            {
                case GameModeType.Hard:
                    leftRightActionUpList[i].RandomBackground();
                    break;
            }

            leftRightActionUpList[i].gameObject.SetActive(true);
        }

        leftRightActionNumber = leftRightActionUpList.Count - 1;

        leftRightActionIndex = numberList.Dequeue();

        LeftRightFirst();
    }

    private void RandomCoinRush()
    {
        coinRushPasscode = 0;

        int first = 0;
        int second = 0;

        switch (gameModeType)
        {
            case GameModeType.Easy:
                first = Random.Range(1, 100);
                second = Random.Range(1, 100);

                break;
            case GameModeType.Normal:
                first = Random.Range(100, 1000);
                second = Random.Range(100, 1000);

                break;
            case GameModeType.Hard:
                first = Random.Range(1000, 5000);
                second = Random.Range(1000, 5000);

                break;
        }

        coinRushPasscode = first + second;
        questionText.text = first.ToString() + " + " + second.ToString() + " = ?";
    }

    #endregion

    bool noTouch = false;

    #region Button
    public void OnGameStartButton()
    {
        if (noTouch) return;

        noTouch = true;

        if(!NetworkConnect.instance.CheckConnectInternet())
        {
            noTouch = false;
            NotionManager.instance.UseNotion(NotionType.NetworkConnectNotion);
            return;
        }

        if (PlayfabManager.instance.isActive) PlayfabManager.instance.GetTitleInternalData(gamePlayType.ToString(), InitializeGame);
    }

    public void OpenGameMenuButton()
    {
        uiManager.OpenMenu();
        changeGameModeView.SetActive(false);
    }

    public void CloseGameModeView()
    {
        gamePlayType = saveGamePlayType;

        changeGameModeView.SetActive(false);
    }

    public void ChoiceGameType(GamePlayType type, GameModeType mode)
    {
        gamePlayType = type;
        gameModeType = mode;

        gameModeBackground.sprite = modeBackgroundImgArray[(int)gameModeType];
        icon.sprite = iconArray[(int)gamePlayType];

        GameStateManager.instance.GamePlayType = gamePlayType;
        GameStateManager.instance.GameModeType = gameModeType;

        gameModeText.localizationName = gamePlayType.ToString();
        gameModeText.ReLoad();

        tryCountView.SetActive(false);
        tryCountZeroView.SetActive(false);

        //levelText.name = gameModeType.ToString();
        //levelText.ReLoad();

        playText.color = new Color(0, 122 / 255f, 89 / 255f);
        gameModeText.GetComponent<Text>().color = new Color(0, 122 / 255f, 89 / 255f);

        switch (gameModeType)
        {
            case GameModeType.Easy:
                break;
            case GameModeType.Normal:
                break;
            case GameModeType.Hard:
                playText.color = new Color(67 / 255f, 83 / 255f, 108 / 255f);
                gameModeText.GetComponent<Text>().color = new Color(67 / 255f, 83 / 255f, 108 / 255f);
                break;
            case GameModeType.Perfect:
                if (GameStateManager.instance.TryCount > 0)
                {
                    tryCountView.SetActive(true);
                    tryCountText.text = GameStateManager.instance.TryCount.ToString();
                }
                else
                {
                    if (!GameStateManager.instance.EventWatchAd)
                    {
                        tryCountZeroView.SetActive(true);

                        tryCountButton[0].SetActive(true);
                        tryCountButton[1].SetActive(false);
                    }
                }
                break;
        }

        switch (gamePlayType)
        {
            case GamePlayType.GameChoice8:
                if (GameStateManager.instance.CoinRushTryCount > 0)
                {
                    tryCountView.SetActive(true);
                    tryCountText.text = GameStateManager.instance.CoinRushTryCount.ToString();
                }
                else
                {
                    if (!GameStateManager.instance.CoinRushWatchAd)
                    {
                        tryCountZeroView.SetActive(true);

                        tryCountButton[0].SetActive(false);
                        tryCountButton[1].SetActive(true);
                    }
                }

                break;
        }

        playerDataBase.ChangeGameMode(gamePlayType, gameModeType);

        level = playerDataBase.GetGameMode(gamePlayType);

        dicData.Clear();
        dicData.Add("GameMode_" + (int)level.gamePlayType, JsonUtility.ToJson(level));

        if (PlayfabManager.instance.isActive) PlayfabManager.instance.SetPlayerData(dicData);

        if (!changeMode) uiManager.CloseMenu();
    }

    public void ChangeGameMode(GamePlayType type)
    {
        saveGamePlayType = gamePlayType;
        gamePlayType = type;

        changeGameModeView.SetActive(true);

        level = playerDataBase.GetGameMode(type);

        if (level.hard) hardGameModeLockObj.SetActive(false);
    }

    bool changeMode = false;

    public void ChoiceGameType(int number)
    {
        changeMode = true;

        ChoiceGameType(gamePlayType, GameModeType.Easy + number);

        uiManager.CheckMenu();
        changeGameModeView.SetActive(false);

        changeMode = false;
    }

    #endregion

    #region Game
    public void InitializeGame(bool check)
    {
        if (!check)
        {
            Debug.Log("This Game locked to server.");
            NotionManager.instance.UseNotion(NotionType.GameModeRockNotion);
            noTouch = false;
            return;
        }

        if(GameStateManager.instance.GameModeType == GameModeType.Perfect)
        {
            if(GameStateManager.instance.TryCount <= 0)
            {
                if (!GameStateManager.instance.EventWatchAd)
                {
                    eventWatchAdView.SetActive(true);

                    tryCountButton[0].SetActive(true);
                    tryCountButton[1].SetActive(false);
                    return;
                }
                else
                {
                    noTouch = false;
                    NotionManager.instance.UseNotion(NotionType.FailEventTry);
                    return;
                }
            }
            else
            {
                GameStateManager.instance.TryCount -= 1;
            }
        }

        if (GameStateManager.instance.GamePlayType == GamePlayType.GameChoice8)
        {
            if (GameStateManager.instance.CoinRushTryCount <= 0)
            {
                if (!GameStateManager.instance.CoinRushWatchAd)
                {
                    eventWatchAdView.SetActive(true);

                    tryCountButton[0].SetActive(false);
                    tryCountButton[1].SetActive(true);
                    return;
                }
                else
                {
                    noTouch = false;
                    NotionManager.instance.UseNotion(NotionType.FailEventTry);
                    return;
                }
            }
        }

        GameStateManager.instance.Fail = false;

        critical = upgradeDataBase.GetValue(UpgradeType.Critical, playerDataBase.CriticalLevel);

        uiManager.OpenGamePlayUI(gamePlayType);

        FirebaseAnalytics.LogEvent(gamePlayType.ToString());
        FirebaseAnalytics.LogEvent(gameModeType.ToString());
        FirebaseAnalytics.LogEvent(gamePlayType + " / " + gameModeType);

        if (PlayfabManager.instance.isActive) PlayfabManager.instance.CheckConsumeItem();

        switch (gamePlayType)
        {
            case GamePlayType.GameChoice1:

                nowIndex = 0;
                setIndex = 1;
                countIndex = 0;

                speedTouchEasyTransform.gameObject.SetActive(false);
                speedTouchNormalTransform.gameObject.SetActive(false);
                speedTouchHardTransform.gameObject.SetActive(false);

                switch (gameModeType)
                {
                    case GameModeType.Easy:
                        targetContentList = speedTouchEasyList;
                        speedTouchEasyTransform.gameObject.SetActive(true);
                        break;
                    case GameModeType.Normal:
                        targetContentList = speedTouchNormalList;
                        speedTouchNormalTransform.gameObject.SetActive(true);
                        break;
                    case GameModeType.Hard:
                        targetContentList = speedTouchHardList;
                        speedTouchHardTransform.gameObject.SetActive(true);
                        break;
                    case GameModeType.Perfect:
                        targetContentList = speedTouchEasyList;
                        speedTouchEasyTransform.gameObject.SetActive(true);
                        break;
                }

                for (int i = 0; i < targetContentList.Count; i++)
                {
                    targetContentList[i].Initialize(gamePlayType);
                }

                CreateUnDuplicateRandom();

                break;
            case GamePlayType.GameChoice2:

                countIndex = 0;

                moleCatchEasyTransform.gameObject.SetActive(false);
                moleCatchHardTransform.gameObject.SetActive(false);

                switch (gameModeType)
                {
                    case GameModeType.Easy:
                        targetContentList = moleCatchEasyList;
                        moleCatchEasyTransform.gameObject.SetActive(true);
                        break;
                    case GameModeType.Normal:
                        targetContentList = moleCatchEasyList;
                        moleCatchEasyTransform.gameObject.SetActive(true);
                        break;
                    case GameModeType.Hard:
                        targetContentList = moleCatchHardList;
                        moleCatchHardTransform.gameObject.SetActive(true);
                        break;
                    case GameModeType.Perfect:
                        targetContentList = moleCatchEasyList;
                        moleCatchEasyTransform.gameObject.SetActive(true);
                        break;
                }

                for (int i = 0; i < targetContentList.Count; i++)
                {
                    targetContentList[i].Initialize(gamePlayType);
                    targetContentList[i].index = i;
                }

                CreateMoleRandom();

                break;
            case GamePlayType.GameChoice3:

                nowIndex = 0;
                filpCardIndex = -1;

                filpCardEasyTransform.gameObject.SetActive(false);
                filpCardNormalTransform.gameObject.SetActive(false);
                filpCardHardTransform.gameObject.SetActive(false);

                switch (gameModeType)
                {
                    case GameModeType.Easy:
                        targetContentList = filpCardEasyList;
                        waitForFilpCardSeconds = new WaitForSeconds(ValueManager.instance.GetFilpCardRememberTime());
                        filpCardEasyTransform.gameObject.SetActive(true);
                        break;
                    case GameModeType.Normal:
                        targetContentList = filpCardNormalList;
                        waitForFilpCardSeconds = new WaitForSeconds(ValueManager.instance.GetFilpCardRememberTime() * 1.5f);
                        filpCardNormalTransform.gameObject.SetActive(true);
                        break;
                    case GameModeType.Hard:
                        targetContentList = filpCardHardList;
                        waitForFilpCardSeconds = new WaitForSeconds(ValueManager.instance.GetFilpCardRememberTime() * 2f);
                        filpCardHardTransform.gameObject.SetActive(true);
                        break;
                    case GameModeType.Perfect:
                        targetContentList = filpCardEasyList;
                        waitForFilpCardSeconds = new WaitForSeconds(ValueManager.instance.GetFilpCardRememberTime());
                        filpCardEasyTransform.gameObject.SetActive(true);
                        break;
                }

                for (int i = 0; i < targetContentList.Count; i++)
                {
                    targetContentList[i].Initialize(gamePlayType);
                }

                CreateFilpCardRandom();

                break;
            case GamePlayType.GameChoice4:

                nowIndex = 0;
                buttonActionLevelIndex = 1;
                buttonActionIndex = 0;

                buttonActionDownEasyTransform.gameObject.SetActive(false);
                buttonActionDownNormalTransform.gameObject.SetActive(false);

                switch (gameModeType)
                {
                    case GameModeType.Easy:
                        targetContentList = buttonActionDownEasyList;
                        buttonActionDownEasyTransform.gameObject.SetActive(true);
                        break;
                    case GameModeType.Normal:
                        targetContentList = buttonActionDownNormalList;
                        buttonActionDownNormalTransform.gameObject.SetActive(true);

                        break;
                    case GameModeType.Hard:
                        targetContentList = buttonActionDownNormalList;
                        buttonActionDownNormalTransform.gameObject.SetActive(true);

                        ShuffleList(targetContentList);
                        break;
                    case GameModeType.Perfect:
                        targetContentList = buttonActionDownEasyList;
                        buttonActionDownEasyTransform.gameObject.SetActive(true);
                        break;
                }

                for (int i = 0; i < targetContentList.Count; i++)
                {
                    targetContentList[i].Initialize(gamePlayType);
                    targetContentList[i].ButtonActionReset(i);
                    targetContentList[i].gameObject.SetActive(true);
                }

                CreateButtonActionRandom();

                break;
            case GamePlayType.GameChoice5:
                timingActionButton1.Initialize(gamePlayType);

                timingActionFillmount.fillAmount = 0.5f;
                timingActionCheckRange.transform.localPosition = Vector3.zero;

                timingActionPlus = 5;

                timingActionCheckRange_1 = 0;
                timingActionCheckRange_2 = 0;

                timingActionRangePosX = 0;
                timingActionMove = false;

                timingActionVector[0].SetActive(false);
                timingActionVector[1].SetActive(false);

                timingActionButton2.gameObject.SetActive(false);

                timingActionRange = 300;

                switch (gameModeType)
                {
                    case GameModeType.Easy:
                        timingActionSpeed = 0.1f;
                        break;
                    case GameModeType.Normal:
                        timingActionSpeed = 0.15f;
                        break;
                    case GameModeType.Hard:
                        timingActionSpeed = 0.3f;
                        timingActionRange = 200;
                        timingActionButton2.gameObject.SetActive(true);
                        timingActionButton2.Initialize(gamePlayType);
                        break;
                    case GameModeType.Perfect:
                        timingActionSpeed = 0.1f;
                        break;
                }

                timingActionSaveSpeed = timingActionSpeed;

                break;
            case GamePlayType.GameChoice6:

                for (int i = 0; i < dragActionList.Count; i++)
                {
                    dragActionList[i].Initialize(gamePlayType);
                    dragActionList[i].gameObject.SetActive(false);
                }

                nowIndex = 0;

                RandomFingerSnap();

                break;
            case GamePlayType.GameChoice7:

                leftRightActionIndex = 0;

                int random = Random.Range(0, 2);

                if(random == 0)
                {
                    leftRightActionButton1.LeftRightReset(0);
                    leftRightActionButton2.LeftRightReset(1);
                }
                else
                {
                    leftRightActionButton1.LeftRightReset(1);
                    leftRightActionButton2.LeftRightReset(0);
                }

                switch (gameModeType)
                {
                    case GameModeType.Normal:
                        leftRightActionButton1.LeftRightRandomBackground();
                        leftRightActionButton2.LeftRightRandomBackground();
                        break;
                    case GameModeType.Hard:
                        leftRightActionButton1.LeftRightRandomBackground();
                        leftRightActionButton2.LeftRightRandomBackground();
                        break;
                }

                CreateLeftRightActionRandom();

                break;
            case GamePlayType.GameChoice8:

                GameStateManager.instance.DontStopGame = true;

                coinRushMax = 0;
                coinRushNumber = 0;

                answerInputText.text = "";

                switch (gameModeType)
                {
                    case GameModeType.Easy:
                        coinRushMax = 200;

                        break;
                    case GameModeType.Normal:
                        coinRushMax = 300;

                        break;
                    case GameModeType.Hard:
                        coinRushMax = 500;

                        break;
                }

                questionText.text = "";
                plusCoinText.text = "0";

                coinRushLockObject.SetActive(false);
                plusCoinObject.SetActive(false);

                RandomCoinRush();

                break;
        }

        eGameStart();

        noTouch = false;
    }

    void CheckPlusScore(int number)
    {
        float random = Random.Range(0, 100f);

        if(random <= critical)
        {
            PlusScore(number * 2);
        }
        else
        {
            PlusScore(number);
        }
    }


    public void CheckNumber(int index, System.Action<bool> action)
    {
        if(nowIndex + 1 == index)
        {
            soundManager.PlaySFX(GameSfxType.Click);

            action(true);

            CheckPlusScore(10);
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
                action(false);

                MinusScore(20);

                if (!GameStateManager.instance.Fail) GameStateManager.instance.Fail = true;
            }
        }
    }

    bool hitMole = false;

    public void CheckMole(int index, System.Action<bool> action)
    {
        if(index == 0)
        {
            soundManager.PlaySFX(GameSfxType.Oha);

            action(true);

            CheckPlusScore(40);

            hitMole = true;
        }
        else if (index == 1)
        {
            if (GameStateManager.instance.Shield)
            {
                GameStateManager.instance.Shield = false;
                soundManager.PlaySFX(GameSfxType.Shield);
                uiManager.UsedItem(ItemType.Shield);
            }
            else
            {
                action(false);
                countIndex = 0;

                MinusScore(10);

                if (!GameStateManager.instance.Fail) GameStateManager.instance.Fail = true;
            }
        }
        else
        {
            action(false);
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

                CheckPlusScore(30);

                filpCardIndex = -1;
                nowIndex++;

                if(nowIndex >= targetContentList.Count / 2)
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
                    action?.Invoke(2);
                    saveAction?.Invoke(2);

                    filpCardIndex = -1;

                    MinusScore(5);

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

            CheckPlusScore(10);

            if (numberList.Count == 0)
            {
                switch (gameModeType)
                {
                    case GameModeType.Hard:
                        if (buttonActionLevelIndex < 12)
                        {
                            buttonActionLevelIndex += 1;
                        }
                        break;
                    default:
                        if (buttonActionLevelIndex < 6)
                        {
                            buttonActionLevelIndex += 1;
                        }
                        break;
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
                action?.Invoke(false);

                MinusScore(10);

                buttonActionLevelIndex = 1;
                CreateButtonActionRandom();

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

    public void CheckTimingAction2()
    {
        if (timingActionValue > 0)
        {
            timingActionValue -= timingActionPlus;
        }
        else
        {
            timingActionValue = 0;
        }
    }

    void CheckTimingActionRange()
    {
        if(timingActionFillmount.fillAmount >= timingActionCheckRange_1 && timingActionFillmount.fillAmount <= timingActionCheckRange_2)
        {
            if (GameStateManager.instance.PlayGame) soundManager.PlaySFX(GameSfxType.Click);

            if (timingActionFillmount.fillAmount >= timingActionCheckRange_1 + 0.1f && timingActionFillmount.fillAmount <= timingActionCheckRange_2 - 0.05f)
            {
                CheckPlusScore(20);
            }
            else
            {
                CheckPlusScore(5);
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

                MinusScore(5);

                timingActionSpeed = timingActionSaveSpeed;
            }
        }
    }

    public void CheckFingerSnapDirection(int direction)
    {
        if (dragActionIndex == direction)
        {
            CheckFingerSnap(true);
        }
        else
        {
            CheckFingerSnap(false);
        }
    }

    void CheckFingerSnap(bool check)
    {
        if(check)
        {
            soundManager.PlaySFX(GameSfxType.Click);

            CheckPlusScore(10);

            dragActionList[nowIndex].MoveFingerSnap(dragActionIndex);

            nowIndex++;

            if (nowIndex > dragActionList.Count - 1)
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
                MinusScore(10);
            }
        }
    }

    public void CheckLeftRightAction(int index)
    {
        if (leftRightActionIndex.Equals(index))
        {
            soundManager.PlaySFX(GameSfxType.Click);

            CheckPlusScore(5);

            if(leftRightActionNumber < 0)
            {
                leftRightActionNumber = leftRightActionUpList.Count - 1;
            }

            leftRightActionUpList[leftRightActionNumber].gameObject.SetActive(false);
            leftRightActionUpList[leftRightActionNumber].transform.SetAsFirstSibling();
            leftRightActionUpList[leftRightActionNumber].gameObject.SetActive(true);

            int number = Random.Range(0, 2);
            numberList.Enqueue(number);
            leftRightActionUpList[leftRightActionNumber].Initialize(number);

            leftRightActionNumber--;
            leftRightActionIndex = numberList.Dequeue();

            LeftRightFirst();
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
                MinusScore(5);

                if (!GameStateManager.instance.Fail) GameStateManager.instance.Fail = true;
            }
        }
    }

    public void LeftRightFirst()
    {
        if (leftRightActionNumber < 0)
        {
            leftRightActionUpList[leftRightActionUpList.Count - 1].First();
        }
        else
        {
            leftRightActionUpList[leftRightActionNumber].First();
        }
    }


    public void CheckCoinRush()
    {
        PlusScore(3);

        soundManager.PlaySFX(GameSfxType.GetMoney);

        if (coinRushNumber + 2 <= coinRushMax)
        {
            coinRushNumber += 2;
            PlusCoin(2);
        }
        else
        {
            coinRushNumber = coinRushMax;
        }

        plusCoinText.text = coinRushNumber.ToString();
    }

    public void CheckCoinRushAnswer()
    {
        if(int.Parse(answerInputText.text) == coinRushPasscode)
        {
            GameStateManager.instance.DontStopGame = false;

            coinRushLockObject.SetActive(false);
            plusCoinObject.SetActive(true);

            soundManager.PlaySFX(GameSfxType.Success);

            NotionManager.instance.UseNotion(NotionType.TouchTreasureNotion);
        }
        else
        {
            soundManager.PlaySFX(GameSfxType.Fail);

            NotionManager.instance.UseNotion(NotionType.WrongNotion);
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

                moleClone = false;

                switch (GameStateManager.instance.GameModeType)
                {
                    case GameModeType.Easy:
                        break;
                    case GameModeType.Normal:
                        moleClone = true;
                        break;
                    case GameModeType.Hard:
                        moleClone = true;
                        break;
                    case GameModeType.Perfect:
                        break;
                }

                StartCoroutine(MoleCatchCoroution());
                break;
            case GamePlayType.GameChoice3:
                StartCoroutine(FilpCardCoroution());
                break;
            case GamePlayType.GameChoice4:
                break;
            case GamePlayType.GameChoice5:
                timingActionValue = 50;
                timingActionFillmount.fillAmount = 0.5f;

                StartCoroutine(TimingActionCoroution());
                StartCoroutine(MoveTimingActionRange());
                break;
            case GamePlayType.GameChoice6:
                break;
            case GamePlayType.GameChoice7:
                break;
            case GamePlayType.GameChoice8:
                coinRushLockObject.SetActive(true);
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

        StopAllCoroutines();

        if(GameStateManager.instance.GameModeType == GameModeType.Perfect)
        {
            if(GameStateManager.instance.TryCount <= 0)
            {
                tryCountView.SetActive(false);

                if (!GameStateManager.instance.EventWatchAd) tryCountZeroView.SetActive(true);
            }

            tryCountText.text = GameStateManager.instance.TryCount.ToString();
        }

        if(GameStateManager.instance.GamePlayType == GamePlayType.GameChoice8)
        {
            if (GameStateManager.instance.CoinRushTryCount <= 0)
            {
                tryCountView.SetActive(false);

                if (!GameStateManager.instance.CoinRushWatchAd) tryCountZeroView.SetActive(true);
            }

            tryCountText.text = GameStateManager.instance.CoinRushTryCount.ToString();
        }
    }

    public void SuccessWatchAd()
    {
        GameStateManager.instance.EventWatchAd = true;
        GameStateManager.instance.TryCount += 1;
        tryCountText.text = GameStateManager.instance.TryCount.ToString();

        eventWatchAdView.SetActive(false);

        tryCountView.SetActive(true);
        tryCountZeroView.SetActive(false);

        noTouch = false;

        NotionManager.instance.UseNotion(NotionType.AddTryCount);
    }

    public void SuccessCoinRushAd()
    {
        GameStateManager.instance.CoinRushWatchAd = true;
        GameStateManager.instance.CoinRushTryCount += 1;
        tryCountText.text = GameStateManager.instance.CoinRushTryCount.ToString();

        eventWatchAdView.SetActive(false);

        tryCountView.SetActive(true);
        tryCountZeroView.SetActive(false);

        noTouch = false;

        NotionManager.instance.UseNotion(NotionType.AddTryCount);
    }

    public void CloseEventWatchAd()
    {
        eventWatchAdView.SetActive(false);

        noTouch = false;
    }

    #endregion

    #region Coroution
    IEnumerator MoleCatchCoroution()
    {
        ShuffleList(targetContentList);

        for(int i = 0; i < targetContentList.Count; i ++)
        {
            targetContentList[i].MoleReset();
        }

        if(countIndex <= 8)
        {
            waitForMoleCatchSeconds = new WaitForSeconds(ValueManager.instance.GetMoleCatchTime() - (0.05f * countIndex));
            waitForMoleNextSeconds = new WaitForSeconds(ValueManager.instance.GetMoleNextTime() - (0.1f * countIndex));
        }

        hitMole = false;

        targetContentList[0].SetMole();

        if (moleClone)
        {
            int random = Random.Range(0, 100);

            if(random >= 50)
            {
                targetContentList[1].SetMoleClone();
            }
        }

        yield return waitForMoleCatchSeconds;

        targetContentList[0].MoleReset();
        targetContentList[1].MoleReset();

        if (!hitMole)
        {
            countIndex = 0;
            MinusScore(0);
        }
        else
        {
            if(gameModeType == GameModeType.Hard)
            {
                countIndex += 2;
            }
            else
            {
                countIndex += 1;
            }
        }

        yield return waitForMoleNextSeconds;

        StartCoroutine(MoleCatchCoroution());
    }

    IEnumerator FilpCardCoroution()
    {
        isActive = false;
        eGamePause();

        for (int i = 0; i < targetContentList.Count; i ++ )
        {
            if(i == targetContentList.Count - 1 && gameModeType == GameModeType.Normal)
            {
                targetContentList[i].NotFilpCard();
            }
            else
            {
                targetContentList[i].filpCardImg.enabled = true;
            }
        }

        uiManager.WaitNotionUI(gamePlayType);

        yield return waitForFilpCardSeconds;

        for (int i = 0; i < targetContentList.Count; i++)
        {
            targetContentList[i].filpCardImg.enabled = false;
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

            if(gameModeType != GameModeType.Hard)
            {
                if (timingActionValue > 0)
                {
                    timingActionValue -= timingActionSpeed;
                }
            }

            timingActionFillmount.fillAmount = timingActionValue / 100f;

            timingActionCheckRange.transform.localPosition = new Vector3(timingActionRangePosX, 0, 0);

            timingActionCheckRange_1 = (timingActionCheckRange.transform.localPosition.x + 400) / 1000;
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
                if(timingActionRangePosX < timingActionRange)
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
                if (timingActionRangePosX > -timingActionRange)
                {
                    timingActionMove = true;
                }
                else
                {
                    timingActionMove = false;
                }
            }

            switch (gameModeType)
            {
                case GameModeType.Easy:
                    SetTimingActionVector(timingActionMove);
                    break;
                case GameModeType.Normal:
                    break;
                case GameModeType.Hard:
                    break;
                case GameModeType.Perfect:
                    SetTimingActionVector(timingActionMove);
                    break;
            }

            yield return new WaitForSeconds(2);
        }
    }

    public void SetTimingActionVector(bool check)
    {
        timingActionVector[0].SetActive(false);
        timingActionVector[1].SetActive(false);

        if(check)
        {
            timingActionVector[1].SetActive(true);
        }
        else
        {
            timingActionVector[0].SetActive(true);
        }
    }

    #endregion
}
