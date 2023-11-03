using System.Collections;
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(PlayerManager))]
[RequireComponent(typeof(InventoryManager))]
public class Managers : MonoBehaviour
{
    public static PlayerManager Player { get; private set; }
    public static InventoryManager Inventory { get; private set; }
    public static MissionManager Mission { get; private set; }

    private List<IGameManager> _startSequence;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        
        Player = GetComponent<PlayerManager>();
        Inventory = GetComponent<InventoryManager>();
        Mission = GetComponent<MissionManager>();

        _startSequence = new List<IGameManager>
        {
            Player,
            Inventory,
            Mission
        };

        StartCoroutine(StartupManagers());
    }

    private IEnumerator StartupManagers()
    {
        var network = new NetworkService();
        
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