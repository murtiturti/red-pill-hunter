using System;
using System.Collections;
using Enemy;
using GameEventsSystem;
using Sound;
using UnityEngine;
using Util;

namespace Attacks
{
    public class Pistol : MonoBehaviour, IWeapon
    {
        private bool _canFire;
        
        [Header("Gun Settings")]
        public float range = 50f;
        public float impactForce = 20f;
        public LayerMask enemyLayer;
        public IntVariable AmmoCount;
        public IntVariable inClipCountVariable;
        
        [SerializeField]
        private int _inClipCount = 10;
        private const int InClipCapacity = 10;
        
        private static readonly int Attack1 = Animator.StringToHash("Fired");
        
        private Camera _mainCamera;
        private Cooldown _cooldownComponent;
        
        [SerializeField]
        private PlayWeaponSound soundComponent;

        private void Start()
        {
            _mainCamera = Camera.main;
            _cooldownComponent = GetComponent<Cooldown>();
            _cooldownComponent.OnCooldownOver += OnCooldownEvent;
            _canFire = true;
            _inClipCount = 10;
            inClipCountVariable.Value = _inClipCount;

        }

        public int Attack()
        {
            Debug.Log(_canFire);
            if (_inClipCount > 0 && _canFire)
            {
                _inClipCount--;
                inClipCountVariable.Value = _inClipCount;
                FireGun();
                _canFire = false;
                _cooldownComponent.StartCooldown();
                return Attack1;
            }
            if (_inClipCount == 0 && _canFire)
            {
                Reload();
                inClipCountVariable.Value = _inClipCount;
                return 0;
            }
            return 0;
        }

        public void Reload()
        {
            soundComponent.PlayReload();
            if (AmmoCount.Value >= InClipCapacity)
            {
                // if you have at least clip capacity ammo, reload full clip
                AmmoCount.Value -= InClipCapacity;
                _inClipCount += InClipCapacity;
            }
            else
            {
                // else put all you have in clip, empty ammo bag
                _inClipCount += AmmoCount.Value;
                AmmoCount.Value = 0;
            }
        }

        private void OnCooldownEvent()
        {
            _canFire = true;
        }

        private void FireGun()
        {
            var ray = _mainCamera.ScreenPointToRay(new Vector2(Screen.width / 2f, Screen.height / 2f));
            if (Physics.Raycast(ray, out var hit, range, enemyLayer))
            {
                var enemyAi = hit.collider.GetComponent<EnemyAI>();
                if (enemyAi != null)
                {
                    enemyAi.Die(ray.direction, impactForce);
                }
            }
        }

        private void OnEnable()
        {
            AmmoCount.Value = Mathf.Clamp(AmmoCount.Value, 0, 50);
        }
    }
}
