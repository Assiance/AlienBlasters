using UnityEngine;

public class CatBomb : MonoBehaviour
{
    [SerializeField] float _forceAmount = 300f;

    Rigidbody2D _rb;
    Animator _animator;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.isKinematic = true;

        _animator = GetComponentInChildren<Animator>();
        _animator.enabled = false;
    }

    public void Launch(Vector2 direction)
    {
        transform.SetParent(null);
        _rb.isKinematic = false;
        _rb.AddForce(direction * _forceAmount);
        _animator.enabled = true;
    }
}
