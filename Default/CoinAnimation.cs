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
    public Text[] coinText;
    public Text[] crystalText;

    [Space]
    [Title("Bool")]
    public bool isCoin = false;
    public bool isCrystal = false;

    [Space]
    [Title("Prefab")]
    public CoinContent goldPrefab;
    public CoinContent expPrefab;
    public CoinContent crystalPrefab;

    List<CoinContent> coinPrefabList = new List<CoinContent>();
    List<CoinContent> crystalPrefabList = new List<CoinContent>();
    List<CoinContent> expPrefabList = new List<CoinContent>();

    private void Awake()
    {
        for (int i = 0; i < 10; i++)
        {
            CoinContent monster = Instantiate(goldPrefab);
            monster.transform.SetParent(goldTransform);
            monster.transform.localPosition = Vector3.zero;
            monster.transform.localScale = new Vector3(1, 1, 1);
            monster.gameObject.SetActive(false);
            coinPrefabList.Add(monster);
        }

        for (int i = 0; i < 10; i++)
        {
            CoinContent monster = Instantiate(expPrefab);
            monster.transform.SetParent(expTransform);
            monster.transform.localPosition = Vector3.zero;
            monster.transform.localScale = new Vector3(1, 1, 1);
            monster.gameObject.SetActive(false);
            expPrefabList.Add(monster);
        }

        for (int i = 0; i < 10; i++)
        {
            CoinContent monster = Instantiate(crystalPrefab);
            monster.transform.SetParent(goldTransform);
            monster.transform.localPosition = Vector3.zero;
            monster.transform.localScale = new Vector3(1, 1, 1);
            monster.gameObject.SetActive(false);
            crystalPrefabList.Add(monster);
        }
    }

    public void OnPlayMoneyAnimation(MoneyType type, int money, int plus)
    {
        switch (type)
        {
            case MoneyType.Coin:

                if(!isCoin)
                {
                    isCoin = true;

                    for (int i = 0; i < coinPrefabList.Count; i++)
                    {
                        coinPrefabList[i].gameObject.SetActive(false);
                    }

                    StartCoroutine(OnPlayCoinCoroution(money, plus, coinPrefabList, goldTarget, coinText));
                }
                else
                {
                    isCoin = false;

                    OnPlayMoneyAnimation(type, money, plus);
                }

                break;
            case MoneyType.Crystal:

                if (!isCrystal)
                {
                    isCrystal = true;

                    for (int i = 0; i < crystalPrefabList.Count; i++)
                    {
                        crystalPrefabList[i].gameObject.SetActive(false);
                    }

                    StartCoroutine(OnPlayCrystalCoroution(money, plus, crystalPrefabList, crystaltarget, crystalText));
                }
                else
                {
                    isCrystal = false;

                    OnPlayMoneyAnimation(type, money, plus);
                }
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
        StartCoroutine(OnPlayCrystalCoroution(0, 100, crystalPrefabList, crystaltarget, crystalText));
    }

    [Button]
    public void OnPlayExpAnimation()
    {
        StartCoroutine(OnPlayExpCoroution());
    }

    IEnumerator OnPlayCoinCoroution(int money, int plus, List<CoinContent> list, Transform target, Text[] text)
    {
        while (isCoin)
        {

            for (int i = 0; i < list.Count; i++)
            {
                list[i].gameObject.SetActive(false);
            }

            if (plus >= list.Count)
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

            while (money < max)
            {
                if (money + 100 < max)
                {
                    money += 100;
                }
                else
                {
                    money += 1;
                }

                text[0].text = money.ToString();
                text[1].text = money.ToString();

                yield return new WaitForSeconds(0.01f);
            }

            isCoin = false;
        }
    }

    IEnumerator OnPlayCrystalCoroution(int money, int plus, List<CoinContent> list, Transform target, Text[] text)
    {
        while (isCrystal)
        {

            for (int i = 0; i < list.Count; i++)
            {
                list[i].gameObject.SetActive(false);
            }

            if (plus >= list.Count)
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

            while (money < max)
            {
                if (money + 100 < max)
                {
                    money += 100;
                }
                else
                {
                    money += 1;
                }

                text[0].text = money.ToString();
                text[1].text = money.ToString();

                yield return new WaitForSeconds(0.01f);
            }

            isCrystal = false;
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
