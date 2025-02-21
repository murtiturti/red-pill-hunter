using System;
using GameEventsSystem;
using UnityEngine;

namespace Consumables
{
    public class AmmoPickup : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            
            if (other.gameObject.CompareTag("Player"))
            {
                GameEvents.TriggerAmmoPickup(10);
                Destroy(gameObject);
            }
        }
    }
}
