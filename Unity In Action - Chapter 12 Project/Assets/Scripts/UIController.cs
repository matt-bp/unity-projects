using System;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private TMP_Text healthLabel;
    [SerializeField] private InventoryPopup popup;

    private void OnEnable()
    {
        Messenger.AddListener(GameEvent.HealthUpdated, OnHealthUpdated);
    }

    private void OnDisable()
    {
        Messenger.RemoveListener(GameEvent.HealthUpdated, OnHealthUpdated);
    }

    private void Start()
    {
        OnHealthUpdated();

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
}