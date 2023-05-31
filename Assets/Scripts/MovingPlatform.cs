using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] Vector3 _position1;
    [SerializeField] Vector3 _position2;
    [Range(0, 1)]
    [SerializeField] float _percentAcross;

    void Update()
    {
        transform.position = Vector3.Lerp(_position1, _position2, _percentAcross);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        var collider = GetComponent<BoxCollider2D>();
        Gizmos.DrawWireCube(_position1, collider.bounds.size);
        Gizmos.DrawWireCube(_position2, collider.bounds.size);

        Gizmos.color = Color.yellow;
        var currentPosition = Vector3.Lerp(_position1, _position2, _percentAcross);
        Gizmos.DrawWireCube(currentPosition, collider.bounds.size);
    }

    [ContextMenu(nameof(SetPosition1))] public void SetPosition1() => _position1 = transform.position;

    [ContextMenu(nameof(SetPosition2))] public void SetPosition2() => _position2 = transform.position;
}
