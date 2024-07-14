namespace UniversalEntities
{
    public sealed class EventCollector<T> 
        : Processor<T>
        , IUpdateSystem where T : class, IEvent
    {
        protected override void OnExecute(IContext context, IEntity entity)
        {
            entity.Remove(m_data1);
        }
    };
}
