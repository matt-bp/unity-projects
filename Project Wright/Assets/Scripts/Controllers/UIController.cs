using System;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private TMP_Text currentLevelLabel;

        private void Start()
        {
            currentLevelLabel.text = $"Level: {StartedManagers.Level.CurrentLevel}";
        }

        public void OnSubmit()
        {
            StartedManagers.Level.GoToNextLevel();            
        }
    }
}