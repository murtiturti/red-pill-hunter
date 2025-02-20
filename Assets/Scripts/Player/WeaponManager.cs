using System;
using System.Collections;
using System.Collections.Generic;
using Attacks;
using GameEventsSystem;
using UnityEngine;
using Util;

namespace Player
{
    public class WeaponManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] weapons;
        
        private int _equippedIndex = 0;
        private IWeapon _equippedWeapon;
        
        [SerializeField]
        private Animator armAnimator;

        private int _weaponAnimId;
        private static readonly int WeaponSwitchId = Animator.StringToHash("Switch");

        [Range(0f, 1f)]
        public float weaponSwitchThreshold = 0.7f;
        public IntVariable AmmoCount;

        private void Start()
        {
            _equippedWeapon = weapons[_equippedIndex].GetComponent<IWeapon>();
        }

        public void Attack()
        {
            var id = _equippedWeapon.Attack();
            if (id != 0)
            {
                armAnimator.SetTrigger(id);
                _weaponAnimId = id;
            }
        }

        public void InterruptAttack()
        {
            if (_weaponAnimId != 0)
            {
                armAnimator.ResetTrigger(_weaponAnimId);
            }
            
            armAnimator.SetTrigger(WeaponSwitchId);
        }

        public void SwitchWeapon(int index)
        {
            StartCoroutine(WaitForSwitchThreshold(index));
        }
        
        private IEnumerator WaitForSwitchThreshold(int index)
        {
            while (true)
            {
                var stateInfo = armAnimator.GetCurrentAnimatorStateInfo(0);
                if (stateInfo.IsName("WeaponSwitch"))
                {
                    if (stateInfo.normalizedTime >= weaponSwitchThreshold)
                    {
                        weapons[_equippedIndex].SetActive(false);
                        weapons[index].SetActive(true);
                        _equippedWeapon = weapons[index].GetComponent<IWeapon>();
                        _equippedIndex = index;
                        yield break;
                    }
                }
                yield return null;
            }
        }

        private void AddAmmo(int ammo)
        {
            AmmoCount.Value += ammo;
        }
        
        private void OnEnable()
        {
            GameEvents.OnAmmoPickup += AddAmmo;
        }

        private void OnDisable()
        {
            GameEvents.OnAmmoPickup -= AddAmmo;
        }
    }
}
