using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionManager : MonoBehaviour, IGameManager
{
    public ManagerStatus Status { get; private set; }
    public int CurrentLevel { get; private set; }
    public int MaxLevel { get; private set; }
    private string LevelName => $"Level_{CurrentLevel}";

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
            Debug.Log($"Loading {LevelName}");
            SceneManager.LoadScene(LevelName);
        }
        else
        {
            Debug.Log("Last level. No more!");
        }
    }

    public void ReachObjective()
    {
        Messenger.Broadcast(GameEvent.LevelComplete);
    }

    public void RestartCurrentLevel()
    {
        Debug.Log($"Restarting {LevelName}");

        SceneManager.LoadScene(LevelName);
    }
}