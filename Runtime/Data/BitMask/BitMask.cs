using System;
using System.Runtime.CompilerServices;

namespace UniversalEntities
{
    public struct BitMask : IEquatable<BitMask>
    {
        ulong m_value;

        public bool IsEmpty
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (m_value == 0ul);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Set(int value)
        {
            m_value |= (1ul << value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Unset(int value)
        {
            m_value &= ~(1ul << value);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Has(int value)
        {
            return ((m_value & (1ul << value)) != 0ul);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Has(in BitMask other)
        {
            ulong rhs = other.m_value;
            return ((m_value & rhs) == rhs);
        }
        
        public override bool Equals(object obj)
        {
            return ((obj is BitMask other) && IsEquals(in this, in other));
        }
        
        public bool Equals(BitMask other)
        {
            return IsEquals(in this, in other);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(BitMask a, BitMask b)
        {
            return IsEquals(in a, in b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(BitMask a, BitMask b)
        {
            return !IsEquals(in a, in b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsEquals(in BitMask a, in BitMask b)
        {
            return (a.m_value == b.m_value);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            return m_value.GetHashCode();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
        {
            return m_value.ToString();
        }
    };
}
