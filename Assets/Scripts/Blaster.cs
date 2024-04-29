using UnityEngine;
using UnityEngine.InputSystem;

public class Blaster : MonoBehaviour
{
    [SerializeField] GameObject _blasterShotPrefab;
    [SerializeField] Transform _firePoint;

    PlayerInput _playerInput;

    // Start is called before the first frame update
    void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerInput.actions["Fire"].performed += TryFire;
    }

    void TryFire(InputAction.CallbackContext obj)
    {
        Instantiate(_blasterShotPrefab, _firePoint.position, Quaternion.identity);
    }
}
