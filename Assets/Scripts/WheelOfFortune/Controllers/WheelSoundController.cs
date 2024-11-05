using System;
using UnityEngine;
using WheelOfFortune.Single;

namespace WheelOfFortune.Controllers
{
    public class WheelSoundController : MonoBehaviour
    {
        [SerializeField] private AudioSource wheelSpinSoundAudioSource;
        
        private void Start()
        {
            WheelSingleton.Instance.Signal.WheelSpinStart += PlayWheelSpinSound;
        }

        public void PlayWheelSpinSound()
        {
            wheelSpinSoundAudioSource.Play();
        }

        private void OnDestroy()
        {
            WheelSingleton.Instance.Signal.WheelSpinStart -= PlayWheelSpinSound;
        }
    }
}