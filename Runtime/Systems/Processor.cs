using System.Runtime.CompilerServices;

#if ENABLE_IL2CPP
    using Unity.IL2CPP.CompilerServices;
#endif

namespace UniversalEntities
{
#if ENABLE_IL2CPP
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
#endif
    public abstract class Processor<T> 
        where T : class, IFragment
    {
        protected T m_data1;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void OnFixedUpdate(Context context)
        {
            UpdateInternal(context);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void OnUpdate(Context context)
        {
            UpdateInternal(context);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void OnLateUpdate(Context context)
        {
            UpdateInternal(context);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateInternal(Context context)
        {
            foreach (var entity in context)
            {
                if (TryGetFragment(entity))
                {
                    OnExecute(context, entity);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected virtual bool TryGetFragment(IEntity entity)
        {
            return entity.TryGetComponent(out m_data1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected abstract void OnExecute(Context context, IEntity entity);
    };
    
        public abstract class Processor<T1, T2> : Processor<T1>
        where T1 : class, IFragment
        where T2 : class, IFragment
    {
        protected T2 m_data2;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override bool TryGetFragment(IEntity entity)
        {
            return base.TryGetFragment(entity) 
                && entity.TryGetComponent(out m_data2);
        }
    };
    
    public abstract class Processor<T1, T2, T3> : Processor<T1, T2>
        where T1 : class, IFragment
        where T2 : class, IFragment
        where T3 : class, IFragment
    {
        protected T3 m_data3;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override bool TryGetFragment(IEntity entity)
        {
            return base.TryGetFragment(entity) 
                && entity.TryGetComponent(out m_data3);
        }
    };
    
    public abstract class Processor<T1, T2, T3, T4> : Processor<T1, T2, T3>
        where T1 : class, IFragment
        where T2 : class, IFragment
        where T3 : class, IFragment
        where T4 : class, IFragment
    {
        protected T4 m_data4;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override bool TryGetFragment(IEntity entity)
        {
            return base.TryGetFragment(entity)
                && entity.TryGetComponent(out m_data4);
        }
    };
    
    public abstract class Processor<T1, T2, T3, T4, T5> : Processor<T1, T2, T3, T4>
        where T1 : class, IFragment
        where T2 : class, IFragment
        where T3 : class, IFragment
        where T4 : class, IFragment
        where T5 : class, IFragment
    {
        protected T5 m_data5;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override bool TryGetFragment(IEntity entity)
        {
            return base.TryGetFragment(entity) 
                && entity.TryGetComponent(out m_data5);
        }
    };
    
    public abstract class Processor<T1, T2, T3, T4, T5, T6> : Processor<T1, T2, T3, T4, T5>
        where T1 : class, IFragment
        where T2 : class, IFragment
        where T3 : class, IFragment
        where T4 : class, IFragment
        where T5 : class, IFragment
        where T6 : class, IFragment
    {
        protected T6 m_data6;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override bool TryGetFragment(IEntity entity)
        {
            return base.TryGetFragment(entity) 
                && entity.TryGetComponent(out m_data6);
        }
    };
    
    public abstract class Processor<T1, T2, T3, T4, T5, T6, T7> : Processor<T1, T2, T3, T4, T5, T6>
        where T1 : class, IFragment
        where T2 : class, IFragment
        where T3 : class, IFragment
        where T4 : class, IFragment
        where T5 : class, IFragment
        where T6 : class, IFragment
        where T7 : class, IFragment
    {
        protected T7 m_data7;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override bool TryGetFragment(IEntity entity)
        {
            return base.TryGetFragment(entity) 
                && entity.TryGetComponent(out m_data7);
        }
    };
    
    public abstract class Processor<T1, T2, T3, T4, T5, T6, T7, T8> : Processor<T1, T2, T3, T4, T5, T6, T7>
        where T1 : class, IFragment
        where T2 : class, IFragment
        where T3 : class, IFragment
        where T4 : class, IFragment
        where T5 : class, IFragment
        where T6 : class, IFragment
        where T7 : class, IFragment
        where T8 : class, IFragment
    {
        protected T8 m_data8;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override bool TryGetFragment(IEntity entity)
        {
            return base.TryGetFragment(entity) 
                && entity.TryGetComponent(out m_data8);
        }
    };
}
