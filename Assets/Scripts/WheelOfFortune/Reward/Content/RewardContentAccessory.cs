using UnityEngine;
using WheelOfFortune.Constants;

namespace WheelOfFortune.Reward.Content
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Wheel/RewardContent/Accessory")]
    public class RewardContentAccessory : RewardContent
    {
        [SerializeField] private int accessoryId;
        public override int Id => accessoryId;
        public override RewardType RewardType => RewardType.Accessory;
    }
}
