using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryPopup : MonoBehaviour
{
    [SerializeField] private Image[] itemIcons;
    [SerializeField] private TMP_Text[] itemLabels;
    
    [SerializeField] private TMP_Text curItemLabel;
    [SerializeField] private Button equipButton;
    [SerializeField] private Button useButton;

    private string curItem;
    
    public void Refresh()
    {
        var itemList = Managers.Inventory.GetItemList();

        var len = itemIcons.Length;
        for (int i = 0; i < len; i++)
        {
            if (i < itemList.Count)
            {
                itemIcons[i].gameObject.SetActive(true);
                itemLabels[i].gameObject.SetActive(true);

                var item = itemList[i];

                var sprite = Resources.Load<Sprite>($"Icons/{item}");
                itemIcons[i].sprite = sprite;
                // itemIcons[i].SetNativeSize();

                var count = Managers.Inventory.GetItemCount(item);
                var message = $"x{count}";
                if (item == Managers.Inventory.EquippedItem)
                {
                    message = $"Equipped\n{message}";
                }

                itemLabels[i].text = message;

                var entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerClick;
                entry.callback.AddListener((_) =>
                {
                    OnItem(item);
                });

                var trigger = itemIcons[i].GetComponent<EventTrigger>();
                trigger.triggers.Clear();
                trigger.triggers.Add(entry);
            }
            else
            {
                itemIcons[i].gameObject.SetActive(false);
                itemLabels[i].gameObject.SetActive(false);
            }
        }

        if (!itemList.Contains(curItem))
        {
            curItem = null;
        }

        if (curItem == null)
        {
            curItemLabel.gameObject.SetActive(false);
            equipButton.gameObject.SetActive(false);
            useButton.gameObject.SetActive(false);
        }
        else
        {
            curItemLabel.gameObject.SetActive(true);
            equipButton.gameObject.SetActive(true);

            useButton.gameObject.SetActive(curItem == "health");

            curItemLabel.text = $"{curItem}:";
        }
    }

    public void OnItem(string item)
    {
        curItem = item;
        Refresh();
    }

    public void OnEquip()
    {
        Managers.Inventory.EquipItem(curItem);
        Refresh();
    }

    public void OnUse()
    {
        Managers.Inventory.ConsumeItem(curItem);
    }
}