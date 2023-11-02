using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;

public class InventoryManager : MonoBehaviour, IGameManager
{
    public ManagerStatus Status { get; private set; }
    private Dictionary<string, int> items;
    public string EquippedItem { get; private set; }

    public void Startup()
    {
        Debug.Log("Inventory manager starting...");

        items = new Dictionary<string, int>();
        
        Status = ManagerStatus.Started;
    }

    private void DisplayItems()
    {
        Debug.Log($"Items: {string.Join(", ", items)}");
    }

    public void AddItem(string name)
    {
        if (!items.ContainsKey(name))
            items[name] = 0;

        items[name]++;
        
        DisplayItems();
    }

    public List<string> GetItemList() => items.Keys.ToList();

    public int GetItemCount(string name) => items.TryGetValue(name, out var value) ? value : 0;

    public bool EquipItem(string name)
    {
        if (items.ContainsKey(name) && EquippedItem != name)
        {
            EquippedItem = name;
            Debug.Log($"Equipped {name}");
            return true;
        }

        EquippedItem = null;
        Debug.Log("Unequipped");
        return false;
    }

    public bool ConsumeItem(string name)
    {
        if (items.ContainsKey(name))
        {
            items[name]--;
            if (items[name] == 0)
            {
                items.Remove(name);
            }
        }
        else
        {
            Debug.Log($"Cannot consume {name}");
            return false;
        }
        
        DisplayItems();
        return true;
    }
}