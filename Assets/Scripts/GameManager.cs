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
    public static bool IsLoading { get; private set; }

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
            _gameData.CurrentLevelName = arg0.name;
            _playerInputManager.joinBehavior = PlayerJoinBehavior.JoinPlayersWhenButtonIsPressed;

            var levelData = _gameData.LevelDatas.FirstOrDefault(l => l.LevelName == arg0.name);
            if (levelData == null)
            {
                levelData = new LevelData()
                {
                    LevelName = arg0.name
                };
                _gameData.LevelDatas.Add(levelData);
            }

            Bind<Coin, CoinData>(levelData.CoinDatas);
            BindLasers(levelData);
           // BindCoins(levelData);

            var allPlayers = FindObjectsOfType<Player>();
            foreach (var player in allPlayers)
            {
                var playerInput = player.GetComponent<PlayerInput>();
                var data = GetPlayerData(playerInput.playerIndex);
                player.Bind(data);

                if (GameManager.IsLoading)
                {
                    player.RestorePositionAndVelocity();
                    IsLoading = false;
                }
            }
            //SaveGame();
        }
    }

    void Bind<T, D>(List<D> datas) 
        where T : MonoBehaviour, IBind<D> 
        where D : INamed, new()
    {
        var instances = FindObjectsOfType<T>();
        foreach (var instance in instances)
        {
            var data = datas.FirstOrDefault(d => d.Name == instance.name);
            if (data == null)
            {
                data = new D() { Name = instance.name };
                datas.Add(data);
            }

            instance.Bind(data);
        }
    }

    void BindCoins(LevelData levelData)
    {
        var allCoins = FindObjectsOfType<Coin>();
        foreach (var coin in allCoins)
        {
            var data = levelData.CoinDatas.FirstOrDefault(c => c.Name == coin.name);
            if (data == null)
            {
                data = new CoinData()
                {
                    IsCollected = false,
                    Name = coin.name
                };
                levelData.CoinDatas.Add(data);
            }

            coin.Bind(data);
        }
    }

    void BindLasers(LevelData levelData)
    {
        var allLasers = FindObjectsOfType<LaserSwitch>();
        foreach (var laser in allLasers)
        {
            var data = levelData.LaserDatas.FirstOrDefault(l => l.Name == laser.name);
            if (data == null)
            {
                data = new LaserData()
                {
                    IsOn = false,
                    Name = laser.name
                };
                levelData.LaserDatas.Add(data);
            }

            laser.Bind(data);
        }
    }

    public void SaveGame()
    {
        if (string.IsNullOrEmpty(_gameData.GameName))
            _gameData.GameName = "Game " + AllGameNames.Count;

        var text = JsonUtility.ToJson(_gameData);
        Debug.Log("Saving Game: " + text);

        PlayerPrefs.SetString(_gameData.GameName, text);

        if (AllGameNames.Contains(_gameData.GameName) == false)
            AllGameNames.Add(_gameData.GameName);

        string commaSeperatedGameNames = string.Join(",", AllGameNames);
        PlayerPrefs.SetString("AllGameNames", commaSeperatedGameNames);
        PlayerPrefs.Save();
    }

    public void ReloadGame() => LoadGame(_gameData.GameName);

    public void LoadGame(string gameName)
    {
        IsLoading = true;
        var text = PlayerPrefs.GetString(gameName);
        Debug.Log("Loading Game: " + text);
        _gameData = JsonUtility.FromJson<GameData>(text);

        if (string.IsNullOrWhiteSpace(_gameData.CurrentLevelName))
            _gameData.CurrentLevelName = "Level 1";

        SceneManager.LoadScene(_gameData.CurrentLevelName);
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

public interface INamed
{
    public string Name { get; set; }
}