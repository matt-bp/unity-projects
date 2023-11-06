using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Managers
{
    [RequireComponent(typeof(PidManager))]
    public class LoadedManagers : MonoBehaviour
    {
        public static PidManager Pid { get; private set; }

        private List<IGameManager> _startSequence;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            Pid = GetComponent<PidManager>();

            _startSequence = new List<IGameManager>
            {
                Pid,
            };

            StartCoroutine(StartupManagers());
        }

        private IEnumerator StartupManagers()
        {
            // This might be a good example of how to do "asynchronous" things in Unity. Make the call, and yield return immediately after.
            // Then, check for some status to make sure it's done.
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