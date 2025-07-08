using System.Runtime.CompilerServices;

namespace UniversalEntities
{
    public static class PipelineSingleton
    {
        public static bool IsAlive
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]get;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]private set;
        }

        public static Pipeline Get
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]get;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]private set;
        }

        internal static void Initialize()
        {
            IsAlive = true;
            Get = new Pipeline();
        }

        internal static void Dispose()
        {
            IsAlive = false;
            Get = null;
        }
    };
}
