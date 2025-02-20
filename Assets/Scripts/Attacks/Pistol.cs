using System;
using System.Collections;
using Enemy;
using GameEventsSystem;
using UnityEngine;
using Util;

namespace Attacks
{
    public class Pistol : MonoBehaviour, IWeapon
    {
        public float cooldown = 0.2f;
        private bool _canFire = true;
        public float range = 50f;
        public float impactForce = 20f;
        public LayerMask enemyLayer;
        public IntVariable AmmoCount;
        
        [SerializeField]
        private int _ammoCount = 0;
        private const int AmmoCapacity = 50;
        
        [SerializeField]
        private int _inClipCount = 1;
        private const int InClipCapacity = 10;
        
        private static readonly int Attack1 = Animator.StringToHash("Fired");
        
        private Camera _mainCamera;

        private void Start()
        {
            _mainCamera = Camera.main;
        }

        public int Attack()
        {
            if (_inClipCount > 0 && _canFire)
            {
                _inClipCount--;
                // Animation
                // Muzzle flash
                // Raycast, kill
                Debug.Log("Fired!");
                FireGun();
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
            if (_ammoCount >= InClipCapacity)
            {
                // if you have at least clip capacity ammo, reload full clip
                _ammoCount -= InClipCapacity;
                _inClipCount += InClipCapacity;
            }
            else
            {
                // else put all you have in clip, empty ammo bag
                _inClipCount += _ammoCount;
                _ammoCount = 0;
            }

            AmmoCount.Value = _ammoCount;
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

        private void FireGun()
        {
            var ray = _mainCamera.ScreenPointToRay(new Vector2(Screen.width / 2f, Screen.height / 2f));
            if (Physics.Raycast(ray, out var hit, range, enemyLayer))
            {
                Debug.Log("Hit something");
                var enemyAi = hit.collider.GetComponent<EnemyAI>();
                if (enemyAi != null)
                {
                    enemyAi.Die(ray.direction, impactForce);
                }
            }
        }

        private void OnEnable()
        {
            _ammoCount = Mathf.Clamp(AmmoCount.Value, 0, AmmoCapacity);
        }
    }
}
