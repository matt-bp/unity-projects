namespace Wright.Library.Managers
{
    public interface IGameManager
    {
        ManagerStatus Status { get; }

        void Startup();
    }
}