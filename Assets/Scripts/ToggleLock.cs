using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleLock : MonoBehaviour
{
    [SerializeField] SpriteRenderer _spriteRenderer;
    
    bool _unlocked;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.color = Color.gray;
    }

    [ContextMenu(nameof(Toggle))]
    public void Toggle()
    {
        _unlocked = !_unlocked;
        _spriteRenderer.color = _unlocked ? Color.white : Color.gray;
    }
}
