using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeEncounter : MonoBehaviour
{
    [SerializeField] List<Transform> _lightnings;
    [SerializeField] float _delayBeforeDamage = 1.5f;
    [SerializeField] float _lightningAnimationTime = 2f;
    [SerializeField] float _delayBetweenLightning = 1f;
    [SerializeField] float _lightningRadius = 1f;
    [SerializeField] LayerMask _playerLayer;

    Collider2D[] _playerHitResults =  new Collider2D[10];

    void OnValidate()
    {
        if (_lightningAnimationTime <= _delayBeforeDamage)
            _delayBeforeDamage = _lightningAnimationTime;
    }

    void OnEnable()
    {
        StartCoroutine(StartEncounter());
    }
    
    IEnumerator StartEncounter()
    {
        while (true)
        {
            foreach (var lightning in _lightnings)
            {
                lightning.gameObject.SetActive(false);
            }

            yield return null;

            int index = Random.Range(0, _lightnings.Count);
            _lightnings[index].gameObject.SetActive(true);
            yield return new WaitForSeconds(_delayBeforeDamage);

            DamagePlayersInRange(_lightnings[index]);
            yield return new WaitForSeconds(_lightningAnimationTime - _delayBeforeDamage);
            
            _lightnings[index].gameObject.SetActive(false);
            yield return new WaitForSeconds(_delayBetweenLightning);
        }
    }

    void DamagePlayersInRange(Transform lightning)
    {
        var hits = Physics2D.OverlapCircleNonAlloc(lightning.position, _lightningRadius, _playerHitResults, _playerLayer);
        for (int i = 0; i < hits; i++)
        {
            var player = _playerHitResults[i].GetComponent<Player>();
            player?.TakeDamage(Vector3.zero);
        }
    }
}
