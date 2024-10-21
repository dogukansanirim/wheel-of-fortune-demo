using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace WheelOfFortune.Reward
{
    public class RewardAnimationItem : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private RectTransform rectTransform;
        
        public void StartAnimation(Sprite sprite, float duration, RectTransform startRectTransform, RectTransform targetRectTransform)
        {
            rectTransform.SetParent(startRectTransform);
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.localScale = Vector3.zero;
            image.sprite = sprite;
            image.SetNativeSize();
            gameObject.SetActive(true);

            duration -= 0.2f;

            rectTransform.DOAnchorPos(Vector2.zero, 0.1f).onComplete = () =>
            {
                rectTransform.SetParent(targetRectTransform);
                
                rectTransform.DOAnchorPos(new Vector2(rectTransform.anchoredPosition.x + Random.Range(-250f, 250f), rectTransform.anchoredPosition.y + Random.Range(-500f, -250f)), duration / 2f).SetDelay(0.1f);
                rectTransform.DOScale(new Vector3(1f, 1f, 1), duration / 2f).SetDelay(0.1f).onComplete = () =>
                {
                    rectTransform.DOAnchorPos(Vector2.zero, duration / 2f).onComplete = () =>
                    {
                        gameObject.SetActive(false);
                    };
                };
            };
        }
    }
}