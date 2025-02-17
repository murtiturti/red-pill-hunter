using System;
using Attacks;
using UnityEngine;

namespace Player
{
    public class WeaponManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] weapons;
        
        private int _equippedIndex = 0;
        private IWeapon _equippedWeapon;

        private void Start()
        {
            _equippedWeapon = weapons[_equippedIndex].GetComponent<IWeapon>();
        }

        public void Attack()
        {
            _equippedWeapon.Attack();
        }

        public void SwitchWeapon(int index)
        {
            weapons[_equippedIndex].SetActive(false);
            weapons[index].SetActive(true);
            _equippedWeapon = weapons[index].GetComponent<IWeapon>();
            _equippedIndex = index;
        }
    }
}
