namespace UniversalEntities
{
    public interface IContextRuntime
    {
        void Init();
        void OnStart();
        void OnFixedUpdate();
        void OnUpdate();
        void OnLateUpdate();
    };
}
