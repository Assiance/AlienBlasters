using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] Vector2 _direction = Vector2.left;
    [SerializeField] float _distance = 10f;
    [SerializeField] SpriteRenderer _laserBurst;
    [SerializeField] LineRenderer _lineRenderer;

    bool _isOn;

    void Awake()
    {
        Toggle(false);
    }

    void OnValidate()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.SetPosition(0, transform.position);

        var endpoint = (Vector2)transform.position + _direction * _distance;
        var hit = Physics2D.Raycast(transform.position, _direction, _distance);
        if (hit.collider != null)
            endpoint = hit.point;

        _lineRenderer.SetPosition(1, endpoint);
        _laserBurst.transform.position = endpoint;
    }

    void Update()
    {
        if (!_isOn)
        {
            _laserBurst.enabled = false;
            return;
        }

        var endpoint = (Vector2)transform.position + _direction * _distance;
        var hit = Physics2D.Raycast(transform.position, _direction, _distance);
        if (hit.collider != null)
        {
            endpoint = hit.point;

            _laserBurst.enabled = true;
            _laserBurst.transform.position = endpoint;

            _laserBurst.transform.localScale = Vector3.one * (0.5f + Mathf.PingPong(Time.time, 1f));

            var laserDamageable = hit.collider.GetComponent<ITakeLaserDamage>();
            if (laserDamageable != null)
            {
                laserDamageable.TakeLaserDamage();
            }
        }
        else
        {
            _laserBurst.enabled = false;
        }

        _lineRenderer.SetPosition(1, endpoint);
    }

    public void Toggle(bool state)
    {
        _isOn = state;
        _lineRenderer.enabled = _isOn;
    }
}
