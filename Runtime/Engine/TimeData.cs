using System.Runtime.CompilerServices;
using UTime = UnityEngine.Time;

namespace UniversalEntities
{
    public static class TimeData
    {
        public static float Time
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]get; 
            [MethodImpl(MethodImplOptions.AggressiveInlining)]private set;
        }
        public static float UnscaledTime
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]get; 
            [MethodImpl(MethodImplOptions.AggressiveInlining)]private set;
        }
        public static float DeltaTime 
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]get; 
            [MethodImpl(MethodImplOptions.AggressiveInlining)]private set;
        }
        public static float UnscaledDeltaTime 
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]get; 
            [MethodImpl(MethodImplOptions.AggressiveInlining)]private set;
        }  
        
        public static float FixedTime
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]get; 
            [MethodImpl(MethodImplOptions.AggressiveInlining)]private set;
        }
        public static float FixedUnscaledTime
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]get; 
            [MethodImpl(MethodImplOptions.AggressiveInlining)]private set;
        }
        public static float FixedDeltaTime 
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]get; 
            [MethodImpl(MethodImplOptions.AggressiveInlining)]private set;
        }
        public static float FixedUnscaledDeltaTime 
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]get; 
            [MethodImpl(MethodImplOptions.AggressiveInlining)]private set;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void OnUpdate()
        {
            Time = UTime.time;
            UnscaledTime = UTime.unscaledTime;
            DeltaTime = UTime.deltaTime;
            UnscaledDeltaTime = UTime.unscaledDeltaTime;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void OnFixedUpdate()
        {
            FixedTime = UTime.fixedTime;
            FixedUnscaledTime = UTime.fixedUnscaledTime;
            FixedDeltaTime = UTime.fixedDeltaTime;
            FixedUnscaledDeltaTime = UTime.fixedUnscaledDeltaTime;
        }
    };
}
