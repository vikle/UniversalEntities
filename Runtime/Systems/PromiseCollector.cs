using System.Collections.Generic;

namespace UniversalEntities
{
    public sealed class PromiseCollector<T> : IUpdateSystem where T : class, IPromise
    {
        public void OnUpdate(Context context)
        {
            
        }
        
        // protected void OnExecute(Context context, IEntity entity)
        // {
        //     var promise = new T();
        //     
        //     List<IEvent> resolve;
        //     var state = promise.State;
        //
        //     switch (state)
        //     {
        //         case EPromiseState.Fulfilled:
        //         case EPromiseState.Rejected: 
        //             resolve = promise.Resolve;
        //             break;
        //             
        //         default: return;
        //     }
        //         
        //     switch (state)
        //     {
        //         case EPromiseState.Fulfilled:
        //             var target = promise.Target;
        //             for (int i = 0, i_max = resolve.Count; i < i_max; i++)
        //             {
        //                 target.AddComponent(resolve[i]);
        //             }
        //             break;
        //         case EPromiseState.Rejected: 
        //             for (int i = 0, i_max = resolve.Count; i < i_max; i++)
        //             {
        //                 FragmentPool.Release(resolve[i]);
        //             }
        //             break; 
        //     }
        //
        //     resolve.Clear();
        //     
        //     promise.Target = null;
        //     promise.State = EPromiseState.Pending;
        //
        //     entity.RemoveComponent(promise);
        // }
    };
}
