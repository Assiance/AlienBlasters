using Cinemachine;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] float _maxHorizontalSpeed = 5;
    [SerializeField] float _jumpVelocity = 5;
    [SerializeField] float _jumpDuration = 0.5f;
    [SerializeField] Sprite _jumpSprite;
    [SerializeField] LayerMask _layerMask;
    [SerializeField] LayerMask _waterLayerMask;
    [SerializeField] float _footOffset = 0.5f;
    [SerializeField] float _groundAcceleration = 10;
    [SerializeField] float _snowAcceleration = 1;
    [SerializeField] AudioClip _coinSfx;
    [SerializeField] AudioClip _hurtSfx;
    [SerializeField] float _knockbackVelocity = 400;
    [SerializeField] Collider2D _duckingCollider;
    [SerializeField] Collider2D _standingCollider;
    [SerializeField] float _wallDetectionDistance = 0.5f;
    [SerializeField] int _wallCheckPoints = 5;
    [SerializeField] float _buffer = 0.1f;

    public Transform ItemPoint;

    public bool IsGrounded;
    public bool IsInWater;
    public bool IsOnSnow;
    public bool IsDucking;
    public bool IsTouchingRightWall;
    public bool IsTouchingLeftWall;

    PlayerData _playerData = new PlayerData();

    public event Action OnCoinsChanged;
    public event Action OnHealthChanged;

    public int Coins
    {
        get => _playerData.Coins;
        private set => _playerData.Coins = value;
    }

    public int Health => _playerData.Health;
    public Vector2 Direction { get; private set; } = Vector2.right;

    Animator _animator;
    SpriteRenderer _spriteRenderer;
    AudioSource _audioSource;
    Rigidbody2D _rb;
    PlayerInput _playerInput;

    float _horizontal;
    int _jumpsRemaining;
    float _jumpEndTime;
    RaycastHit2D[] _results = new RaycastHit2D[100];

    void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _audioSource = GetComponent<AudioSource>();
        _rb = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();

        FindObjectOfType<PlayerCanvas>().Bind(this);
    }

    void OnEnable()
    {
        FindObjectOfType<CinemachineTargetGroup>()?.AddMember(transform, 1f, 1f);
    }

    void OnDisable()
    {
        FindObjectOfType<CinemachineTargetGroup>()?.RemoveMember(transform);
    }

    void OnDrawGizmos()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Gizmos.color = Color.red;

        Vector2 origin = new Vector2(transform.position.x, transform.position.y - spriteRenderer.bounds.extents.y);
        Gizmos.DrawLine(origin, origin + Vector2.down * 0.1f);

        // Draw Left Foot
        origin = new Vector2(transform.position.x - _footOffset,
            transform.position.y - spriteRenderer.bounds.extents.y);
        Gizmos.DrawLine(origin, origin + Vector2.down * 0.1f);

        // Draw Right Foot
        origin = new Vector2(transform.position.x + _footOffset,
            transform.position.y - spriteRenderer.bounds.extents.y);
        Gizmos.DrawLine(origin, origin + Vector2.down * 0.1f);

        DrawGizmosForSide(Vector2.left);
        DrawGizmosForSide(Vector2.right);
    }

    void DrawGizmosForSide(Vector2 direction)
    {
        var activeCollider = IsDucking ? _duckingCollider : _standingCollider;
        float colliderHeight = activeCollider.bounds.size.y - 2 * _buffer;
        float segmentSize = colliderHeight / (float)(_wallCheckPoints - 1);

        Vector3 colliderOffset = activeCollider.offset;

        for (int i = 0; i < _wallCheckPoints; i++)
        {
            // Calculate the starting point at the top of the collider
            var origin = transform.position + (Vector3)colliderOffset -
                         new Vector3(0, activeCollider.bounds.size.y / 2f, 0);
            // Adjust the origin to include the buffer and distribute points evenly along the collider height
            origin += new Vector3(0, _buffer + i * segmentSize, 0);
            // Apply the direction and wall detection distance
            origin += (Vector3)direction * _wallDetectionDistance;
            // Draw the gizmo at the calculated position
            Gizmos.DrawWireSphere(origin, 0.05f);
        }
    }

    bool CheckForWall(Vector2 direction)
    {
        var activeCollider = IsDucking ? _duckingCollider : _standingCollider;
        float colliderHeight = activeCollider.bounds.size.y - 2 * _buffer;
        float segmentSize = colliderHeight / (float)(_wallCheckPoints - 1);

        Vector3 colliderOffset = activeCollider.offset;

        for (int i = 0; i < _wallCheckPoints; i++)
        {
            // Calculate the starting point at the top of the collider
            var origin = transform.position + (Vector3)colliderOffset -
                         new Vector3(0, activeCollider.bounds.size.y / 2f, 0);
            // Adjust the origin to include the buffer and distribute points evenly along the collider height
            origin += new Vector3(0, _buffer + i * segmentSize, 0);
            // Apply the direction and wall detection distance
            origin += (Vector3)direction * _wallDetectionDistance;
            // Draw the gizmo at the calculated position

            int hits = Physics2D.Raycast(origin,
                direction,
                new ContactFilter2D() { layerMask = _layerMask, useLayerMask = true, useTriggers = true },
                _results,
                0.1f);

            for (int j = 0; j < hits; j++)
            {
                var hit = _results[j];
                if (hit.collider == null)
                    continue;

                if (hit.collider.isTrigger)
                    continue;

                return true;
            }
        }

        return false;
    }


    // Update is called once per frame
    void Update()
    {
        UpdateGrounding();
        UpdateWallTouching();

        if (GameManager.CinematicPlaying == false)
        {
            UpdateMovement();
        }

        UpdateAnimation();
        UpdateDirection();
    }

    private void UpdateWallTouching()
    {
        IsTouchingRightWall = CheckForWall(Vector2.right);
        IsTouchingLeftWall = CheckForWall(Vector2.left);
    }

    private void UpdateMovement()
    {
        var input = _playerInput.actions["Move"].ReadValue<Vector2>();
        var horizontalInput = input.x;
        var verticalInput = input.y;

        var vertical = _rb.velocity.y;

        if (_playerInput.actions["Jump"].WasPerformedThisFrame() && _jumpsRemaining > 0)
        {
            _jumpEndTime = Time.time + _jumpDuration;
            _jumpsRemaining--;

            _audioSource.pitch = _jumpsRemaining > 0 ? 1 : 1.2f;
            _audioSource.Play();
        }

        if (_playerInput.actions["Jump"].ReadValue<float>() > 0 && _jumpEndTime > Time.time)
            vertical = _jumpVelocity;

        var desiredHorizontal = horizontalInput * _maxHorizontalSpeed;
        var acceleration = IsOnSnow ? _snowAcceleration : _groundAcceleration;

        //_horizontal = Mathf.Lerp(_horizontal, desiredHorizontal, Time.deltaTime * acceleration);

        _animator.SetBool("Duck", verticalInput < 0 && Mathf.Abs(verticalInput) > Mathf.Abs(horizontalInput));

        IsDucking = _animator.GetBool("IsDucking");
        if (IsDucking)
            desiredHorizontal = 0;

        _duckingCollider.enabled = IsDucking;
        _standingCollider.enabled = !IsDucking;

        if (desiredHorizontal > _horizontal)
        {
            _horizontal += Time.deltaTime * acceleration;
            if (_horizontal > desiredHorizontal)
                _horizontal = desiredHorizontal;
        }
        else if (desiredHorizontal < _horizontal)
        {
            _horizontal -= Time.deltaTime * acceleration;
            if (_horizontal < desiredHorizontal)
                _horizontal = desiredHorizontal;
        }

        if (desiredHorizontal > 0 && IsTouchingRightWall)
            _horizontal = 0;

        if (desiredHorizontal < 0 && IsTouchingLeftWall)
            _horizontal = 0;

        if (IsInWater)
            _rb.velocity = new Vector2(_rb.velocity.x, vertical);
        else
            _rb.velocity = new Vector2(_horizontal, vertical);
    }


    void UpdateGrounding()
    {
        IsGrounded = false;
        IsOnSnow = false;
        IsInWater = false;

        // Check center
        Vector2 origin = new Vector2(transform.position.x, transform.position.y - _spriteRenderer.bounds.extents.y);
        CheckGrounding(origin);

        // Check left
        origin = new Vector2(transform.position.x - _footOffset,
            transform.position.y - _spriteRenderer.bounds.extents.y);
        CheckGrounding(origin);

        // Check right
        origin = new Vector2(transform.position.x + _footOffset,
            transform.position.y - _spriteRenderer.bounds.extents.y);
        CheckGrounding(origin);

        if ((IsGrounded || IsInWater) && _rb.velocity.y <= 0)
            _jumpsRemaining = 2;
    }

    private void CheckGrounding(Vector2 origin)
    {
        int hits = Physics2D.Raycast(origin,
            Vector2.down,
            new ContactFilter2D() { layerMask = _layerMask, useLayerMask = true, useTriggers = true, },
            _results,
            0.1f);

        for (int i = 0; i < hits; i++)
        {
            var hit = _results[i];
            if (hit.collider == null)
                continue;

            IsGrounded = true;
            IsOnSnow = IsOnSnow || hit.collider.CompareTag("Snow");
        }

        var water = Physics2D.OverlapPoint(origin, _waterLayerMask);
        IsInWater = water is not null;
    }

    void UpdateAnimation()
    {
        _animator.SetBool("Jump", !IsGrounded);
        _animator.SetBool("Move", _horizontal != 0);
    }

    void UpdateDirection()
    {
        if (_horizontal > 0)
        {
            _animator.transform.rotation = Quaternion.identity;
            Direction = Vector2.right;
        }
        else if (_horizontal < 0)
        {
            _animator.transform.rotation = Quaternion.Euler(0, 180, 0);
            Direction = Vector2.left;
        }
    }

    public void AddPoint()
    {
        Coins++;
        _audioSource.PlayOneShot(_coinSfx);
        OnCoinsChanged?.Invoke();
    }

    public void Bind(PlayerData playerData)
    {
        _playerData = playerData;
    }

    public void TakeDamage(Vector2 hitNormal)
    {
        _playerData.Health--;
        if (_playerData.Health <= 0)
        {
            SceneManager.LoadScene(0);
            return;
        }

        _rb.AddForce(-hitNormal * _knockbackVelocity);
        _audioSource.PlayOneShot(_hurtSfx);
        OnHealthChanged?.Invoke();
    }

    public void StopJump()
    {
        _jumpEndTime = Time.time;
    }

    public void Bounce(Vector2 normal, float bounciness)
    {
        _rb.AddForce(-normal * bounciness);
    }
}