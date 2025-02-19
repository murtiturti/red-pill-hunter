using System;
using UnityEngine;

namespace GameEventsSystem
{
    public class GameEvents 
    {
        // Add events here as:
        // public static event Action/<T> OnEvent
        public static event Action<int> OnAmmoPickup;

        // Invoke methods go here
        public static void TriggerAmmoPickup(int ammo)
        {
            OnAmmoPickup?.Invoke(ammo);
        }
    }
}
