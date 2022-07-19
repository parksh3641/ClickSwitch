using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinAnimation : MonoBehaviour
{
    public MoneyType moneyType = MoneyType.Coin;

    public int coinPosX = 0;
    public int coinPosY = 0;

    public int expPosX = 0;
    public int expPosY = 0;

    [Space]
    [Title("Target Pos")]
    public Transform goldTransform;
    public Transform expTransform;

    public Transform goldTarget;
    public Transform expTarget;
    public Transform crystaltarget;

    [Space]
    [Title("Text")]
    public Text coinText;
    public Text crystalText;

    [Space]
    [Title("Prefab")]
    public CoinContent goldPrefab;
    public CoinContent expPrefab;
    public CoinContent crystalPrefab;

    [Space]
    [Title("Bool")]
    private bool coinAnim = false;

    List<CoinContent> coinPrefabList = new List<CoinContent>();
    List<CoinContent> coinPrefabList2 = new List<CoinContent>();
    List<CoinContent> expPrefabList = new List<CoinContent>();
    List<CoinContent> crystalPrefabList = new List<CoinContent>();

    private void Awake()
    {
        for(int i = 0; i < 10; i ++)
        {
            CoinContent monster = Instantiate(goldPrefab);
            monster.transform.parent = goldTransform;
            monster.transform.localPosition = Vector3.zero;
            monster.transform.localScale = new Vector3(1, 1, 1);
            monster.gameObject.SetActive(false);
            coinPrefabList.Add(monster);
        }

        for (int i = 0; i < 10; i++)
        {
            CoinContent monster = Instantiate(goldPrefab);
            monster.transform.parent = goldTransform;
            monster.transform.localPosition = Vector3.zero;
            monster.transform.localScale = new Vector3(1, 1, 1);
            monster.gameObject.SetActive(false);
            coinPrefabList2.Add(monster);
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

        for (int i = 0; i < 10; i++)
        {
            CoinContent monster = Instantiate(crystalPrefab);
            monster.transform.parent = goldTransform;
            monster.transform.localPosition = Vector3.zero;
            monster.transform.localScale = new Vector3(1, 1, 1);
            monster.gameObject.SetActive(false);
            crystalPrefabList.Add(monster);
        }
    }

    public void OnPlayCoinAnimation(MoneyType type ,int money, int plus)
    {
        switch (type)
        {
            case MoneyType.Coin:
                if(!coinAnim)
                {
                    coinAnim = true;
                    StartCoroutine(OnPlayCoinCoroution(money, plus, coinPrefabList, goldTarget, coinText));
                }
                else
                {
                    StopAllCoroutines();

                    for (int i = 0; i < coinPrefabList.Count; i++)
                    {
                        coinPrefabList[i].gameObject.SetActive(false);
                    }

                    StartCoroutine(OnPlayCoinCoroution(money, plus, coinPrefabList2, goldTarget, coinText));
                }
                break;
            case MoneyType.Crystal:
                StartCoroutine(OnPlayCoinCoroution(money, plus, crystalPrefabList, crystaltarget, crystalText));
                break;
        }
    }

    [Button]
    public void OnPlayCoin()
    {
        StartCoroutine(OnPlayCoinCoroution(0, 100, coinPrefabList, goldTarget, coinText));
    }

    [Button]
    public void OnPlayCrystal()
    {
        StartCoroutine(OnPlayCoinCoroution(0, 100, crystalPrefabList, crystaltarget, crystalText));
    }

    [Button]
    public void OnPlayExpAnimation()
    {
        StartCoroutine(OnPlayExpCoroution());
    }

    IEnumerator OnPlayCoinCoroution(int money, int plus, List<CoinContent> list, Transform target, Text text)
    {
        for(int i = 0; i < list.Count; i ++)
        {
            list[i].gameObject.SetActive(false);
        }    

        if(plus >= list.Count)
        {
            for (int i = 0; i < list.Count; i++)
            {
                list[i].gameObject.SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < plus; i++)
            {
                list[i].gameObject.SetActive(true);
            }
        }

        yield return new WaitForSeconds(1.0f);

        if (plus >= list.Count)
        {
            for (int i = 0; i < list.Count; i++)
            {
                list[i].GoToTarget(target.localPosition - new Vector3(coinPosX, coinPosY, 0));
            }
        }
        else
        {
            for (int i = 0; i < plus; i++)
            {
                list[i].GoToTarget(target.localPosition - new Vector3(coinPosX, coinPosY, 0));
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

            text.text = money.ToString();

            yield return new WaitForSeconds(0.01f);
        }

        coinAnim = false;
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
