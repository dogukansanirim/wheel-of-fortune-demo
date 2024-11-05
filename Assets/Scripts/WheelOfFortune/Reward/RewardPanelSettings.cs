using UnityEngine;

namespace WheelOfFortune.Reward
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Wheel/RewardPanelSettings")]
    public class RewardPanelSettings : ScriptableObject
    {
        [SerializeField] [Range(0, 5f)] private float rewardMoveAnimationDuration;
        
        public float RewardMoveAnimationDuration => rewardMoveAnimationDuration;
    }
}