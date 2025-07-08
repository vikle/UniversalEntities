using System.Runtime.CompilerServices;

namespace UniversalEntities
{
    public static class PipelineSingleton
    {
        static Pipeline s_pipeline;

        public static Pipeline Get
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]get
            {
                if (s_pipeline == null)
                {
                    s_pipeline = new Pipeline();
                }
                
                return s_pipeline;
            }
        }

        internal static void Dispose()
        {
            s_pipeline = null;
        }
    };
}
