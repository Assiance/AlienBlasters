using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    float _jumpEndTime;
    [SerializeField] float _maxHorizontalSpeed = 5;
    [SerializeField] float _jumpVelocity = 5;
    [SerializeField] float _jumpDuration = 0.5f;
    [SerializeField] Sprite _jumpSprite;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _footOffset = 0.5f;
    [SerializeField] private int _jumpsRemaining;
    [SerializeField] float _acceleration = 10;

    public bool IsGrounded;
    
    SpriteRenderer _spriteRenderer;
    Animator _animator;
    private float _horizontal;
    private AudioSource _audioSource;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    void OnDrawGizmos()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Gizmos.color = Color.red;

        Vector2 origin = new Vector2(transform.position.x, transform.position.y - spriteRenderer.bounds.extents.y);
        Gizmos.DrawLine(origin, origin + Vector2.down * 0.1f);

        //Draw Left Foot
        origin = new Vector2(transform.position.x - _footOffset, transform.position.y - spriteRenderer.bounds.extents.y);
        Gizmos.DrawLine(origin, origin + Vector2.down * 0.1f);

        //Draw Right Foot
        origin = new Vector2(transform.position.x + _footOffset, transform.position.y - spriteRenderer.bounds.extents.y);
        Gizmos.DrawLine(origin, origin + Vector2.down * 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGrounding();

        var horizontalInput = Input.GetAxis("Horizontal");
        Debug.Log(_horizontal);
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        var vertical = rb.velocity.y;

        if (Input.GetButtonDown("Fire1") && _jumpsRemaining > 0)
        {
            _jumpEndTime = Time.time + _jumpDuration;
            _jumpsRemaining--;

            _audioSource.pitch = _jumpsRemaining > 0 ? 1f : 1.5f;
            _audioSource.Play();
        }

        if (Input.GetButton("Fire1") && _jumpEndTime > Time.time)
            vertical = _jumpVelocity;

        var desiredHorizontal = horizontalInput * _maxHorizontalSpeed;
        _horizontal = Mathf.Lerp(_horizontal, desiredHorizontal, Time.deltaTime * _acceleration);
        rb.velocity = new Vector2(_horizontal, vertical);
        UpdateSprite();
    }

    private void UpdateGrounding()
    {
        IsGrounded = false;

        //Check Left
        Vector2 origin = new Vector2(transform.position.x, transform.position.y - _spriteRenderer.bounds.extents.y);
        var hit = Physics2D.Raycast(origin, Vector2.down, 0.1f, _layerMask);
        if (hit.collider)
            IsGrounded = true;

        //Check Right
        origin = new Vector2(transform.position.x - _footOffset, transform.position.y - _spriteRenderer.bounds.extents.y);
        hit = Physics2D.Raycast(origin, Vector2.down, 0.1f, _layerMask);
        if (hit.collider)
            IsGrounded = true;

        //Check Left
        origin = new Vector2(transform.position.x + _footOffset, transform.position.y - _spriteRenderer.bounds.extents.y);
        hit = Physics2D.Raycast(origin, Vector2.down, 0.1f, _layerMask);
        if (hit.collider)
            IsGrounded = true;

        if (IsGrounded && GetComponent<Rigidbody2D>().velocity.y <= 0)
            _jumpsRemaining = 2;
    }

    private void UpdateSprite()
    {
        _animator.SetBool("IsGrounded", IsGrounded);
        _animator.SetFloat("HorizontalSpeed", Math.Abs(_horizontal));

        if (_horizontal > 0)
            _spriteRenderer.flipX = false;
        else if (_horizontal < 0)
            _spriteRenderer.flipX = true;
    }
}