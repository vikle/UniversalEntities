using System;
using System.Collections.Generic;

namespace UniversalEntities
{
    public abstract class ObjectPool<TType> where TType : class
    {
        static readonly Dictionary<Type, Stack<TType>> sr_pool = new(64);
        
        protected static TValue GetInstanceInternal<TValue>() where TValue : class, TType, new()
        {
            var pool_type = typeof(TValue);

            TValue instance;
            
            if (sr_pool.TryGetValue(pool_type, out var stack))
            {
                instance = (stack.Count > 0) 
                    ? (TValue)stack.Pop() 
                    : new();
            }
            else
            {
                instance = new();
                sr_pool[pool_type] = new();
            }
            
            return instance;
        }
        
        protected static void ReleaseInternal<TValue>(TValue instance) where TValue : class, TType
        {
            var pool_type = instance.GetType();

            if (sr_pool.TryGetValue(pool_type, out var stack) == false)
            {
                stack = new();
                sr_pool[pool_type] = stack;
            }
            
            stack.Push(instance);
        }
    };
}
