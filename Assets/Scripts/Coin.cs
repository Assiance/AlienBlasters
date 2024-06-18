using UnityEngine;

public class Coin : MonoBehaviour, IBind<CoinData>
{
    CoinData _data;

    public void Bind(CoinData data)
    {
        _data = data;
        gameObject.SetActive(!_data.IsCollected);
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<Player>();
        if (player != null)
        {
            _data.IsCollected = true;
            _data.Name = this.name;
            player.AddPoint();
            gameObject.SetActive(false);
        }
    }
}

public interface IBind<T>
{
    void Bind(T data);
}
