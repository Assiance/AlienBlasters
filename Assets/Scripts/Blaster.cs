using UnityEngine;
using UnityEngine.InputSystem;

public class Blaster : MonoBehaviour
{
    [SerializeField] BlasterShot _blasterShotPrefab;
    [SerializeField] Transform _firePoint;

    Player _player;
    PlayerInput _playerInput;

    // Start is called before the first frame update
    void Awake()
    {
        _player = GetComponent<Player>();
        _playerInput = GetComponent<PlayerInput>();
        _playerInput.actions["Fire"].performed += TryFire;
    }

    void TryFire(InputAction.CallbackContext obj)
    {
        var shot = Instantiate(_blasterShotPrefab, _firePoint.position, Quaternion.identity);
        shot.Launch(_player.Direction);
    }
}
