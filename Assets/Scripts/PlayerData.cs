using System;
using System.Collections.Generic;

[Serializable]
public class PlayerData
{
    //DO NOT USE PROPERTIES. THEY DONT SHOW UP IN THE INSPECTOR
    public int Coins;
    public int Health = 6;
}

[Serializable]
public class GameData
{
    public List<PlayerData> PlayerDatas = new List<PlayerData>();
}