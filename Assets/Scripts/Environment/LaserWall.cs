using System;
using GameEventsSystem;
using UnityEngine;

namespace Environment
{
    public class LaserWall : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                GameEvents.TriggerPlayerDeath();
            }
        }
    }
}
