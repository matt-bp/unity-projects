using System;
using Events;
using Models;
using TMPro;
using UnityEngine;
using Wright.Library.Messages;

namespace Presenters
{
    public class LevelPresenter : MonoBehaviour
    {
        [Header("View")]
        [SerializeField] private TMP_Text timeLabel;
        
        private void OnEnable()
        {
            Debug.Log("LP Enable");
            Messenger<string>.AddListener(ModelToPresenter.CURRENT_INPUT, OnUsingInput);
            Messenger<double>.AddListener(ModelToPresenter.TIME_UPDATE, OnTimeUpdate);
        }

        private void OnDisable()
        {
            Debug.Log("LP Disable");
            Messenger<string>.RemoveListener(ModelToPresenter.CURRENT_INPUT, OnUsingInput);
            Messenger<double>.AddListener(ModelToPresenter.TIME_UPDATE, OnTimeUpdate);
        }

        private void OnUsingInput(string input)
        {
            Debug.Log($"Using input {input} for the task.");
            
            LoadedModels.Measurement.ResetTime();
        }

        private void OnTimeUpdate(double dt)
        {
            timeLabel.text = $"Time is now: {TimeSpan.FromSeconds(dt):mm\\:ss}";
        }

        private void Update()
        {
            LoadedModels.Measurement.IncrementTime();
        }

        public void OnSubmitClicked()
        {
            Messenger.Broadcast(PresenterToModel.SUBMITTED);
        }
    }
}