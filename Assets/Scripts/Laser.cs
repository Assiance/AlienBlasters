using UnityEngine;
using UnityEngine.UIElements;

public class Laser : MonoBehaviour
{
    [SerializeField] Vector2 _direction = Vector2.left;
    [SerializeField] float _distance = 10f;
    [SerializeField] SpriteRenderer _laserBurst;
    [SerializeField] float _burstSpeed = 1f;

    LineRenderer _lineRenderer;
    bool _isOn;

    void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        Toggle(false);
    }

    public void Toggle(bool state)
    {
        _isOn = state;
        _lineRenderer.enabled = state;
    }

    void Update()
    {
        if (!_isOn)
        {
            _laserBurst.enabled = false;
            return;
        }
        
        var endPoint = (Vector2)transform.position + (_direction * _distance);

        var firstThing = Physics2D.Raycast(transform.position, _direction, _distance);
        if (firstThing.collider)
        {
            endPoint = firstThing.point;
            
            _laserBurst.transform.position = firstThing.point;
            _laserBurst.enabled = true;
            _laserBurst.transform.localScale = Vector3.one * (0.75f + Mathf.PingPong(Time.time * _burstSpeed, 1f));

            var damageable = firstThing.collider.GetComponent<ITakeLaserDamage>();
            damageable?.TakeLaserDamage();
        }
        else
        {
            _laserBurst.enabled = false;
        }

        _lineRenderer.SetPosition(1, endPoint);
    }
}
