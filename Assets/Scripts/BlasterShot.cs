using UnityEngine;

public class BlasterShot : MonoBehaviour
{
    [SerializeField] float _speed = 8f;
    [SerializeField] GameObject _impactExplosion;
    [SerializeField] float _maxLifetime = 4f;
    
    Rigidbody2D _rb;
    Vector2 _direction = Vector2.right;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, _maxLifetime);
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
        var damageable = collision.gameObject.GetComponent<ITakeDamage>();
        if (damageable != null)
        {
            damageable.TakeDamage();
        }

        var explosion = Instantiate(_impactExplosion, collision.contacts[0].point, Quaternion.identity);
        Destroy(explosion, 0.8f);
        
        Destroy(gameObject);
        //gameObject.SetActive(false);
    }
}
