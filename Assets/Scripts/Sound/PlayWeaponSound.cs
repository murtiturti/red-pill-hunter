using System;
using UnityEngine;

namespace Sound
{
    public class PlayWeaponSound : MonoBehaviour
    {
        private AudioSource _audioSource;
        public AudioClip gunClip, katanaClip, reloadClip;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void PlayGunshot()
        {
            _audioSource.PlayOneShot(gunClip);
        }

        public void PlayKatana()
        {
            _audioSource.PlayOneShot(katanaClip);
        }

        public void PlayReload()
        {
            _audioSource.PlayOneShot(reloadClip);
        }
    }
}
