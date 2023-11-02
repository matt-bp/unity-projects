using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BasicUI : MonoBehaviour
{
    private void OnGUI()
    {
        var posX = 10;
        var posY = 10;
        var width = 100;
        var height = 30;
        var buffer = 10;

        var itemList = Managers.Inventory.GetItemList();
        if (!itemList.Any())
        {
            GUI.Box(new Rect(posX, posY, width, height), "No Items");
        }

        foreach (var item in itemList)
        {
            var count = Managers.Inventory.GetItemCount(item);
            var image = Resources.Load<Texture2D>($"Icons/{item}");
            GUI.Box(new Rect(posX, posY, width, height), new GUIContent($"({count})", image));
            posX += width + buffer;
        }

        var equipped = Managers.Inventory.EquippedItem;
        if (equipped is not null)
        {
            posX = Screen.width - (width + buffer);
            var image = Resources.Load<Texture2D>($"Icons/{equipped}");
            GUI.Box(new Rect(posX, posY, width, height), new GUIContent("Equipped", image));
        }

        posX = 10;
        posY += height + buffer;

        foreach (var item in itemList)
        {
            if (GUI.Button(new Rect(posX, posY, width, height), $"Equip {item}"))
            {
                Managers.Inventory.EquipItem(item);
            }

            posX += width + buffer;
        }
    }
}
