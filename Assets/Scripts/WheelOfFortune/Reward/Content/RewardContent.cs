using UnityEngine;
using WheelOfFortune.Constants;

namespace WheelOfFortune.Reward.Content
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Wheel/RewardContent/Basic")]
    public class RewardContent : ScriptableObject
    {
        [SerializeField] private RewardType rewardType;
        [SerializeField] private Sprite iconSprite;
        [SerializeField] private int minValue;
        [SerializeField] private int maxValue;
        
        public virtual RewardType RewardType => rewardType;
        public Sprite IconSprite => iconSprite;
        public int MinValue => minValue;
        public int MaxValue => maxValue;
        public virtual int Id => -1;
    }
}
