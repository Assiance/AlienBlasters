using UnityEngine;

public class BrickParticle : MonoBehaviour
{
    void Start()
    {
        var particleSystem = GetComponent<ParticleSystem>();
        Destroy(gameObject, particleSystem.main.duration);
    }
}
