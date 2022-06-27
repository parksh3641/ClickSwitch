using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinAnimation : MonoBehaviour
{
    public int coinPosX = 0;
    public int coinPosY = 0;

    public int expPosX = 0;
    public int expPosY = 0;

    public Transform goldTransform;
    public Transform expTransform;

    public Transform goldTarget;
    public Transform expTarget;

    public MoneyType moneyType = MoneyType.Coin;

    public Text goldText;

    public CoinContent goldPrefab;
    public CoinContent expPrefab;


    public List<CoinContent> goldPrefabList = new List<CoinContent>();
    public List<CoinContent> expPrefabList = new List<CoinContent>();

    private void Awake()
    {
        for(int i = 0; i < 10; i ++)
        {
            CoinContent monster = Instantiate(goldPrefab);
            monster.transform.parent = goldTransform;
            monster.transform.localPosition = Vector3.zero;
            monster.transform.localScale = new Vector3(1, 1, 1);
            monster.gameObject.SetActive(false);
            goldPrefabList.Add(monster);
        }

        for (int i = 0; i < 10; i++)
        {
            CoinContent monster = Instantiate(expPrefab);
            monster.transform.parent = expTransform;
            monster.transform.localPosition = Vector3.zero;
            monster.transform.localScale = new Vector3(1, 1, 1);
            monster.gameObject.SetActive(false);
            expPrefabList.Add(monster);
        }
    }

    public void OnPlayCoinAnimation(int money, int plus)
    {
        StartCoroutine(OnPlayCoinCoroution(money, plus));
    }

    [Button]
    public void OnPlayExpAnimation()
    {
        StartCoroutine(OnPlayExpCoroution());
    }

    [Button]
    public void OnPlay()
    {
        StartCoroutine(OnPlayCoinCoroution(0, 100));
    }

    IEnumerator OnPlayCoinCoroution(int money, int plus)
    {
        if(plus >= goldPrefabList.Count)
        {
            for (int i = 0; i < goldPrefabList.Count; i++)
            {
                goldPrefabList[i].gameObject.SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < plus; i++)
            {
                goldPrefabList[i].gameObject.SetActive(true);
            }
        }

        yield return new WaitForSeconds(1.0f);

        if (plus >= goldPrefabList.Count)
        {
            for (int i = 0; i < goldPrefabList.Count; i++)
            {
                goldPrefabList[i].GoToTarget(goldTarget.localPosition - new Vector3(coinPosX, coinPosY, 0));
            }
        }
        else
        {
            for (int i = 0; i < plus; i++)
            {
                goldPrefabList[i].GoToTarget(goldTarget.localPosition - new Vector3(coinPosX, coinPosY, 0));
            }
        }

        yield return new WaitForSeconds(0.5f);

        int max = money + plus;

        while(money < max)
        {
            if(money + 100 < max)
            {
                money += 100;
            }
            else
            {
                money += 1;
            }

            goldText.text = money.ToString();

            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator OnPlayExpCoroution()
    {
        for (int i = 0; i < expPrefabList.Count; i++)
        {
            expPrefabList[i].gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(1.0f);

        for (int i = 0; i < expPrefabList.Count; i++)
        {
            expPrefabList[i].GoToTarget(expTarget.localPosition - new Vector3(expPosX, expPosY, 0));
        }
    }
}
