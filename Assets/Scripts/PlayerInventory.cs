using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
    public Transform ItemPoint;

    PlayerInput _playerInput;

    IItem EquippedItem
    {
        get
        {
            return _items.Count >= _currentItemIndex ? _items[_currentItemIndex] : null;
        }
    }

    List<IItem> _items = new List<IItem>();
    int _currentItemIndex;

    void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerInput.actions["Fire"].performed += UseEquippedItem;
        _playerInput.actions["EquipNext"].performed += EquipNext;

        foreach (var item in GetComponentsInChildren<IItem>())
            Pickup(item);
    }

    void EquipNext(InputAction.CallbackContext obj)
    {
        _currentItemIndex++;
        if (_currentItemIndex >= _items.Count)
            _currentItemIndex = 0;

        ToggleEquippedItem();
    }

    private void ToggleEquippedItem()
    {
        for (int i = 0; i < _items.Count; i++)
        {
            _items[i].gameObject.SetActive(i == _currentItemIndex);
        }
    }

    private void UseEquippedItem(InputAction.CallbackContext obj)
    {
        if (EquippedItem != null && GameManager.CinematicPlaying == false)
            EquippedItem.Use();
    }

    public void Pickup(IItem item)
    {
        item.transform.SetParent(ItemPoint);
        item.transform.localPosition = Vector3.zero;
        
        _items.Add(item);
        _currentItemIndex = _items.Count - 1;

        ToggleEquippedItem();

        var collider = item.gameObject.GetComponent<Collider2D>();
        if (collider)
            collider.enabled = false;
    }
}
