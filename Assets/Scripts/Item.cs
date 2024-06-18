using System;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        var playerInventory = other.GetComponent<PlayerInventory>();
        if (playerInventory)
        {
            playerInventory.Pickup(this, true);
        }
    }
    
    public abstract void Use();
}