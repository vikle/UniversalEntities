namespace UniversalEntities
{
    public sealed class EventCollector<T> : IUpdateSystem where T : class, IEvent
    {
        public void OnUpdate(Context context)
        {
            
        }
    };
}
