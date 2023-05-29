using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] List<PlayerData> _playerDatas = new List<PlayerData>();

    PlayerInputManager _playerInputManager;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        _playerInputManager = GetComponent<PlayerInputManager>();
        _playerInputManager.onPlayerJoined += HandlePlayerJoined;

        SceneManager.sceneLoaded += HandleSceneLoaded;
    }

    void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Menu")
        {
            _playerInputManager.joinBehavior = PlayerJoinBehavior.JoinPlayersManually;
        }
        else
        {
            _playerInputManager.joinBehavior = PlayerJoinBehavior.JoinPlayersWhenButtonIsPressed;
        }
    }

    void HandlePlayerJoined(PlayerInput playerInput)
    {
        Debug.Log("Player Joined " + playerInput.playerIndex);
        var playerData = GetPlayerData(playerInput.playerIndex);

        var player = playerInput.GetComponent<Player>();
        player.Bind(playerData);
    }

    private PlayerData GetPlayerData(int playerIndex)
    {
        if (_playerDatas.Count <= playerIndex)
        {
            var playerData = new PlayerData();
            _playerDatas.Add(playerData);
        }

        return _playerDatas[playerIndex];
    }

    public void NewGame()
    {
        Debug.Log("New Game");
    }
}
