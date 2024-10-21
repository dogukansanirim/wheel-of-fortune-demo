using UnityEngine;
using WheelOfFortune.Constants;

namespace WheelOfFortune.Reward.Content
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Wheel/RewardContent/PointsItem")]
    public class RewardContentPointsItem : RewardContent
    {
        [SerializeField] private int pointsItemId;
        public override int Id => pointsItemId;
        public override RewardType RewardType => RewardType.PointsItem;
    }
}
