using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;

public class Blaster : MonoBehaviour
{
    [SerializeField] BlasterShot _blasterShotPrefab;
    [SerializeField] Transform _firePoint;

    Player _player;
    PlayerInput _playerInput;
    ObjectPool<BlasterShot> _pool;

    // Start is called before the first frame update
    void Awake()
    {
        _pool = new ObjectPool<BlasterShot>(AddNewBlasterShotToPool,
            shot => shot.gameObject.SetActive(true),
            shot => shot.gameObject.SetActive(false));
        
        _player = GetComponent<Player>();
        _playerInput = GetComponent<PlayerInput>();
        //_playerInput.actions["Fire"].performed += TryFire;
    }

    BlasterShot AddNewBlasterShotToPool()
    {
        var shot = Instantiate(_blasterShotPrefab);
        shot.SetPool(_pool);

        return shot;
    }

    void TryFire(InputAction.CallbackContext obj)
    {
        var shot = _pool.Get(); //Instantiate(_blasterShotPrefab, _firePoint.position, Quaternion.identity);
        shot.Launch(_player.Direction, _firePoint.position);
    }

    void Update()
    {
        if (_playerInput.actions["Fire"].ReadValue<float>() > 0)
        {
            var shot = _pool.Get(); //Instantiate(_blasterShotPrefab, _firePoint.position, Quaternion.identity);
            shot.Launch(_player.Direction, _firePoint.position);
        }
    }
}
