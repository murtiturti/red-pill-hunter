using UnityEngine;

namespace Attacks
{
    public class Katana : MonoBehaviour, IWeapon
    {
        public void Attack()
        {
            Debug.Log("Swoosh");
        }
    }
}
