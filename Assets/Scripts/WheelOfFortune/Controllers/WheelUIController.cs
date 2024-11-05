using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using WheelOfFortune.Constants;
using WheelOfFortune.Reward.Content;
using WheelOfFortune.Single;
using WheelOfFortune.Slice;
using WheelOfFortune.Zone;
using Random = UnityEngine.Random;

namespace WheelOfFortune.Controllers
{
    public class WheelUIController : MonoBehaviour
    {
        [SerializeField] private WheelSettings wheelSettings;
        [SerializeField] private GameObject zoneNumberPrefab;
        [SerializeField] private RectTransform zoneNumberBlockRectTransform;
        [SerializeField] private SliceItem[] sliceItems;
        [SerializeField] private RectTransform bombRectTransform;
        [Header("Wheel Image Gameobjects")]
        [SerializeField] private GameObject wheelBasicZoneImageGameObject;
        [SerializeField] private GameObject wheelSafeZoneImageGameObject;
        [SerializeField] private GameObject wheelSuperZoneImageGameObject;
        [Header("Wheel Indicator Image Gameobjects")]
        [SerializeField] private GameObject wheelIndicatorBasicZoneImageGameObject;
        [SerializeField] private GameObject wheelIndicatorSafeZoneImageGameObject;
        [SerializeField] private GameObject wheelIndicatorSuperZoneImageGameObject;
        [Header("Spin Counter Block Image Gameobjects")]
        [SerializeField] private GameObject spinCounterBasicImageGameObject;
        [SerializeField] private GameObject spinCounterSafeImageGameObject;
        [SerializeField] private GameObject spinCounterSuperImageGameObject;
        
        [SerializeField, HideInInspector] private TextMeshProUGUI safeZoneInfoTmp;
        [SerializeField, HideInInspector] private TextMeshProUGUI superZoneInfoTmp;
        [SerializeField, HideInInspector] private RectTransform wheelRotateRectTransform;
        [SerializeField, HideInInspector] private Image fadeInOutImage;
        [SerializeField, HideInInspector] private Button spinButton;
        [SerializeField, HideInInspector] private Button exitButton;

        private float _zoneNumberPrefabWidth;
        private readonly List<ZoneNumberItem> _zoneNumberItems = new ();
        
        public RectTransform SliceRewardIconRectTransform { get; private set; }

        private int _spinNo = -1;
        public bool IsLastSpin => _spinNo == wheelSettings.SpinContents.Count - 1;
        
        private bool _isFadeInOutActive;

        public bool IsFadeInOutActive
        {
            get => _isFadeInOutActive;
            private set
            {
                _isFadeInOutActive = value;
                WheelSingleton.Instance.Signal.TriggerStateMachine?.Invoke();
            }
        }
        
        private bool _isBombAnimationOver;

        public bool IsBombAnimationOver
        {
            get => _isBombAnimationOver;
            private set
            {
                _isBombAnimationOver = value;
                WheelSingleton.Instance.Signal.TriggerStateMachine?.Invoke();
            }
        }
        
        private bool _isWheelReloaded;

        public bool IsWheelReloaded
        {
            get => _isWheelReloaded;
            private set
            {
                _isWheelReloaded = value;
                WheelSingleton.Instance.Signal.TriggerStateMachine?.Invoke();
            }
        }
        
        private bool _isWheelSpinning;

        public bool IsWheelSpinning
        {
            get => _isWheelSpinning;
            private set
            {
                _isWheelSpinning = value;
                WheelSingleton.Instance.Signal.TriggerStateMachine?.Invoke();
            }
        }
        
        private bool _isExitBtnClicked;

        public bool IsExitBtnClicked
        {
            get => _isExitBtnClicked;
            private set
            {
                _isExitBtnClicked = value;
                WheelSingleton.Instance.Signal.TriggerStateMachine?.Invoke();
            }
        }

        private List<RewardContent> _currentSpinRewards;
        
        public RewardContent CurrentReward { get; private set; }
        public int CurrentRewardValue { get; private set; }
        
        public bool IsCurrentRewardBomb => CurrentReward.RewardType == RewardType.GameOver;
        
        private void Awake()
        {
            SetSpinButtonInteractable(false);
            spinButton.onClick.AddListener(SpinBtnOnClick);
            SetExitButtonInteractable(false);
            exitButton.onClick.AddListener(ExitBtnOnClick);
            fadeInOutImage.color = new Color(0, 0, 0, 1);
            UnSetSliceItems();
        }

        private void Start()
        {
            ConfigureZonePanel();
            WheelSingleton.Instance.WheelUIController = this;
        }
        
        #region FadeInOut

        public void StartFadeOut()
        {
            IsFadeInOutActive = true;
            fadeInOutImage.DOFade(0, 2).SetEase(Ease.InCirc).onComplete = () =>
            {
                IsFadeInOutActive = false;
            };
        }
        
