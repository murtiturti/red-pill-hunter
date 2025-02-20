using UnityEngine;

namespace Util
{
    [CreateAssetMenu(fileName = "IntVariable", menuName = "Scriptable Objects/IntVariable")]
    public class IntVariable : ScriptableObject
    {
        public int Value;
    }
}
