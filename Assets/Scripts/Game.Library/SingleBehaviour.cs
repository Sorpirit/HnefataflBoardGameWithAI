using System;
using UnityEngine;

namespace Game.Library
{
    public abstract class SingleBehaviour<T> : MonoBehaviour
    {
        public static T Instance { get; private set; }
        
        protected virtual void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = (T) (object) this;
            OnAwake();
        }

        private void OnDestroy()
        {
            if (Equals(Instance, this))
            {
                Instance = default;
            }
            
            OnDestroyInternal();
        }

        protected virtual void OnAwake() { }

        protected virtual void OnDestroyInternal() { }
    }
}