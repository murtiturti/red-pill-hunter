using System;
using UnityEngine;

namespace Attacks
{
    public class Katana : MonoBehaviour, IWeapon
    {
        private static readonly int Attack1 = Animator.StringToHash("Swing");

        public int Attack()
        {
            return Attack1;
        }
    }
}
