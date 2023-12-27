using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Wright.Library.Managers;

namespace Managers
{
    public class LevelManager : MonoBehaviour, IGameManager
    {
        public ManagerStatus Status { get; private set; }
        public int CurrentLevel { get; private set; }
        private int LastLevel { get; set; }
        private string LevelName => $"Level_{CurrentLevel}";
        
        public void Startup()
        {
            Debug.Log("Level Manager starting...");
            
            // And we have this many levels to do
            LastLevel = 1;

            Status = ManagerStatus.Started;
        }

        public void StartLevels()
        {
            RestartAndGo();
        }

        public void GoToNextLevel()
        {
            if (CurrentLevel < LastLevel)
            {
                CurrentLevel++;
                Debug.Log($"Loading {LevelName}");
                SceneManager.LoadScene(LevelName);
            }
            else
            {
                SceneManager.LoadScene("End");
            }
        }

        public void RestartAndGo()
        {
            CurrentLevel = 0;
            GoToNextLevel();
        }
    }
}