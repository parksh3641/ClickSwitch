using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RewardClass
{
    public RewardReceiveType rewardReceiveType = RewardReceiveType.Free;
    public RewardType rewardType = RewardType.Coin;
    public int count = 0;
    [Space]
    public IconType iconType = IconType.Icon_0;
    [Space]
    public BannerType bannerType = BannerType.Banner_0;
}

[CreateAssetMenu(fileName = "ProgressDataBase", menuName = "ScriptableObjects/ProgressDataBase")]
public class ProgressDataBase : ScriptableObject
{
    [Title("Free Reward")]
    public List<RewardClass> freeRewardList = new List<RewardClass>();

    [Space]
    [Title("Paid Reward")]
    public List<RewardClass> paidRewardList = new List<RewardClass>();
}
