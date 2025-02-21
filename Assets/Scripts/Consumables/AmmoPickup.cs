using System;
using GameEventsSystem;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Consumables
{
    public class AmmoPickup : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            
            if (other.gameObject.CompareTag("Player"))
            {
                GameEvents.TriggerAmmoPickup(Random.Range(3, 8));
                Destroy(gameObject);
            }
        }
    }
}
