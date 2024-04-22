using UnityEngine;
using UnityEngine.Events;

public class LaserSwitch : MonoBehaviour
{
    [SerializeField] Sprite _left;
    [SerializeField] Sprite _right;

    [SerializeField] UnityEvent _on;
    [SerializeField] UnityEvent _off;

    SpriteRenderer _spriteRenderer;
    bool _isOn;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        var player = collider.gameObject.GetComponent<Player>();
        if (player == null)
            return;

        var playerRb = player.GetComponent<Rigidbody2D>();
        if (playerRb.velocity.x > 0)
        {
            TurnOn();
        }
        else if (playerRb.velocity.x < 0)
        {
            TurnOff();
        }
    }
    private void TurnOff()
    {
        if (_isOn)
        {
            _isOn = false;
            _off.Invoke();
            _spriteRenderer.sprite = _left;
        }
    }

    private void TurnOn()
    {
        if (_isOn == false)
        {
            _isOn = true;
            _on.Invoke();
            _spriteRenderer.sprite = _right;
        }
    }

}
