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
        var player = other.GetComponent<Player>();
        if (player)
        {
            transform.SetParent(player.ItemPoint);
            transform.localPosition = Vector3.zero;
            var playerInput = player.GetComponent<PlayerInput>();
            playerInput.actions["Fire"].performed += UseKey;
        }
    }

    void UseKey(InputAction.CallbackContext context)
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
