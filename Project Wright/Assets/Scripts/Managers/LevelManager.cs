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
            
            // The game hasn't started yet
            CurrentLevel = 0;
            // And we have this many levels to do
            LastLevel = 2;

            Status = ManagerStatus.Started;
        }

        public void GoToNextLevel()
        {
            if (CurrentLevel <= LastLevel)
            {
                CurrentLevel++;
                Debug.Log($"Loading {LevelName}");
                SceneManager.LoadScene(LevelName);
            }
            else
            {
                Debug.Log("Last level! What do I do? ...");
            }
        }
    }
}