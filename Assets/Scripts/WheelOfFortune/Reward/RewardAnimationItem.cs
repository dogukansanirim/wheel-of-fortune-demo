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

        private const float SpawnPosXRandomAdditionValueMin = -250f;
        private const float SpawnPosXRandomAdditionValueMax = 250f;
        private const float SpawnPosYRandomAdditionValueMin = -500f;
        private const float SpawnPosYRandomAdditionValueMax = -250f;
        
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
                
                rectTransform.DOAnchorPos(new Vector2(rectTransform.anchoredPosition.x + Random.Range(SpawnPosXRandomAdditionValueMin, SpawnPosXRandomAdditionValueMax), rectTransform.anchoredPosition.y + Random.Range(SpawnPosYRandomAdditionValueMin, SpawnPosYRandomAdditionValueMax)), duration / 2f).SetDelay(0.1f);
                rectTransform.DOScale(new Vector3(1f, 1f, 1), duration / 2f).SetDelay(0.1f).onComplete = () =>
                {
                    rectTransform.DOAnchorPos(Vector2.zero, duration / 2f).onComplete = () =>
                    {
                        gameObject.SetActive(false);
                    };
                };
            };
        }

        private void OnDestroy()
        {
            rectTransform.DOKill();
        }
    }
}