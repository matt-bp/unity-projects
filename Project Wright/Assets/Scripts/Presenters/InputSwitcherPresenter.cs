using Events;
using TMPro;
using UnityEngine;
using Wright.Library.Messages;

namespace Presenters
{
    public class InputSwitcherPresenter : MonoBehaviour
    {
        [Header("View")]
        [SerializeField] private TMP_Text currentInput;
        
        private void OnEnable()
        {
            Debug.Log("ISP Enable");
            Messenger<string>.AddListener(ModelToPresenter.CURRENT_INPUT, OnCurrentInput);
        }

        private void OnDisable()
        {
            Debug.Log("ISP Disable");
            Messenger<string>.RemoveListener(ModelToPresenter.CURRENT_INPUT, OnCurrentInput);
        }
        
        private void OnCurrentInput(string input)
        {
            currentInput.text = $"Switch to: {input}";
        }
        
        public void OnInputSwitched()
        {
            Messenger.Broadcast(PresenterToModel.SWITCHED_INPUT);
        }
    }
}