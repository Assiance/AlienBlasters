using UnityEngine;

public class Brick : MonoBehaviour, ITakeLaserDamage
{
    [SerializeField] ParticleSystem _brickParticles;
    [SerializeField] float _laserDestructionTime = 1f;

    float _laserDamageTime;
    SpriteRenderer _spriteRenderer;
    float _resetColorTime;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (_resetColorTime > 0 && Time.time >= _resetColorTime)
        {
            _resetColorTime = 0f;
            _spriteRenderer.color = Color.white;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        var player = collision.gameObject.GetComponent<Player>();
        if (player == null)
            return;

        Vector2 normal = collision.GetContact(0).normal;
        float dot = Vector2.Dot(normal, Vector2.up);
        Debug.Log($"Dot: {dot}");

        if (dot > 0.5f)
        {
            player.StopJump();
            Explode();
        }
    }

    public void TakeLaserDamage()
    {
        _spriteRenderer.color = Color.red;
        _resetColorTime = Time.time + 0.1f;
        
        _laserDamageTime += Time.deltaTime;
        if (_laserDamageTime >= _laserDestructionTime)
            Explode();
    }

    private void Explode()
    {
        Instantiate(_brickParticles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
