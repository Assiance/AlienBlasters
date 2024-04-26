using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] Vector2 _direction = Vector2.left;
    [SerializeField] float _distance = 10f;

    LineRenderer _lineRenderer;
    bool _isOn;

    void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        Toggle(false);
    }

    void Update()
    {
        if (!_isOn)
            return;

        var endpoint = (Vector2)transform.position + _direction * _distance;
        var hit = Physics2D.Raycast(transform.position, _direction, _distance);
        if (hit.collider != null)
        {
            endpoint = hit.point;

            var laserDamageable = hit.collider.GetComponent<ITakeLaserDamage>();
            if (laserDamageable != null)
            {
                laserDamageable.TakeLaserDamage();
            }
        }

        _lineRenderer.SetPosition(1, endpoint);
    }

    public void Toggle(bool state)
    {
        _isOn = state;
        _lineRenderer.enabled = _isOn;
    }
}
