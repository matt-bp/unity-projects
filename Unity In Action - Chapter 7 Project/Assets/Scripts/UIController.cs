using System;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreLabel;
    [SerializeField] private SettingsPopup settingsPopup;
    private int _score;

    private void Start()
    {
        _score = 0;
        scoreLabel.text = _score.ToString();
        
        settingsPopup.Close();
    }
    
    public void OnOpenSettings()
    {
        settingsPopup.Open();
    }

    public void OnDown()
    {
        Debug.Log("Down...");
    }

    private void OnEnable()
    {
        Messenger.AddListener(GameEvent.ENEMY_HIT, OnEnemyHit);
    }

    private void OnDisable()
    {
        Messenger.RemoveListener(GameEvent.ENEMY_HIT, OnEnemyHit);
    }

    private void OnEnemyHit()
    {
        _score += 1;
        scoreLabel.text = _score.ToString();
    }
}
