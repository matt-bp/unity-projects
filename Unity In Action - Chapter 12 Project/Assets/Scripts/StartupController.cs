using System;
using UnityEngine;
using UnityEngine.UI;

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
        Managers.Mission.GoToNextLevel();
    }
}