        public void StartFadeIn()
        {
            IsFadeInOutActive = true;
            fadeInOutImage.DOFade(1, 2).SetEase(Ease.OutCirc).onComplete = () =>
            {
                IsFadeInOutActive = false;
            };
        }

        #endregion

        #region Configure

        private void ConfigureZonePanel()
        {
            safeZoneInfoTmp.text = "" + wheelSettings.SafeZonePerXSpin;
            superZoneInfoTmp.text = "" + wheelSettings.SuperZonePerXSpin;
            
            _zoneNumberPrefabWidth = zoneNumberPrefab.GetComponent<RectTransform>().rect.width;
            
            for (var i = 0; i < wheelSettings.SpinContents.Count; i++)
            {
                ZoneNumberItem zoneNumberItem = Instantiate(zoneNumberPrefab, zoneNumberBlockRectTransform).GetComponent<ZoneNumberItem>();
                _zoneNumberItems.Add(zoneNumberItem);
                Color color = wheelSettings.BasicZoneNumberColor;

                if ((i + 1) % wheelSettings.SuperZonePerXSpin == 0)
                {
                    color = wheelSettings.SuperZoneNumberColor;
                } else if ((i + 1) % wheelSettings.SafeZonePerXSpin == 0)
                {
                    color = wheelSettings.SafeZoneNumberColor;
                }
                
                zoneNumberItem.SetText(i+1, color);
            }
        }

        public void UnSetSliceItems()
        {
            foreach (var sliceItem in sliceItems)
            {
                sliceItem.UnSetSliceContent();
            }
        }
        
        public void ReloadWheel()
        {
            UnSetSliceItems();
            
            _spinNo++;
            zoneNumberBlockRectTransform.DOAnchorPosX(-_spinNo * _zoneNumberPrefabWidth, 1);
            if (_spinNo != 0) _zoneNumberItems[_spinNo-1].SetPassive();
            
            if ((_spinNo + 1) % wheelSettings.SuperZonePerXSpin == 0)
            {
                wheelSuperZoneImageGameObject.SetActive(true);
                wheelIndicatorSuperZoneImageGameObject.SetActive(true);
                spinCounterSuperImageGameObject.SetActive(true);
                
                wheelSafeZoneImageGameObject.SetActive(false);
                wheelIndicatorSafeZoneImageGameObject.SetActive(false);
                wheelBasicZoneImageGameObject.SetActive(false);
                wheelIndicatorBasicZoneImageGameObject.SetActive(false);
                spinCounterBasicImageGameObject.SetActive(false);
                spinCounterSafeImageGameObject.SetActive(false);
            } else if ((_spinNo + 1) % wheelSettings.SafeZonePerXSpin == 0)
            {
                wheelSafeZoneImageGameObject.SetActive(true);
                wheelIndicatorSafeZoneImageGameObject.SetActive(true);
                spinCounterSafeImageGameObject.SetActive(true);
                
                wheelSuperZoneImageGameObject.SetActive(false);
                wheelIndicatorSuperZoneImageGameObject.SetActive(false);
                wheelBasicZoneImageGameObject.SetActive(false);
                wheelIndicatorBasicZoneImageGameObject.SetActive(false);
                spinCounterBasicImageGameObject.SetActive(false);
                spinCounterSuperImageGameObject.SetActive(false);
            }
            else
            {
                wheelBasicZoneImageGameObject.SetActive(true);
                wheelIndicatorBasicZoneImageGameObject.SetActive(true);
                spinCounterBasicImageGameObject.SetActive(true);
                
                wheelSafeZoneImageGameObject.SetActive(false);
                wheelIndicatorSafeZoneImageGameObject.SetActive(false);
                wheelSuperZoneImageGameObject.SetActive(false);
                wheelIndicatorSuperZoneImageGameObject.SetActive(false);
                spinCounterSafeImageGameObject.SetActive(false);
                spinCounterSuperImageGameObject.SetActive(false);
            }
            
            wheelRotateRectTransform.rotation = Quaternion.identity;
            _currentSpinRewards = wheelSettings.SpinContents[_spinNo].SliceRewards;
            
            for (var i = 0; i < _currentSpinRewards.Count; i++)
            {
                int sliceIndex = i;
                
                WheelSingleton.Instance.SetTimout(() =>
                {
                    RewardContent reward = _currentSpinRewards[sliceIndex];

                    int value;
                    
                    if (reward.MinValue == reward.MaxValue)
                    {
                        value = reward.MinValue;
                    }
                    else
                    {
                        int minValue = reward.MinValue;
                        int maxValue = reward.MaxValue;
                        float spinRatio = 5 * (_spinNo + 1) / (float) wheelSettings.SpinContents.Count;
                        
                        value = (int) (Random.Range(minValue, maxValue + 1) * spinRatio);
                        
                        if (value < 1) value = 1;
                    }
                    
                    sliceItems[sliceIndex].SetSliceContent(reward.IconSprite, value);
                    
                    if (sliceIndex == _currentSpinRewards.Count - 1)
                    {
                        IsWheelReloaded = true;
                    } 
                }, i * wheelSettings.SliceLoadDuration);
            }
        }

