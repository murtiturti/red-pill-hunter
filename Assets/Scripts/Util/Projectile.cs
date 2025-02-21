using System;
using GameEventsSystem;
using UnityEngine;

namespace Util
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody rb;
        [SerializeField]
        private float speed;

        public void Shoot(Vector3 direction)
        {
            rb.linearVelocity = direction * speed;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                GameEvents.TriggerPlayerDeath();
                Debug.Log("Hit player");
            }
            Destroy(gameObject);
        }
    }
}
