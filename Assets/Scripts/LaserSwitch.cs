using UnityEngine;
using UnityEngine.Events;

public class LaserSwitch : MonoBehaviour
{
    [SerializeField] Sprite _left;
    [SerializeField] Sprite _right;

    [SerializeField] UnityEvent _on;
    [SerializeField] UnityEvent _off;

    SpriteRenderer _spriteRenderer;
    LaserData _data;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    public void Bind(LaserData data)
    {
        _data = data;
        UpdateSwitchState();
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
        if (_data.IsOn)
        {
            _data.IsOn = false;
            UpdateSwitchState();
            _data.IsOn = false;
        }
    }

    private void TurnOn()
    {
        if (_data.IsOn == false)
        {
            _data.IsOn = true;
            UpdateSwitchState();
            _data.IsOn = true;
        }
    }
    
    void UpdateSwitchState()
    {
        if (_data.IsOn)
        {
            _on.Invoke();
            _spriteRenderer.sprite = _right;
        }
        else
        {
            _off.Invoke();
            _spriteRenderer.sprite = _left;
        }
    }
}
