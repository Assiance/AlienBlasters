using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    AudioSource _audioSource;
    BuoyancyEffector2D _buoyancyEffector2D;

    void Awake()
    {
        _buoyancyEffector2D = GetComponent<BuoyancyEffector2D>();
        _audioSource = GetComponent<AudioSource>();
        
        foreach (var waterFlowAnimation in GetComponentsInChildren<WaterFlowAnimation>())
        {
            waterFlowAnimation.enabled = false;
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _audioSource?.Play();
        }
    }
    
    public void SetSpeed(float speed)
    {
        _buoyancyEffector2D.flowMagnitude = speed;
        foreach (var waterFlowAnimation in GetComponentsInChildren<WaterFlowAnimation>())
        {
            waterFlowAnimation.enabled = speed != 0;
        }
    }
}