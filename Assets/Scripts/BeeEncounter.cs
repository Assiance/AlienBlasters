using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

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
    [SerializeField] Rigidbody2D _beeRigidBody;
    [SerializeField] Animator _beeAnimator;
    [SerializeField] Transform[] _beeDestinations;
    [SerializeField] float _minIdleTime = 1;
    [SerializeField] float _maxIdleTime = 2;
    [SerializeField] GameObject _beeLaser;
    [SerializeField] int _maxHealth = 50;

    Collider2D[] _playerHitResults = new Collider2D[10];
    List<Transform> _activeLightnings;
    int _currentHealth;
    bool _shotStarted;
    bool _shotFinished;

    void OnValidate()
    {
        if (_lightningAnimationTime <= _delayBeforeDamage)
            _delayBeforeDamage = _lightningAnimationTime;
    }

    void OnEnable()
    {
        _currentHealth = _maxHealth;
        
        StartCoroutine(StartLightning());
        StartCoroutine(StartMovement());

        var shootAnimationWrapper = GetComponentInChildren<ShootAnimationWrapper>();
        shootAnimationWrapper.OnShoot += () => _shotStarted = true;
        shootAnimationWrapper.OnReload += () => _shotFinished = true;
    }

    IEnumerator StartMovement()
    {
        _beeLaser.SetActive(false);
        var grabBag = new GrabBag<Transform>(_beeDestinations);
        while (true)
        {
            var destination = grabBag.Grab();
            if (destination is null)
            {
                Debug.LogWarning("Destination is null. Aborting movement.");
                yield break;
            }

            _beeAnimator.SetBool("Move", true);
            while (Vector2.Distance(_bee.transform.position, destination.position) > 0.1f)
            {
                _bee.transform.position =
                    Vector2.MoveTowards(_bee.transform.position, destination.position, Time.deltaTime);
                yield return null;
            }

            _beeAnimator.SetBool("Move", false);
            yield return new WaitForSeconds(Random.Range(_minIdleTime, _maxIdleTime));
            _beeAnimator.SetTrigger("Fire");

            yield return new WaitUntil(() => _shotStarted);
            _shotStarted = false;
            _beeLaser.SetActive(true);

            yield return new WaitUntil(() => _shotFinished);
            _shotFinished = false;
            _beeLaser.SetActive(false);
        }
    }

    IEnumerator StartLightning()
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
        var hits = Physics2D.OverlapCircleNonAlloc(lightning.position, _lightningRadius, _playerHitResults,
            _playerLayer);
        for (int i = 0; i < hits; i++)
        {
            var player = _playerHitResults[i].GetComponent<Player>();
            player?.TakeDamage(Vector3.zero);
        }
    }

    public void TakeDamage()
    {
        _currentHealth--;

        if (_currentHealth == _maxHealth / 2)
        {
            StartCoroutine(ToggleFlood(true));
        }
        if (_currentHealth <= 0)
        {
            StopAllCoroutines();
            StartCoroutine(ToggleFlood(false));
            _beeAnimator.SetBool("Dead", true);
            _beeRigidBody.bodyType = RigidbodyType2D.Dynamic;

            foreach (var collider in _bee.GetComponentsInChildren<Collider2D>())
            {
                collider.gameObject.layer = LayerMask.NameToLayer("Dead");
            }
        }
        else
            _beeAnimator.SetTrigger("Hit");
    }

    IEnumerator ToggleFlood(bool enableFlood)
    {
        var targetWaterY = enableFlood ? _water.transform.position.y + 1 : _water.transform.position.y - 1;
        _water.transform.position = new Vector3(_water.transform.position.x, targetWaterY, _water.transform.position.z);

        yield return null;
    }

    public Water _water;
    
    [ContextMenu(nameof(HalfHealth))]
    void HalfHealth()
    {
        _currentHealth = _maxHealth / 2;
        _currentHealth++;
        TakeDamage();
    }
    
    [ContextMenu(nameof(Kill))]
    void Kill()
    {
        _currentHealth = 1;
        TakeDamage();
    }
    
    [ContextMenu(nameof(FullHealth))]
    void FullHealth()
    {
        _currentHealth = _maxHealth;
    }
}