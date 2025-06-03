using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UniversalEntities
{
    internal static class ArrayTool
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void Push<T>(ref T[] stack, ref int capacity, in T value)
        {
            EnsureCapacity(ref stack, capacity);
            stack[capacity++] = value;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool TryPop<T>(T[] stack, ref int capacity, out T value)
        {
            if (capacity > 0)
            {
                value = stack[--capacity];
                return true;
            }

            value = default;
            return false;
        }

        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void EnsureCapacity<T>(ref T[] array, int minCapacity)
        {
            int current_capacity = array.Length;

            if (current_capacity > minCapacity) return;
            
            while (current_capacity <= minCapacity)
            {
                current_capacity = (current_capacity << 1);
                Array.Resize(ref array, current_capacity);
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void WhereCast<T, TV>(T[] source, out TV[] result) 
            where T : class 
            where TV : class
        {
            var list = new List<TV>(source.Length);
            
            for (int i = 0, i_max = source.Length; i < i_max; i++)
            {
                if (source[i] is TV value)
                {
                    list.Add(value);
                }
            }
            
            result = list.ToArray();
        }
    };
}
