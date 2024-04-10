using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

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
    }

    void HandleSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (arg0.name == "Menu")
            _playerInputManager.joinBehavior = PlayerJoinBehavior.JoinPlayersManually;
        else
        {
            _playerInputManager.joinBehavior = PlayerJoinBehavior.JoinPlayersWhenButtonIsPressed;
            SaveGame();
        }

    }

    void SaveGame()
    {
        var text = JsonUtility.ToJson(_gameData);
        Debug.Log("Saving Game: " + text);
        PlayerPrefs.SetString("Game1", text);
    }

    public void LoadGame()
    {
        var text = PlayerPrefs.GetString("Game1");
        Debug.Log("Loading Game: " + text);
        _gameData = JsonUtility.FromJson<GameData>(text);

        SceneManager.LoadScene("Level 1");
    }

    void HandlePlayerJoined(PlayerInput playerInput)
    {
        Debug.Log("Handle Player Joined " + playerInput);
        PlayerData playerData = GetPlayerData(playerInput.playerIndex);

        Player player = playerInput.GetComponent<Player>();
        player.Bind(playerData);
    }

    PlayerData GetPlayerData(int playerIndex)
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
        Debug.Log("New Game Called");
        _gameData = new GameData();
        SceneManager.LoadScene("Level 1");
    }
}
