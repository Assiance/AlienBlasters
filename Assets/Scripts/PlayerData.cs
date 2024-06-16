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
}