using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class DataManager : MonoBehaviour, IGameManager
{
    public ManagerStatus Status { get; private set; }
    private NetworkService network;
    private string filename;

    public void Startup(NetworkService service)
    {
        Debug.Log("Data manager starting...");

        network = service;

        filename = Path.Combine(Application.persistentDataPath, "game.dat");

        Status = ManagerStatus.Started;
    }

    public void SaveGameState()
    {
        var gameState = new Dictionary<string, object>();

        gameState.Add("inventory", Managers.Inventory.GetData());
        gameState.Add("health", Managers.Player.Health);
        gameState.Add("maxHealth", Managers.Player.MaxHealth);
        gameState.Add("currentLevel", Managers.Mission.CurrentLevel);
        gameState.Add("maxLevel", Managers.Mission.MaxLevel);

        using var stream = File.Create(filename);
        var formatter = new BinaryFormatter();
        formatter.Serialize(stream, gameState);
    }

    public void LoadGameState()
    {
        if (!File.Exists(filename))
        {
            Debug.Log("No saved game");
            return;
        }

        Dictionary<string, object> gameState;

        using var stream = File.Open(filename, FileMode.Open);
        var formatter = new BinaryFormatter();
        gameState = formatter.Deserialize(stream) as Dictionary<string, object>;

        Managers.Inventory.UpdateData((Dictionary<string, int>)gameState["inventory"]);
        Managers.Player.UpdateData((int)gameState["health"], (int)gameState["maxHealth"]);
        Managers.Mission.UpdateData((int)gameState["curLevel"], (int)gameState["maxLevel"]);
        Managers.Mission.RestartCurrentLevel();
    }
}