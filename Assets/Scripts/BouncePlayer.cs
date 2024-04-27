using UnityEngine;

public class BouncePlayer : MonoBehaviour
{
    [SerializeField] bool _onlyFromTop;
    [SerializeField] float _bounciness = 200;

    void OnCollisionEnter2D(Collision2D other)
    {
        var notFromTop = Vector2.Dot(other.GetContact(0).normal, Vector2.down) < 0.5f;
        if (_onlyFromTop && notFromTop)
            return;

        if (other.collider.CompareTag("Player"))
        {
            var player = other.collider.GetComponent<Player>();
            if (player)
            {
                player.Bounce(other.contacts[0].normal, _bounciness);
            }
        }
    }
}