using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    [SerializeField] bool _ignoreFromTop;

    void OnCollisionEnter2D(Collision2D other)
    {
        var hitFromTop = Vector2.Dot(other.GetContact(0).normal, Vector2.down) > 0.5f;
        if (_ignoreFromTop && hitFromTop)
            return;

        if (other.collider.CompareTag("Player"))
        {
            var player = other.collider.GetComponent<Player>();
            if (player)
            {
                player.TakeDamage(other.contacts[0].normal);
            }
        }
    }
}