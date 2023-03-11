using System;
using Unity.VisualScripting;
using UnityEngine;

public class MemoryCard : MonoBehaviour
{
    [SerializeField] private GameObject cardBack;
    [SerializeField] private SceneController controller;
    
    public void OnMouseDown()
    {
        if (!cardBack.activeSelf) return;
        
        cardBack.SetActive(false);
        controller.CardRevealed(this);
    }
    
    public int Id { get; private set; }

    public void SetCard(int id, Sprite image)
    {
        Id = id;
        GetComponent<SpriteRenderer>().sprite = image;
    }

    public void Unreveal()
    {
        cardBack.SetActive(true);
    }
}
