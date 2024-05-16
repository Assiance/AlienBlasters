using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : MonoBehaviour
{
    [SerializeField] private GameObject _catBombPrefab;
    [SerializeField] Transform _firePoint;


    void Start()
    {
        InvokeRepeating(nameof(SpawnCatBomb), 3f, 3f);
    }

    void SpawnCatBomb()
    {
        Instantiate(_catBombPrefab, _firePoint);
    }

    void Update()
    {
        
    }
}
