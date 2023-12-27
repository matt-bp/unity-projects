using Managers;
using UnityEngine;

namespace Controllers
{
    public class EndController : MonoBehaviour
    {
        public void OnClick()
        {
            StartedManagers.Level.RestartAndGo();
        }
    }
}