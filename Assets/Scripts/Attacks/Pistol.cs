using System;
using System.Collections;
using UnityEngine;

namespace Attacks
{
    public class Pistol : MonoBehaviour, IWeapon
    {
        private int _ammoCount = 20;
        private const int _ammoCapacity = 50;
        private int _inClipCount = 10;
        private int _inClipCapacity = 10;

        public float cooldown = 0.2f;
        private bool _canFire = true;
        
        private static readonly int Attack1 = Animator.StringToHash("Fired");
        
        public int Attack()
        {
            if (_inClipCount > 0 && _canFire)
            {
                _inClipCount--;
                // Animation
                // Muzzle flash
                // Raycast, kill
                Debug.Log("Fired!");
                _canFire = false;
                Cooldown();
                return Attack1;
            }
            else if (_inClipCount == 0 && _canFire)
            {
                Reload();
                return 0;
            }

            return 0;
        }

        public void Reload()
        {
            if (_ammoCount >= _inClipCapacity)
            {
                // if you have at least clip capacity ammo, reload full clip
                _ammoCount -= _inClipCapacity;
                Debug.Log("Ammo Count: " + _ammoCount);
            }
            else
            {
                // else put all you have in clip, empty ammo bag
                _inClipCount = _ammoCount;
                _ammoCount = 0;
                Debug.Log("Ammo Count: " + _ammoCount);
            }
        }

        private void Cooldown()
        {
            StartCoroutine(CooldownTimer());
        }

        private IEnumerator CooldownTimer()
        {
            yield return new WaitForSeconds(cooldown);
            _canFire = true;
        }
    }
}
