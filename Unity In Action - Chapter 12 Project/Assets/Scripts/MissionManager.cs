using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionManager : MonoBehaviour, IGameManager
{
    public ManagerStatus Status { get; private set; }
    public int CurrentLevel { get; private set; }
    public int MaxLevel { get; private set; }

    private NetworkService network;

    public void Startup(NetworkService service)
    {
        Debug.Log("Mission manager starting...");

        network = service;

        CurrentLevel = 0;
        MaxLevel = 1;

        Status = ManagerStatus.Started;
    }

    public void GoToNextLevel()
    {
        if (CurrentLevel < MaxLevel)
        {
            CurrentLevel++;
            var name = $"Level_{CurrentLevel}";
            Debug.Log($"Loading {name}");
            SceneManager.LoadScene(name);
        }
        else
        {
            Debug.Log("Last level. No more!");
        }
    }
}