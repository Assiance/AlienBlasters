using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BeeEncounter : MonoBehaviour, ITakeDamage
{
    [SerializeField] List<Transform> _lightnings;
    [SerializeField] float _delayBeforeDamage = 1.5f;
    [SerializeField] float _lightningAnimationTime = 2f;
    [SerializeField] float _delayBetweenLightning = 1f;
    [SerializeField] float _delayBetweenStrikes = 0.25f;
    [SerializeField] float _lightningRadius = 1f;
    [SerializeField] LayerMask _playerLayer;
    [SerializeField] int _numberOfLightnings = 1;
    [SerializeField] GameObject _bee;

    Collider2D[] _playerHitResults =  new Collider2D[10];
    List<Transform> _activeLightnings;
    public int _health = 10;

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
        _activeLightnings = new List<Transform>();

        foreach (var lightning in _lightnings)
        {
            lightning.gameObject.SetActive(false);
        }

        while (true)
        {
            for (int i = 0; i < _numberOfLightnings; i++)
            {
                yield return SpawnNewLightning();
            }

            yield return new WaitUntil(() => _activeLightnings.All(t => t.gameObject.activeSelf == false));
            _activeLightnings.Clear();
        }
    }

    private IEnumerator SpawnNewLightning()
    {
        if (_activeLightnings.Count >= _lightnings.Count)
        {
            Debug.LogError("The number of requested lightnings exceeds the total available lightnings.");
            yield break;
        }

        int index = Random.Range(0, _lightnings.Count);
        var currentLightning = _lightnings[index];

        while (_activeLightnings.Contains(currentLightning))
        {
            index = Random.Range(0, _lightnings.Count);
            currentLightning = _lightnings[index];
        }

        StartCoroutine(ShowLightning(currentLightning));
        _activeLightnings.Add(currentLightning);

        yield return new WaitForSeconds(_delayBetweenStrikes);
    }

    private IEnumerator ShowLightning(Transform currentLightning)
    {
        currentLightning.gameObject.SetActive(true);
        yield return new WaitForSeconds(_delayBeforeDamage);

        DamagePlayersInRange(currentLightning);
        yield return new WaitForSeconds(_lightningAnimationTime - _delayBeforeDamage);

        currentLightning.gameObject.SetActive(false);
        yield return new WaitForSeconds(_delayBetweenLightning);
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

    public void TakeDamage()
    {
        _health--;
        
        if (_health <= 0)
            _bee.SetActive(false);
    }
}
