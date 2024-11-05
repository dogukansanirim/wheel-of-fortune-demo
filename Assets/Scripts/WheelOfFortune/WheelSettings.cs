using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using WheelOfFortune.Helper;
using WheelOfFortune.Reward;

namespace WheelOfFortune
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Wheel/WheelSettings")]
    public class WheelSettings : ScriptableObject
    {
        [SerializeField] private List<WheelSpinContent> spinContents;
        [SerializeField] private int sliceCount;
        [SerializeField] private float sliceLoadDuration;
        [SerializeField] [Range(0,1)] private float sliceAngleMaxDeflectionRatio;
        [SerializeField] private int spinLoopCount;
        [SerializeField] private float spinDuration;
        [SerializeField] private Ease spinEase;
        [SerializeField] private int safeZonePerXSpin;
        [SerializeField] private int superZonePerXSpin;
        [SerializeField] private Color basicZoneNumberColor;
        [SerializeField] private Color safeZoneNumberColor;
        [SerializeField] private Color superZoneNumberColor;
        [SerializeField] private float fadeInOutDuration;
        [SerializeField] private float bombAnimationDuration;
        [SerializeField] private float bombAnimationPunchValue;

        public List<WheelSpinContent> SpinContents => spinContents;

        public int SliceCount => sliceCount;
        public float SingleSliceAngle => 360 / (float)SliceCount;
        public float SliceAngleMaxDeflectionRatio => sliceAngleMaxDeflectionRatio;
        public float SliceLoadDuration => sliceLoadDuration;
        public int SpinLoopCount => spinLoopCount;
        public float SpinDuration => spinDuration;
        public Ease SpinEase => spinEase;
        public int SafeZonePerXSpin => safeZonePerXSpin;
        public int SuperZonePerXSpin => superZonePerXSpin;
        public Color BasicZoneNumberColor => basicZoneNumberColor;
        public Color SafeZoneNumberColor => safeZoneNumberColor;
        public Color SuperZoneNumberColor => superZoneNumberColor;
        public float FadeInOutDuration => fadeInOutDuration;
        public float BombAnimationDuration => bombAnimationDuration;
        public float BombAnimationPunchValue => bombAnimationPunchValue;
    }
}