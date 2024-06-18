using UnityEngine;

public class Blaster : Item
{
    [SerializeField] Transform _firePoint;

    Player _player;

    // Start is called before the first frame update
    void Awake()
    {
        _player = GetComponentInParent<Player>();
    }

    public override void Use()
    {
        var shot = PoolManager.Instance.GetBlasterShot();
        shot.Launch(_player.Direction, _firePoint.position);
    }

}
