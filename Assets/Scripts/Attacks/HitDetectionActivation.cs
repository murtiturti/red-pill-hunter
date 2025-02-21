using System;
using UnityEngine;

namespace Attacks
{
    public class HitDetectionActivation : MonoBehaviour
    {
        public Katana katana;

        // This method is called on animation event during the SwordSwing animation
        public void Detect()
        {
            katana.HitCheck();
        }
    }
}
