namespace UniversalEntities
{
    public interface IContextRuntime
    {
        void Init();
        void OnStart();
        void OnUpdate();
    };
}
