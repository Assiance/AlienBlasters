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

    public void Toggle(bool state)
    {
        _isOn = state;
        _lineRenderer.enabled = state;
    }

    void Update()
    {
        if (!_isOn)
            return;
        
        var endPoint = (Vector2)transform.position + (_direction * _distance);

        var firstThing = Physics2D.Raycast(transform.position, _direction, _distance);
        if (firstThing.collider)
        {
            endPoint = firstThing.point;
        }

        _lineRenderer.SetPosition(1, endPoint);
    }
}
