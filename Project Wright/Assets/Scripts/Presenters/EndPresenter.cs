using Events;
using UnityEngine;
using Wright.Library.Messages;

namespace Presenters
{
    public class EndPresenter : MonoBehaviour
    {
        public void OnExitClicked()
        {
            Messenger.Broadcast(PresenterToModel.EXITING_GAME);
            
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}