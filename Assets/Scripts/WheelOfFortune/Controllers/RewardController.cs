using System.Collections.Generic;
using UnityEngine;
using WheelOfFortune.Reward;
using WheelOfFortune.Reward.Content;
using WheelOfFortune.Single;

namespace WheelOfFortune.Controllers
{
    public class RewardController : MonoBehaviour
    {
        [SerializeField] private RewardPanelSettings rewardPanelSettings;
        [SerializeField] private RectTransform rewardPanelRectTransform;
        [SerializeField] private RewardItem rewardItemPrefab;
        [SerializeField] private List<RewardAnimationItem> rewardAnimationItemList;

        private readonly List<RewardItem> _givenRewards = new();
        
        private bool _isRewardActive;

        public bool IsRewardActive
        {
            get => _isRewardActive;
            private set
            {
                _isRewardActive = value;
                WheelSingleton.Instance.Signal.TriggerStateMachine?.Invoke();
            }
        }
        
        private void Start()
        {
            WheelSingleton.Instance.RewardController = this;
        }
        
        public void ActivateCurrentReward(int value, RewardContent rewardContent, RectTransform startRectTransform)
        {
            IsRewardActive = true;

            RewardItem rewardItem = _givenRewards.Find(givenReward =>
                givenReward.RewardType == rewardContent.RewardType && givenReward.Id == rewardContent.Id);
            
            if (rewardItem == null)
            {
                rewardItem = Instantiate(rewardItemPrefab, rewardPanelRectTransform);
                _givenRewards.Add(rewardItem);
                rewardItem.SetRewardItem(rewardContent, value, rewardPanelSettings.RewardMoveAnimationDuration);
                StartRewardAnimation(rewardContent.IconSprite, rewardPanelSettings.RewardMoveAnimationDuration, startRectTransform, rewardItem.IconRectTransform);
            }
            else
            {
                StartRewardAnimation(rewardContent.IconSprite, rewardPanelSettings.RewardMoveAnimationDuration, startRectTransform, rewardItem.IconRectTransform);
                rewardItem.AddToValue(value, rewardPanelSettings.RewardMoveAnimationDuration);
            }
            
            WheelSingleton.Instance.SetTimout(() =>
            {
                IsRewardActive = false;
            }, rewardPanelSettings.RewardMoveAnimationDuration);
        }

        private void StartRewardAnimation(Sprite rewardSprite, float duration, RectTransform startRectTransform, RectTransform targetRectTransform)
        {
            foreach (var rewardAnimationItem in rewardAnimationItemList)
            {
                rewardAnimationItem.StartAnimation(rewardSprite, duration, startRectTransform, targetRectTransform);
            }
        }
    }
}