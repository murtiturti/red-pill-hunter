using System;
using System.Collections.Generic;
using UnityEngine;

namespace Util
{
    public abstract class Variable<T> : ScriptableObject
    {
        [SerializeField]
        private T value;
        
        public event Action<T> OnValueChanged;

        public T Value
        {
            get { return value; }
            set
            {
                if (!EqualityComparer<T>.Default.Equals(this.value, value))
                {
                    this.value = value;
                    OnValueChanged?.Invoke(this.value);
                }
            }
        }
    }
}
