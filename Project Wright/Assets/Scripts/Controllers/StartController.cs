using Managers;
using UnityEngine;

namespace Controllers
{
    public class StartController : MonoBehaviour
    {
        public void OnStartClick()
        {
            StartedManagers.Level.StartLevels();
        }
    }
}