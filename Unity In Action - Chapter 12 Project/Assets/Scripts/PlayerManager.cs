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

        Health = 50;
        MaxHealth = 100;

        Status = ManagerStatus.Started;
    }

    public void ChangeHealth(int value)
    {
        Health += value;

        Health = Math.Min(MaxHealth, Math.Max(0, Health));
        
        Messenger.Broadcast(GameEvent.HealthUpdated);
    }
}