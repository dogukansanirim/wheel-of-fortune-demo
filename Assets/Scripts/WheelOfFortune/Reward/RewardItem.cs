using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WheelOfFortune.Constants;
using WheelOfFortune.Reward.Content;

namespace WheelOfFortune.Reward
{
    public class RewardItem : MonoBehaviour
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private RectTransform iconRectTransform;
        [SerializeField] private TextMeshProUGUI valueTextTmp;

        public RectTransform IconRectTransform => iconRectTransform;
        
        public RewardType RewardType { get; private set; }
        public virtual int Id { get; private set; }
        public int Value { get; private set; }
        
        public void SetRewardItem(RewardContent rewardContent, int value, float animationDelay)
        {
            RewardType = rewardContent.RewardType;
            Id = rewardContent.Id;
            Value = value;
            iconImage.sprite = rewardContent.IconSprite;
            iconImage.SetNativeSize();
            StartIconPunchScaleAnimation(animationDelay);
            StartValueChangeAnimation(0, Value, 1f, animationDelay);
        }

        private readonly Vector3 _iconPunchScaleVector = new (0.3f, 0.3f, 0);
        
        public void AddToValue(int valueToAdd, float animationDelay)
        {
            StartValueChangeAnimation(Value, Value + valueToAdd, 1f, animationDelay);
            Value += valueToAdd;
            StartIconPunchScaleAnimation(animationDelay);
        }

        private void StartIconPunchScaleAnimation(float delay)
        {
            iconRectTransform.DOPunchScale(_iconPunchScaleVector, 0.3f, 1).SetDelay(delay);
        }

        private void SetValueText(int value)
        {
            valueTextTmp.text = Helper.CurrencyHelper.DigitStringFormatWithComma(value);
        }

        private IEnumerator _valueChangeAnimation;

        private void StartValueChangeAnimation(int startValue, int endValue, float duration, float delay)
        {
            StopValueChangeAnimation();
            _valueChangeAnimation = ValueChangeAnimation(startValue, endValue, duration, delay);
            StartCoroutine(_valueChangeAnimation);
        }
        
        private void StopValueChangeAnimation()
        {
            if (_valueChangeAnimation == null) return;
            StopCoroutine(_valueChangeAnimation);
            _valueChangeAnimation = null;
        }
        
        private IEnumerator ValueChangeAnimation(int startValue, int endValue, float duration, float delay)
        {
            yield return new WaitForSeconds(delay);
            
            float ratio = (endValue - startValue) / 100f;

            int increaseValue = 1;
            if (ratio > 1)
            {
                increaseValue = (int) ratio;
            }
            
            for (int i = startValue; i < endValue + 1; i += increaseValue)
            {
                yield return new WaitForSeconds(duration / ((endValue-startValue) / (float)increaseValue));
                SetValueText(i);
            }
            
            SetValueText(endValue);
            _valueChangeAnimation = null;
        }
    }
}