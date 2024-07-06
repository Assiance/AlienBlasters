using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class SplineAttackPoints : MonoBehaviour
{
    [SerializeField] SplineContainer _splineContainer;
    [SerializeField] List<float> _attackPoints;

    private void OnDrawGizmos()
    {
        if (_splineContainer == null)
            return;

        foreach (var point in _attackPoints)
        {
            var position = _splineContainer.EvaluatePosition(point);
            
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(position, 0.2f);
        }
    }
}