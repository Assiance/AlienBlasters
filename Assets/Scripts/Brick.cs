using UnityEngine;

public class Brick : MonoBehaviour
{
    [SerializeField] ParticleSystem _brickParticles;

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
            Instantiate(_brickParticles, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
