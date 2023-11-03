using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IGameManager
{
    public ManagerStatus Status { get; private set; }
    public int Health { get; private set; }
    public int MaxHealth { get; private set; }
    private NetworkService service;
    
    public void Startup(NetworkService s)
    {
        service = s;
        
        Debug.Log("Player manager starting...");

        Respawn();

        Status = ManagerStatus.Started;
    }

    public void ChangeHealth(int value)
    {
        Health += value;

        Health = Math.Min(MaxHealth, Math.Max(0, Health));
        
        if (Health == 0)
            Messenger.Broadcast(GameEvent.LevelFailed);
        
        Messenger.Broadcast(GameEvent.HealthUpdated);
    }

    public void UpdateData(int health, int maxHealth)
    {
        Health = health;
        MaxHealth = maxHealth;
    }

    public void Respawn()
    {
        UpdateData(5, 10);
    }
}