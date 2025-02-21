using UnityEngine;

namespace Util
{
    public class DestroyAfterSeconds : MonoBehaviour
    {
        public float seconds;
        
        void Start()
        {
            Destroy(gameObject, seconds);
        }
        
    }
}
