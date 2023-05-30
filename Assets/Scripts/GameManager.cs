using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public List<string> AllGameNames = new List<string>();

    [SerializeField] GameData _gameData;

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

        AllGameNames = PlayerPrefs.GetString("AllGameNames").Split(',').ToList();   
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
            SaveGame();
        }
    }

    void SaveGame()
    {
        var text = JsonUtility.ToJson(_gameData);

        if (AllGameNames.Contains(_gameData.GameName) == false)
            AllGameNames.Add(_gameData.GameName);

        PlayerPrefs.SetString("AllGameNames", string.Join(",", AllGameNames));
        PlayerPrefs.SetString(_gameData.GameName, text);
        PlayerPrefs.Save();
    }

    public void LoadGame(string gameName)
    {
        var text = PlayerPrefs.GetString(gameName);
        _gameData = JsonUtility.FromJson<GameData>(text);
        SceneManager.LoadScene("Level 1");
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
        if (_gameData.PlayerDatas.Count <= playerIndex)
        {
            var playerData = new PlayerData();
            _gameData.PlayerDatas.Add(playerData);
        }

        return _gameData.PlayerDatas[playerIndex];
    }

    public void NewGame()
    {
        Debug.Log("New Game");
        
        _gameData = new GameData();
        _gameData.GameName = DateTime.Now.ToString("G");
        
        SceneManager.LoadScene("Level 1");
    }

    public void DeleteGame(string gameName)
    {
        AllGameNames.Remove(gameName);
        PlayerPrefs.DeleteKey(gameName);
        PlayerPrefs.SetString("AllGameNames", string.Join(",", AllGameNames));
        PlayerPrefs.Save();
    }
}
