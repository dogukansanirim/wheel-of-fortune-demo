using TMPro;
using UnityEngine;

namespace WheelOfFortune.Zone
{
    public class ZoneNumberItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI tmp;

        public void SetText(int value, Color color)
        {
            tmp.text = "" + value;
            tmp.color = color;
        }

        public void SetPassive()
        {
            Color color = tmp.color;
            tmp.color = new Color(color.r, color.g, color.b, color.a * 0.4f);
        }
    }
}