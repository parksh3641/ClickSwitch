using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class IconBoxManager : MonoBehaviour
{

    public GameObject boxView;

    public Text boxCountText;

    public GameObject[] buttons;

    [Title("Content")]
    public IconBoxContent iconBoxContent;

    public Transform iconBoxContentTransform;


    [Space]
    [Title("Box")]
    public Image boxIcon;

    public Sprite boxOpenIcon;
    public GameObject boxOpenEffect;

    public int boxCount = 0;
    private int iconNumber = 0;
    private bool waitBox = false;


    List<IconBoxContent> iconBoxContentList = new List<IconBoxContent>();


    public SoundManager soundManager;
    ShopDataBase shopDataBase;
    PlayerDataBase playerDataBase;

    private void Awake()
    {
        if (shopDataBase == null) shopDataBase = Resources.Load("ShopDataBase") as ShopDataBase;
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;

        boxView.SetActive(false);

        for(int i = 0; i < buttons.Length; i ++)
        {
            buttons[i].SetActive(false);
        }

        PlayerDataBase.eGetBox += OpenBoxView;
    }


    private void OnApplicationQuit()
    {
        PlayerDataBase.eGetBox -= OpenBoxView;
    }

    public void Initialize()
    {
        iconNumber = System.Enum.GetValues(typeof(IconType)).Length;

        for (int i = 0; i < iconNumber; i++)
        {
            IconBoxContent monster = Instantiate(iconBoxContent);
            monster.transform.parent = iconBoxContentTransform;
            monster.transform.position = Vector3.zero;
            monster.transform.rotation = Quaternion.identity;
            monster.transform.localScale = Vector3.one;

            monster.Initialize(IconType.Icon_0 + i);

            monster.gameObject.SetActive(false);

            iconBoxContentList.Add(monster);
        }
    }

    public void OpenBoxView()
    {
        if (!boxView.activeInHierarchy)
        {
            boxView.SetActive(true);

            InitializeBox();
        }
    }

    public void CloseBoxView()
    {
        waitBox = false;
        boxView.SetActive(false);
    }

    void InitializeBox()
    {
        boxCount = playerDataBase.IconBox;
        boxCountText.text = boxCount.ToString();

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].SetActive(false);
        }

        if (boxCount > 1)
        {
            buttons[1].SetActive(true);
            buttons[0].SetActive(true);
        }
        else
        {
            buttons[0].SetActive(true);
        }

        for (int i = 0; i < iconBoxContentList.Count; i++)
        {
            iconBoxContentList[i].gameObject.SetActive(false);
        }

        Debug.Log("BoxView Reset!");
    }

    public void OpenBoxOne()
    {
        if(boxCount > 0)
        {
            if (waitBox) return;
            RandomIcon(1);
        }
    }

    public void OpenBoxALL()
    {
        if (boxCount > 0)
        {
            if (waitBox) return;
            RandomIcon(boxCount);
        }
    }

    void RandomIcon(int number)
    {
        StartCoroutine(RandomIconCoroution(number));
    }

    IEnumerator RandomIconCoroution(int number)
    {
        waitBox = true;

        for (int i = 0; i < number; i ++)
        {
            int random = Random.Range(3, iconNumber);

            if(shopDataBase.GetIconNumber(IconType.Icon_0 + random) + 1 < 5)
            {
                GetIcon(random);
            }
            else
            {
                Debug.Log("이미 5개 이상 보유 하고 있습니다 돈으로 지급하");

            }



            CheckBoxCount();
            yield return new WaitForSeconds(0.5f);
        }

        waitBox = false;
    }

    void GetIcon(int number)
    {
        iconBoxContentList[number].gameObject.SetActive(true);
        iconBoxContentList[number].AddCount();
        shopDataBase.AddIcon(IconType.Icon_0 + number);
        if (PlayfabManager.instance.isActive) PlayfabManager.instance.GrantItemsToUser("Icon_" + number, "Icon");
    }

    void CheckBoxCount()
    {
        boxCount -= 1;
        boxCountText.text = boxCount.ToString();

        if (boxCount <= 0)
        {
            StopAllCoroutines();
            buttons[0].SetActive(false);
            buttons[1].SetActive(false);
            buttons[2].SetActive(true);

            playerDataBase.IconBox = 0;
            if (PlayfabManager.instance.isActive) PlayfabManager.instance.UpdatePlayerStatisticsInsert("IconBox", 0);
        }
    }
}
