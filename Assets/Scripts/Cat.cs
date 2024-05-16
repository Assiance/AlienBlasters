using UnityEngine;

public class Cat : MonoBehaviour
{
    [SerializeField] CatBomb _catBombPrefab;
    [SerializeField] Transform _firePoint;

    CatBomb _catBomb;

    void Start()
    {
        SpawnCatBomb();

        var shootAnimationWrapper = GetComponentInChildren<ShootAnimationWrapper>();
        shootAnimationWrapper.OnReload += SpawnCatBomb;
        shootAnimationWrapper.OnShoot += ShootCatBomb;
    }

    void SpawnCatBomb()
    {
        if (_catBomb == null)
            _catBomb = Instantiate(_catBombPrefab, _firePoint);
    }

    void ShootCatBomb()
    {
        _catBomb.Launch(Vector2.up + Vector2.left);
        _catBomb = null;
    }
}
