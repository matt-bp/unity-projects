using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Controllers
{
    public class EndController : MonoBehaviour
    {
        public void OnClick()
        {
            SceneManager.LoadScene("Start");
        }
    }
}