using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : MonoBehaviour
{
    Rigidbody2D _rb;
    SpriteRenderer _spriteRenderer;
    
    Sprite _defaultSprite;
    
    [SerializeField] float _jumpDelay = 3;
    [SerializeField] Vector2 _jumpForce;
    [SerializeField] Sprite _jumpSprite;
    
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _defaultSprite = _spriteRenderer.sprite;

        InvokeRepeating(nameof(Jump), _jumpDelay, _jumpDelay);
    }

    void Jump()
    {
        _rb.AddForce(_jumpForce);
        _spriteRenderer.sprite = _jumpSprite;
        
        _jumpForce *= new Vector2(-1, 1);
        _spriteRenderer.flipX = !_spriteRenderer.flipX;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        _spriteRenderer.sprite = _defaultSprite;
    }
}
