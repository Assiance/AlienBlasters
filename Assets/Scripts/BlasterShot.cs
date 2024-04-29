using UnityEngine;

public class BlasterShot : MonoBehaviour
{
    [SerializeField] private float _speed = 8f;
    
    Rigidbody2D _rb;
    Vector2 _direction = Vector2.right;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        _rb.velocity = _direction * _speed;
    }

    public void Launch(Vector2 direction)
    {
        _direction = direction;
        transform.rotation = _direction == Vector2.right ? Quaternion.identity : Quaternion.Euler(0, 180, 0);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        gameObject.SetActive(false);
    }
}
