namespace UniversalEntities
{
    public sealed class EventCollector<T> 
        : Processor<T>
        , IUpdateSystem where T : class, IEvent
    {
        protected override void OnExecute(Context context, IEntity entity)
        {
            entity.RemoveComponent(m_data1);
        }
    };
}
