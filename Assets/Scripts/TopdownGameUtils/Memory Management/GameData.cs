using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class GameData 
{
    [SerializeField]
    Vector3 playerPos;
    [SerializeField]
    string gameName;

    public GameData(){}

    public GameData(Vector3 playerPos, string gameName)
    {
        this.playerPos = playerPos;
        this.gameName = gameName;
    }

    public Vector3 PlayerPos { get => playerPos; set => playerPos = value; }
    public string GameName { get => gameName; set => gameName = value; }
}
