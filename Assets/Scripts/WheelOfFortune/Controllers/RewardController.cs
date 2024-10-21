using System.Collections.Generic;
using UnityEngine;
using WheelOfFortune.Reward;
using WheelOfFortune.Reward.Content;
using WheelOfFortune.Single;

namespace WheelOfFortune.Controllers
{
    public class RewardController : MonoBehaviour
    {
        [SerializeField] private RectTransform rewardPanelRectTransform;
        [SerializeField] private GameObject rewardItemPrefab;
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
            
            float animationDuration = 2f;

            RewardItem rewardItem = _givenRewards.Find(givenReward =>
                givenReward.RewardType == rewardContent.RewardType && givenReward.Id == rewardContent.Id);
            
            if (rewardItem == null)
            {
                rewardItem = Instantiate(rewardItemPrefab, rewardPanelRectTransform).GetComponent<RewardItem>();
                _givenRewards.Add(rewardItem);
                rewardItem.SetRewardItem(rewardContent, value, animationDuration);
                StartRewardAnimation(rewardContent.IconSprite, animationDuration, startRectTransform, rewardItem.IconRectTransform);
            }
            else
            {
                StartRewardAnimation(rewardContent.IconSprite, animationDuration, startRectTransform, rewardItem.IconRectTransform);
                rewardItem.AddToValue(value, animationDuration);
            }
            
            WheelSingleton.Instance.SetTimout(() =>
            {
                IsRewardActive = false;
            }, animationDuration);
        }

        private void StartRewardAnimation(Sprite rewardSprite, float duration, RectTransform startRectTransform, RectTransform targetRectTransform)
        {
            rewardAnimationItemList.ForEach(rewardAnimationItem => 
                rewardAnimationItem.StartAnimation(rewardSprite, duration, startRectTransform, targetRectTransform));
        }
    }
}