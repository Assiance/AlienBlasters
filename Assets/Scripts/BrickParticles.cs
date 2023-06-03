using UnityEngine;

public class BrickParticles : MonoBehaviour
{
    ParticleSystem _brickParticles;

    void Start()
    {
        _brickParticles = GetComponent<ParticleSystem>();
        Destroy(gameObject, _brickParticles.main.duration);
    }
}
