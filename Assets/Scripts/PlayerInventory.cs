using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
    public Transform ItemPoint;

    PlayerInput _playerInput;

    Key EquippedKey
    {
        get
        {
            return _items.Count >= _currentItemIndex ? _items[_currentItemIndex] : null;
        }
    }

    List<Key> _items = new List<Key>();
    int _currentItemIndex;

    void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerInput.actions["Fire"].performed += UseEquippedItem;
        _playerInput.actions["EquipNext"].performed += EquipNext;
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
        if (EquippedKey)
            EquippedKey.Use();
    }

    public void Pickup(Key key)
    {
        key.transform.SetParent(ItemPoint);
        key.transform.localPosition = Vector3.zero;
        
        _items.Add(key);
        _currentItemIndex = _items.Count - 1;

        ToggleEquippedItem();
    }
}