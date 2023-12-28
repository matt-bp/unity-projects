using System.Collections;
using Events;
using UnityEngine;
using UnityEngine.SceneManagement;
using Wright.Library.Messages;

namespace Models
{
    public class LevelProgressionModel : MonoBehaviour
    {
        private string _currentInputMethod = "Keyboard & Mouse";
        private int _currentLevel = 1;
        private string CurrentLevelName => $"Level_{_currentLevel}";
        [SerializeField] private int lastLevel;
        
        private void OnEnable()
        {
            Debug.Log("LPM Enable");
            Messenger.AddListener(PresenterToModel.SWITCHED_INPUT, OnSwitchedInput);
            Messenger.AddListener(GameEvents.START, OnStart);
            Messenger.AddListener(PresenterToModel.SUBMITTED, OnSubmitted);
        }

        private void OnDisable()
        {
            Debug.Log("LPM Disable");
            Messenger.RemoveListener(PresenterToModel.SWITCHED_INPUT, OnSwitchedInput);
            Messenger.RemoveListener(GameEvents.START, OnStart);
            Messenger.RemoveListener(PresenterToModel.SUBMITTED, OnSubmitted);
        }

        private void OnSwitchedInput()
        {
            Debug.Log("Switched input");
            if (_currentLevel <= lastLevel)
            {
                Debug.Log("Go to next scene");

                StartCoroutine(StartLevelWithInputMethod());
                
                _currentLevel++;
            }
            else
            {
                Debug.Log("Show you're done screen.");
            }
        }

        private void OnStart()
        {
            StartCoroutine(LoadInputSwitchSceneAsync());
        }

        private void OnSubmitted()
        {
            if (_currentLevel >= lastLevel)
            {
                LoadEndScene();
            }
            else
            {
                StartCoroutine(LoadInputSwitchSceneAsync());
            }
        }

        private IEnumerator LoadInputSwitchSceneAsync()
        {
            const string inputSwitchingSceneName = "Input_Switch";
            
            var load = SceneManager.LoadSceneAsync(inputSwitchingSceneName);

            while (!load.isDone) yield return null;
            
            Messenger<string>.Broadcast(ModelToPresenter.CURRENT_INPUT, _currentInputMethod);
        }

        private IEnumerator StartLevelWithInputMethod()
        {
            Debug.Assert(_currentLevel > 0 && _currentLevel <= lastLevel);
            
            var load = SceneManager.LoadSceneAsync(CurrentLevelName);

            while (!load.isDone) yield return null;
            
            Messenger<string>.Broadcast(ModelToPresenter.CURRENT_INPUT, _currentInputMethod);
        }

        private void LoadEndScene()
        {
            SceneManager.LoadScene("End");
        }
    }
}