using System;
using System.Collections.Generic;

namespace UniversalEntities
{
    public abstract class ObjectPool<TType> where TType : class
    {
        static readonly Dictionary<Type, Stack<TType>> sr_pool = new Dictionary<Type, Stack<TType>>(64);
        
        protected static TValue GetInternal<TValue>() where TValue : class, TType, new()
        {
            var pool_type = typeof(TValue);

            TValue instance;
            
            if (sr_pool.TryGetValue(pool_type, out var stack))
            {
                instance = (stack.Count > 0) 
                    ? (TValue)stack.Pop() 
                    : new TValue();
            }
            else
            {
                instance = new TValue();
                sr_pool[pool_type] = new Stack<TType>();
            }
            
            return instance;
        }
        
        protected static void ReleaseInternal<TValue>(TValue instance) where TValue : class, TType
        {
            var pool_type = instance.GetType();

            if (sr_pool.TryGetValue(pool_type, out var stack) == false)
            {
                stack = new Stack<TType>();
                sr_pool[pool_type] = stack;
            }
            
            stack.Push(instance);
        }
    };
}
