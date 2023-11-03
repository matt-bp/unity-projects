using System.Collections;
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(PlayerManager))]
[RequireComponent(typeof(InventoryManager))]
[RequireComponent(typeof(MissionManager))]
[RequireComponent(typeof(DataManager))]
public class Managers : MonoBehaviour
{
    public static PlayerManager Player { get; private set; }
    public static InventoryManager Inventory { get; private set; }
    public static MissionManager Mission { get; private set; }
    public static DataManager Data { get; private set; }

    private List<IGameManager> _startSequence;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        
        Player = GetComponent<PlayerManager>();
        Inventory = GetComponent<InventoryManager>();
        Mission = GetComponent<MissionManager>();
        Data = GetComponent<DataManager>();

        _startSequence = new List<IGameManager>
        {
            Player,
            Inventory,
            Mission,
            Data
        };

        StartCoroutine(StartupManagers());
    }

    private IEnumerator StartupManagers()
    {
        var network = new NetworkService();
        
        // This might be a good example of how to do "asynchronous" things in Unity. Make the call, and yield return immediately after.
        // Then, check for some status to make sure it's done.
        foreach (var manager in _startSequence)
        {
            manager.Startup(network);
        }

        yield return null;

        var numModules = _startSequence.Count;
        var numReady = 0;

        while (numReady < numModules)
        {
            var lastReady = numReady;
            numReady = 0;

            foreach (var manager in _startSequence)
            {
                if (manager.Status == ManagerStatus.Started)
                {
                    numReady++;
                }
            }

            if (numReady > lastReady)
            {
                Debug.Log($"Progress: {numReady}/{numModules}");
                Messenger<int, int>.Broadcast(StartupEvent.ManagersProgress, numReady, numModules);
            }

            // Pause for one frame before checking again
            yield return null;
        }
        
        Debug.Log("All managers started up");
        Messenger.Broadcast(StartupEvent.ManagersStarted);
    }
}