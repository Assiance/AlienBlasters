using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] GameObject _opened1;
    [SerializeField] GameObject _opened2;
    [SerializeField] GameObject _closed1;
    [SerializeField] GameObject _closed2;

    [ContextMenu(nameof(Open))]
    public void Open()
    {
        _opened1.SetActive(true);
        _opened2.SetActive(true);
        _closed1.SetActive(false);
        _closed2.SetActive(false);
    }
    
    [ContextMenu(nameof(Close))]
    public void Close()
    {
        _opened1.SetActive(false);
        _opened2.SetActive(false);
        _closed1.SetActive(true);
        _closed2.SetActive(true);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var playerInteractionController = other.GetComponent<PlayerInteractionController>();
        if (playerInteractionController)
        {
            playerInteractionController.Add(this);
        }
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        var playerInteractionController = other.GetComponent<PlayerInteractionController>();
        if (playerInteractionController)
        {
            playerInteractionController.Remove(this);
        }
    }

    public void Interact(PlayerInteractionController playerInteractionController)
    {
        var distance1 = Vector2.Distance(playerInteractionController.transform.position, _opened1.transform.position);
        var distance2 = Vector2.Distance(playerInteractionController.transform.position, _opened2.transform.position);
        
        if (distance1 < distance2)
            playerInteractionController.transform.position = _opened2.gameObject.transform.position;
        else
            playerInteractionController.transform.position = _opened1.gameObject.transform.position;
    }
}
