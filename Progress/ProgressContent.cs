using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressContent : MonoBehaviour
{
    public Text numberText;

    public RewardContent[] rewardContents;

    public void Initialize(int number, ProgressManager manager, RewardClass freeRewardClass, RewardClass paidRewardClass)
    {
        numberText.text = (number + 1).ToString();

        rewardContents[0].Initialize(manager, freeRewardClass, number);
        rewardContents[1].Initialize(manager, paidRewardClass, number);
    }

    public void UnLockFree()
    {
        rewardContents[0].UnLock();
    }

    public void CheckMarkFree()
    {
        rewardContents[0].CheckMark(true);
    }

    public void UnLockPaid()
    {
        rewardContents[1].UnLock();
    }

    public void CheckMarkPaid()
    {
        rewardContents[1].CheckMark(true);
    }
}
