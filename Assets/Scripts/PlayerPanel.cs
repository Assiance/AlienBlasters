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
    }

    void Update()
    {
        if (_player)
        {
            _scoreText.SetText(_player.Coins.ToString());
            for (int i = 0; i < _hearts.Length; i++)
            {
                _hearts[i].enabled = i < _player.Health;
            }
        }
    }
}
