using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

#if ENABLE_IL2CPP
using Unity.IL2CPP.CompilerServices;
#endif

namespace UniversalEntities
{
#if ENABLE_IL2CPP
    [Il2CppEagerStaticClassConstruction]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
#endif
    public sealed partial class Pipeline
    {
        public static Pipeline Instance { get; }

        static Pipeline() { Instance = new Pipeline(); }
        
        bool[] m_sparseEntities;
        
        Entity[] m_denseEntities;
        int m_denseCount;
        
        readonly List<ISystem> m_allSystems;
        
        IFixedUpdateSystem[] m_fixedUpdateSystems;
        IUpdateSystem[] m_updateSystems;
        ILateUpdateSystem[] m_lateUpdateSystems;
        IEntityInitializeSystem[] m_entityInitializeSystems;
        IEntityTerminateSystem[] m_entityTerminateSystems;
        
        ArrayList m_injectionsCache;

        internal Pipeline()
        {
            m_sparseEntities = new bool[128];
            m_denseEntities = new Entity[32];
            m_allSystems = new List<ISystem>(128);
            m_injectionsCache = new ArrayList(16);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PipelineEnumerator GetEnumerator()
        {
            return new PipelineEnumerator(m_denseEntities, m_denseCount);
        }
    };
}
