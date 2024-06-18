using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class PlayerData
{
    //DO NOT USE PROPERTIES. THEY DONT SHOW UP IN THE INSPECTOR
    public int Coins;
    public int Health = 6;
    public Vector2 Position;
    public Vector2 Velocity;
}

[Serializable]
public class GameData
{
    public List<PlayerData> PlayerDatas = new List<PlayerData>();
    public string GameName;
    public string CurrentLevelName;
    public List<LevelData> LevelDatas = new List<LevelData>();
}

[Serializable]
public class LevelData      
{
    public string LevelName;
    public List<CoinData> CoinDatas = new List<CoinData>();
    public List<LaserData> LaserDatas = new List<LaserData>();
}

[Serializable]
public class CoinData : INamed
{
    [field: SerializeField]
    public string Name { get; set; }
    public bool IsCollected;
}

[Serializable]
public class LaserData
{
    public string Name;
    public bool IsOn;
}