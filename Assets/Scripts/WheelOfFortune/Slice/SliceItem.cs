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
            
            switch (value)
            {
                case <= 0:
                    valueText.text = ""; 
                    break;
                case >= 1_000_000:
                    valueText.text = "x" + value/1_000_000 + "M";
                    break;
                case >= 1_000:
                    valueText.text = "x" + value/1_000 + "K";
                    break;
                default:
                    valueText.text = "x" + value;
                    break;
            }
            
            gameObject.SetActive(true);
        }
        
        public void UnSetSliceContent()
        {
            gameObject.SetActive(false);
        }
    }
}