using System;

namespace Utilities
{
    public struct PoolToken<T> : IDisposable
    {
        private SimpleObjectPool<T> _pool;
        private T _value;
        
        public T Value => _value;
        
        public PoolToken(SimpleObjectPool<T> pool)
        {
            _pool = pool;
            _value = _pool.Get();
        }
        
        public void Dispose()
        {
            _pool.Return(_value);
            _value = default(T);
        }
        
        public static implicit operator T(PoolToken<T> token)
        {
            return token.Value;
        }
    }
}