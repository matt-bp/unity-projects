using System;
using Events;
using UnityEngine;
using Wright.Library.Messages;

namespace Presenters
{
    public class LevelPresenter : MonoBehaviour
    {
        private void OnEnable()
        {
            Debug.Log("LP Enable");
            Messenger<string>.AddListener(ModelToPresenter.CURRENT_INPUT, OnUsingInput);
        }

        private void OnDisable()
        {
            Debug.Log("LP Disable");
            Messenger<string>.RemoveListener(ModelToPresenter.CURRENT_INPUT, OnUsingInput);
        }

        private void OnUsingInput(string input)
        {
            Debug.Log($"Using input {input} for the task.");
        }
    }
}