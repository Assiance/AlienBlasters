using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPanel : MonoBehaviour
{
    [SerializeField] TMP_Text _scoreText;
    [SerializeField] Image[] _hearts;

    Player _player;

    public void Bind(Player player)
    {
        _player = player;
        
        _player.CoinsChanged += UpdateCoins;
        _player.HealthChanged += UpdateHealth;
        UpdateCoins();
        UpdateHealth();
    }

    void UpdateHealth()
    {
        for (int i = 0; i < _hearts.Length; i++)
        {
            _hearts[i].enabled = i < _player.Health;
        }
    }

    void UpdateCoins()
    {
        _scoreText.SetText(_player.Coins.ToString());
    }
}
