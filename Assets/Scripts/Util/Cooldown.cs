using System;
using System.Collections;
using UnityEngine;

namespace Util
{
    public class Cooldown : MonoBehaviour
    {
        public float timeInSeconds;
        public event Action OnCooldownOver;

        public void StartCooldown()
        {
            StartCoroutine(CoolDown());
        }

        private IEnumerator CoolDown()
        {
            yield return new WaitForSeconds(timeInSeconds);
            OnCooldownOver?.Invoke();
        }
    }
}