        #endregion
        
        #region Buttons

        public void SetSpinButtonInteractable(bool value)
        {
            spinButton.interactable = value;
        }
        
        private void SpinBtnOnClick()
        {
            IsWheelSpinning = true;
            int finishedSlice = Random.Range(0, wheelSettings.SliceCount);
            float finishedAngle = Random.Range(wheelSettings.SingleSliceAngle / -4, wheelSettings.SingleSliceAngle / 4); //to prevent wheel stop between two slices
            float rotateAngle = -360f * wheelSettings.SpinLoopCount + -finishedSlice * wheelSettings.SingleSliceAngle - finishedAngle;

            WheelSingleton.Instance.WheelSoundController.PlayWheelSpinSound();
            wheelRotateRectTransform.DORotate(
                        new Vector3(0f, 0f, rotateAngle),
                        wheelSettings.SpinDuration,
                        RotateMode.FastBeyond360)
                    .SetEase(wheelSettings.SpinEase).onComplete =
                () =>
                {
                    CurrentReward = _currentSpinRewards[finishedSlice];
                    CurrentRewardValue = sliceItems[finishedSlice].Value;
                    SliceRewardIconRectTransform = sliceItems[finishedSlice].IconRectTransform;
                    IsWheelSpinning = false;
                };
        }
        
        public void SetExitButtonInteractable(bool value)
        {
            exitButton.interactable = value;
        }
        
        private void ExitBtnOnClick()
        {
            IsExitBtnClicked = true;
        }
        
        #endregion

        #region Bomb

        public void StartBombAnimation()
        {
            bombRectTransform.localScale = Vector3.zero;
            bombRectTransform.gameObject.SetActive(true);
            bombRectTransform.DOScale(Vector3.one * 30, 3f).onComplete = () => IsBombAnimationOver = true;
        }

        #endregion

        private void OnDestroy()
        {
            spinButton.onClick.RemoveListener(SpinBtnOnClick);
            exitButton.onClick.RemoveListener(ExitBtnOnClick);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            EditorApplication.delayCall += () =>
            {
                if (this == null) return;

                GameObject safeZoneInfoTmpGO = GameObject.Find(OnValidateGameObjectName.SafeZoneInfoTmp);
                if (safeZoneInfoTmpGO != null) safeZoneInfoTmp = safeZoneInfoTmpGO.GetComponent<TextMeshProUGUI>();
                else Debug.Log(OnValidateGameObjectName.SafeZoneInfoTmp + "couldn't be find active on the scene");
                
                GameObject superZoneInfoTmpGO = GameObject.Find(OnValidateGameObjectName.SuperZoneInfoTmp);
                if (superZoneInfoTmpGO != null) superZoneInfoTmp = superZoneInfoTmpGO.GetComponent<TextMeshProUGUI>();
                else Debug.Log(OnValidateGameObjectName.SuperZoneInfoTmp + "couldn't be find active on the scene");
                
                GameObject fadeInOutImageGO = GameObject.Find(OnValidateGameObjectName.FadeInOutImage);
                if (fadeInOutImageGO != null) fadeInOutImage = fadeInOutImageGO.GetComponent<Image>();
                else Debug.Log(OnValidateGameObjectName.FadeInOutImage + "couldn't be find active on the scene");
                
                GameObject wheelRotateRectTransformGO = GameObject.Find(OnValidateGameObjectName.WheelRotateRectTransform);
                if (wheelRotateRectTransformGO != null) wheelRotateRectTransform = wheelRotateRectTransformGO.GetComponent<RectTransform>();
                else Debug.Log(OnValidateGameObjectName.WheelRotateRectTransform + "couldn't be find active on the scene");
                
                GameObject spinButtonGO = GameObject.Find(OnValidateGameObjectName.WheelSpinBtn);
                if (spinButtonGO != null) spinButton = spinButtonGO.GetComponent<Button>();
                else Debug.Log(OnValidateGameObjectName.WheelSpinBtn + "couldn't be find active on the scene");
                
                GameObject exitButtonGO = GameObject.Find(OnValidateGameObjectName.WheelExitBtn);
                if (exitButtonGO != null) exitButton = exitButtonGO.GetComponent<Button>();
                else Debug.Log(OnValidateGameObjectName.WheelExitBtn + "couldn't be find active on the scene");
                
                EditorUtility.SetDirty(this);
            };
        }
#endif
    }
}