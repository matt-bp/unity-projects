namespace Managers
{
    public interface IGameManager
    {
        ManagerStatus Status { get; }

        void Startup();
    }
}