using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static bool CinematicPlaying { get; private set; }

    public List<string> AllGameNames = new List<string>();

    [SerializeField] GameData _gameData;
    
    PlayerInputManager _playerInputManager;

    public void ToggleCinematic(bool cinematicPlaying)
    {
        CinematicPlaying = cinematicPlaying;
    }


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

        var commaSeperatedGameNames = PlayerPrefs.GetString("AllGameNames");
        Debug.Log("commaSeperatedGameNames: " + commaSeperatedGameNames);
        AllGameNames = commaSeperatedGameNames.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList();
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

        PlayerPrefs.SetString(_gameData.GameName, text);

        if (AllGameNames.Contains(_gameData.GameName) == false)
            AllGameNames.Add(_gameData.GameName);

        string commaSeperatedGameNames = string.Join(",", AllGameNames);
        PlayerPrefs.SetString("AllGameNames", commaSeperatedGameNames);
        PlayerPrefs.Save();
    }

    public void LoadGame(string gameName)
    {
        var text = PlayerPrefs.GetString(gameName);
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
        _gameData.GameName = DateTime.Now.ToString("G");
        SceneManager.LoadScene("Level 1");
    }

    public void DeleteGame(string gameName)
    {
        PlayerPrefs.DeleteKey(gameName);
        AllGameNames.Remove(gameName);

        string commaSeperatedGameNames = string.Join(",", AllGameNames);
        PlayerPrefs.SetString("AllGameNames", commaSeperatedGameNames);
        PlayerPrefs.Save();
    }
}
