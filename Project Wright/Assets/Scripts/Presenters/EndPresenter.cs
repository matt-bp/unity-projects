using UnityEngine;

namespace Presenters
{
    public class EndPresenter : MonoBehaviour
    {
        public void OnExitClicked()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}