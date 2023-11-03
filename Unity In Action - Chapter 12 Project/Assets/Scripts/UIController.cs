using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private TMP_Text healthLabel;
    [SerializeField] private InventoryPopup popup;
    [SerializeField] private TMP_Text levelEnding;
    
    private void OnEnable()
    {
        Messenger.AddListener(GameEvent.HealthUpdated, OnHealthUpdated);
        Messenger.AddListener(GameEvent.LevelComplete, OnLevelComplete);
        Messenger.AddListener(GameEvent.LevelFailed, OnLevelFailed);
    }

    private void OnDisable()
    {
        Messenger.RemoveListener(GameEvent.HealthUpdated, OnHealthUpdated);
        Messenger.RemoveListener(GameEvent.LevelComplete, OnLevelComplete);
        Messenger.RemoveListener(GameEvent.LevelFailed, OnLevelFailed);
    }

    private void Start()
    {
        OnHealthUpdated();

        levelEnding.gameObject.SetActive(false);
        popup.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            var isShowing = popup.gameObject.activeSelf;
            popup.gameObject.SetActive(!isShowing);
            popup.Refresh();
        }
    }

    private void OnHealthUpdated()
    {
        var message = $"Health: {Managers.Player.Health}/{Managers.Player.MaxHealth}";
        healthLabel.text = message;
    }

    private void OnLevelComplete()
    {
        StartCoroutine(CompleteLevel());
    }

    private IEnumerator CompleteLevel()
    {
        levelEnding.gameObject.SetActive(true);
        levelEnding.text = "Level Complete!";

        yield return new WaitForSeconds(2);
        
        Managers.Mission.GoToNextLevel();
    }

    private void OnLevelFailed()
    {
        StartCoroutine(FailLevel());
    }

    private IEnumerator FailLevel()
    {
        levelEnding.gameObject.SetActive(true);
        levelEnding.text = "Level Failed :(";

        yield return new WaitForSeconds(2);
        
        Managers.Player.Respawn();
        Managers.Mission.RestartCurrentLevel();
    }
}