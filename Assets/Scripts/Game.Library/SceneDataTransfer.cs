using System;
using System.Collections.Generic;

namespace Game.Library
{
    public class SceneDataTransferComponent : SingleBehaviour<SceneDataTransferComponent>
    {
        private Dictionary<Type, object> _data = new();
        private HashSet<Type> _tags = new();
        
        protected override void OnAwake()
        {
            DontDestroyOnLoad(gameObject);
        }
        
        public void AddTag<T>()
        {
            _tags.Add(typeof(T));
        }
        
        public bool HasTag<T>()
        {
            return _tags.Contains(typeof(T));
        }
        
        public bool Contains<T>()
        {
            return _data.ContainsKey(typeof(T));
        }
        
        public bool TryReadSingle<T>(out T data)
        {
            if (_data.TryGetValue(typeof(T), out var value))
            {
                data = (T) value;
                return true;
            }

            data = default;
            return false;
        }
        
        public T ReadSingle<T>()
        {
            return (T) _data[typeof(T)];
        }
        
        public void WriteSingle<T>(T data)
        {
            _data[typeof(T)] = data;
        }
        
        private void RemoveSingle<T>()
        {
            _data.Remove(typeof(T));
        }
        
        public void Clear()
        {
            _data.Clear();
            _tags.Clear();
        }
    }
}