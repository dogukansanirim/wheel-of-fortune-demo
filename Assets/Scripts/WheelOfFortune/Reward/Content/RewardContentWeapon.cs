using UnityEngine;
using WheelOfFortune.Constants;

namespace WheelOfFortune.Reward.Content
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Wheel/RewardContent/Weapon")]
    public class RewardContentWeapon : RewardContent
    {
        [SerializeField] private int weaponId;
        public override int Id => weaponId;
        public override RewardType RewardType => RewardType.Weapon;
    }
}
