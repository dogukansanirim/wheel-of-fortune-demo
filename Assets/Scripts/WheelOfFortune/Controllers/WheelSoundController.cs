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
            WheelSingleton.Instance.WheelSoundController = this;
        }

        public void PlayWheelSpinSound()
        {
            wheelSpinSoundAudioSource.Play();
        }
    }
}