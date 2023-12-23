using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wright.Library.Managers;
using Wright.Library.Messages;

namespace Managers
{
    [RequireComponent(typeof(LevelManager))]
    public class StartedManagers : MonoBehaviour
    {
        public static LevelManager Level { get; private set; }
        private List<IGameManager> _startSequence;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            Level = GetComponent<LevelManager>();
            
            _startSequence = new List<IGameManager>
            {
                Level
            };

            StartCoroutine(StartupManagers());
        }

        private IEnumerator StartupManagers()
        {
            foreach (var manager in _startSequence)
            {
                manager.Startup();
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
}

