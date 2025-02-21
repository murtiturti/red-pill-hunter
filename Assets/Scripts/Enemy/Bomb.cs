using System;
using GameEventsSystem;
using UnityEngine;

namespace Enemy
{
    public class Bomb : MonoBehaviour
    {
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                GameEvents.TriggerPlayerDeath();
            }
            Destroy(gameObject);
        }
    }
}
