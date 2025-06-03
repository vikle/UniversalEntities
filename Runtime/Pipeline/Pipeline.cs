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
        
        readonly List<ISystem> m_allSystems;
        
        IFixedUpdateSystem[] m_fixedUpdateSystems;
        IUpdateSystem[] m_updateSystems;
        ILateUpdateSystem[] m_lateUpdateSystems;
        IEntityInitializeSystem[] m_entityInitializeSystems;
        IEntityTerminateSystem[] m_entityTerminateSystems;
        
        ArrayList m_injectionsCache;

        internal Pipeline()
        {
            m_sparseEntities = new Entity[64];
            m_allSystems = new List<ISystem>(128);
            m_injectionsCache = new ArrayList(32);
            m_sparseSet = new AllocableSparseSet();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SparseEntitiesEnumerator GetEnumerator()
        {
            return new SparseEntitiesEnumerator(m_sparseEntities, m_sparseSet.m_dense, m_sparseSet.m_count);
        }
    };
}
