using System;
using UnityEngine;
using UnityEngine.Events;

public class ToggleLock : MonoBehaviour
{
    [SerializeField] UnityEvent OnUnlock;
    
    SpriteRenderer _spriteRenderer;
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

        if (_unlocked)
            OnUnlock?.Invoke();
    }
}
