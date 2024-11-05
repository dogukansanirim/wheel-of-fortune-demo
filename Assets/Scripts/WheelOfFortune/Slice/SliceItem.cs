using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace WheelOfFortune.Slice
{
    public class SliceItem : MonoBehaviour
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI valueText;
        
        public RectTransform IconRectTransform => iconImage.rectTransform;
        
        public int Value { get; private set; }

        public void SetSliceContent(Sprite iconSprite, int value)
        {
            iconImage.sprite = iconSprite;
            iconImage.SetNativeSize();
            Value = value;
            valueText.text = Helper.CurrencyHelper.DigitStringFormatWithLetter(value);
            gameObject.SetActive(true);
        }
        
        public void UnSetSliceContent()
        {
            gameObject.SetActive(false);
        }
    }
}