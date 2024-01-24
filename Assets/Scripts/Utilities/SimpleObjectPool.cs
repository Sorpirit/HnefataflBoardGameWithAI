using System;
using System.Collections.Generic;

namespace Utilities
{
    public class SimpleObjectPool<T>
    {
        private readonly Stack<T> _objects;
        private readonly Func<T> _objectGenerator;
        
        public SimpleObjectPool(Func<T> objectGenerator, int initialCount = 0)
        {
            _objects = new Stack<T>();
            _objectGenerator = objectGenerator;
            
            for (int i = 0; i < initialCount; i++)
            {
                _objects.Push(_objectGenerator());
            }
        }
        
        internal T Get()
        {
            if (_objects.Count > 0)
                return _objects.Pop();
            
            return _objectGenerator();
        }
        
        public PoolToken<T> Borrow() => new(this);
        
        public void Return(T item)
        {
            _objects.Push(item);
        }
    }
}