using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
    public Transform ItemPoint;

    PlayerInput _playerInput;
    Key _quippedKey;

    void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerInput.actions["Fire"].performed += UseEquippedItem;
    }

    private void UseEquippedItem(InputAction.CallbackContext obj)
    {
        if (_quippedKey)
            _quippedKey.Use();
    }

    public void Pickup(Key key)
    {
        key.transform.SetParent(ItemPoint);
        key.transform.localPosition = Vector3.zero;
        _quippedKey = key;
    }
}
