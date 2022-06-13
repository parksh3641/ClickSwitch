using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinAnimation : MonoBehaviour
{
    public Transform target;
    public MoneyType moneyType = MoneyType.Gold;

    public Text goldText;

    public CoinContent goldPrefab;

    public Transform goldTransform;

    public List<CoinContent> goldPrefabList = new List<CoinContent>();

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
    }

    public void OnPlay(int money, int plus)
    {
        StartCoroutine(OnPlayCorution(money, plus));
    }

    [Button]
    public void OnPlay()
    {
        StartCoroutine(OnPlayCorution(0, 100));
    }

    IEnumerator OnPlayCorution(int money, int plus)
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
                goldPrefabList[i].GoToTarget(target.localPosition - new Vector3(350, 35, 0));
            }
        }
        else
        {
            for (int i = 0; i < plus; i++)
            {
                goldPrefabList[i].GoToTarget(target.localPosition - new Vector3(350, 35, 0));
            }
        }

        yield return new WaitForSeconds(0.5f);

        int max = money + plus;

        while(money < max)
        {
            money += 1;
            goldText.text = money.ToString();

            yield return new WaitForSeconds(0.01f);
        }

    }
}
