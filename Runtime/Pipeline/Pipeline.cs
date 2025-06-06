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
        
        internal Entity[] m_sparseEntities;
        readonly AllocableSparseSet m_sparseSet;
        
        readonly Dictionary<BitMask, Filter> m_filtersMap;
        Filter[] m_filtersBuffer;
        int m_filtersCount;
        
        readonly List<ISystem> m_allSystems;
        
        IFixedUpdateSystem[] m_fixedUpdateSystems;
        IUpdateSystem[] m_updateSystems;
        ILateUpdateSystem[] m_lateUpdateSystems;
        IEntityInitializeSystem[] m_entityInitializeSystems;
        IEntityTerminateSystem[] m_entityTerminateSystems;
        
        ArrayList m_injectionsCache;

        public int EntityCount
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => m_sparseSet.m_count;
        }
        
        public bool AutoUpdateFilters
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]get;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]set;
        }
        
        public Pipeline()
        {
            m_sparseEntities = new Entity[64];
            m_allSystems = new List<ISystem>(128);
            m_injectionsCache = new ArrayList(32);
            m_sparseSet = new AllocableSparseSet();
            
            m_filtersMap = new Dictionary<BitMask, Filter>(64);
            m_filtersBuffer = new Filter[64];
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SparseEntitiesEnumerator GetEnumerator()
        {
            return new SparseEntitiesEnumerator(m_sparseEntities, m_sparseSet.m_dense, m_sparseSet.m_count);
        }
    };
}
