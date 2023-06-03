using UnityEngine;
using UnityEngine.Events;

public class LaserSwitch : MonoBehaviour
{
    [SerializeField] Sprite _left;
    [SerializeField] Sprite _right;
    
    [SerializeField] UnityEvent _on;
    [SerializeField] UnityEvent _off;

    SpriteRenderer _spriteRenderer;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        var player = collision.GetComponent<Player>();
        if (player == null)
            return;

        var playerRigidBody = player.GetComponent<Rigidbody2D>();
        if (playerRigidBody.velocity.x > 0)
            TurnOn();
        else if (playerRigidBody.velocity.x < 0)
            TurnOff();
    }

    void TurnOff()
    {
        _spriteRenderer.sprite = _left;
        _off.Invoke();
    }

    void TurnOn()
    {
        _spriteRenderer.sprite = _right;
        _on.Invoke();
    }
}
