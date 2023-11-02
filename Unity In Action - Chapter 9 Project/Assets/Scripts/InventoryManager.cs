using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;

public class InventoryManager : MonoBehaviour, IGameManager
{
    public ManagerStatus Status { get; private set; }
    private List<string> items;

    public void Startup()
    {
        Debug.Log("Inventory manager starting...");

        items = new List<string>();
        
        Status = ManagerStatus.Started;
    }

    private void DisplayItems()
    {
        Debug.Log($"Items: {string.Join(", ", items)}");
    }

    public void AddItem(string name)
    {
        items.Add(name);
        
        DisplayItems();
    }
    
    
}