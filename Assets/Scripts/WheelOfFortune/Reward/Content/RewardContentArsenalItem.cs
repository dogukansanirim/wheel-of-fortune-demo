using UnityEngine;
using WheelOfFortune.Constants;

namespace WheelOfFortune.Reward.Content
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Wheel/RewardContent/ArsenalItem")]
    public class RewardContentArsenalItem : RewardContent
    {
        [SerializeField] private int arsenalItemId;
        public override int Id => arsenalItemId;
        public override RewardType RewardType => RewardType.ArsenalItem;
    }
}
