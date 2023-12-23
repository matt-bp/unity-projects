using Managers;
using UnityEngine;
using UnityEngine.UI;
using Wright.Library.Messages;

namespace Controllers
{
    public class StartupController : MonoBehaviour
    {
        [SerializeField] private Slider progressBar;

        private void OnEnable()
        {
            Messenger<int, int>.AddListener(StartupEvent.ManagersProgress, OnManagersProgress);
            Messenger.AddListener(StartupEvent.ManagersStarted, OnManagersStarted);
        }

        private void OnDisable()
        {
            Messenger<int, int>.RemoveListener(StartupEvent.ManagersProgress, OnManagersProgress);
            Messenger.RemoveListener(StartupEvent.ManagersStarted, OnManagersStarted);
        }

        private void OnManagersProgress(int numReady, int numModules)
        {
            var progress = (float)numReady / numModules;
            progressBar.value = progress;
        }

        private void OnManagersStarted()
        {
            // LoadedManagers.Level.GoToNextLevel();
            Debug.Log("Go to next level!");
        }
    }
}