using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;
using UnityEngine.InputSystem;

public class Key : MonoBehaviour
{
    [SerializeField] float _useRange = 1f;

    void OnTriggerEnter2D(Collider2D other)
    {
        var playerInventory = other.GetComponent<PlayerInventory>();
        if (playerInventory)
        {
            playerInventory.Pickup(this);
        }
    }

    public void Use()
    {
        var hits = Physics2D.OverlapCircleAll(transform.position, _useRange);
        foreach (var hit in hits)
        {
            var toggleLock = hit.GetComponent<ToggleLock>();
            if (toggleLock)
            {
                toggleLock.Toggle();
                break;
            }
        }
    }
}
