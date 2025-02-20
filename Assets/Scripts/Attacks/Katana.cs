using System;
using Enemy;
using UnityEngine;

namespace Attacks
{
    public class Katana : MonoBehaviour, IWeapon
    {
        private static readonly int Attack1 = Animator.StringToHash("Swing");

        public LayerMask collisionMask;
        public Transform hitPoint;
        public float hitRadius = 1.5f;

        public int Attack()
        {
            return Attack1;
        }

        public void HitCheck()
        {
            var colliders = Physics.OverlapSphere(hitPoint.position, hitRadius, collisionMask);
            foreach (var collider in colliders)
            {
                var enemyController = collider.GetComponent<EnemyAI>();
                if (enemyController != null)
                {
                    enemyController.Split();
                }
            }
        }
    }
}
