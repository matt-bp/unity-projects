using System;
using UnityEngine;
using Wright.Library.Managers;

namespace Managers
{
    public class LevelManager : MonoBehaviour, IGameManager
    {
        public ManagerStatus Status { get; private set; }
        public int CurrentLevel { get; private set; }
        public int LastLevel { get; private set; }
        
        public void Startup()
        {
            Debug.Log("Level Manager starting...");
            
            CurrentLevel = 0;
            LastLevel = 0;

            Status = ManagerStatus.Started;
        }
    }
}