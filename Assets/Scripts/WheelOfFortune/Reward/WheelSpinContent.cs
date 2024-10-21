using System.Collections.Generic;
using UnityEngine;
using WheelOfFortune.Reward.Content;

namespace WheelOfFortune.Reward
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Wheel/WheelSpinContent")]
    public class WheelSpinContent : ScriptableObject
    {
        [SerializeField] private int spinNo;
        public int SpinNo => spinNo;
        
        [SerializeField] private List<RewardContent> sliceRewardContents;
        public List<RewardContent> SliceRewards => sliceRewardContents;
    }
}