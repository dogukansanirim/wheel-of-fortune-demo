using System;
using System.Collections;
using UnityEngine;
using WheelOfFortune.Controllers;

namespace WheelOfFortune.Single
{
    public class WheelSingleton : MonoBehaviour
    {
        public static WheelSingleton Instance { get; private set; }

        public WheelSingleton()
        {
            Instance ??= this;
        }
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }

            Instance = this;
        }

        public void SetTimout(Action action, float sec)
        {
            StartCoroutine(Timeout(action, sec));
        }
        
        private IEnumerator Timeout(Action callBack, float sec)
        {
            yield return new WaitForSeconds(sec);
            callBack.Invoke();
        }

        public WheelSignal Signal { get; } = new ();

        private RewardController _rewardController;

        public RewardController RewardController
        {
            get => _rewardController;

            set
            {
                if (_rewardController != null) return;
                _rewardController = value;
            }
        }
        
        private WheelUIController _wheelUIController;

        public WheelUIController WheelUIController
        {
            get => _wheelUIController;

            set
            {
                if (_wheelUIController != null) return;
                _wheelUIController = value;
            }
        }
        
        private WheelSoundController _wheelSoundController;

        public WheelSoundController WheelSoundController
        {
            get => _wheelSoundController;

            set
            {
                if (_wheelSoundController != null) return;
                _wheelSoundController = value;
            }
        }
    }
}