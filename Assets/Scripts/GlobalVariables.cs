﻿
using UnityEditor;
using UnityEngine;
using WebSocketSharp;

public class GlobalVariables : MonoBehaviour
{

    static public string MAIN_MENU_SCENE = "Menu";
    static public string WAITING_ROOM_SCENE = "WaitingRoom";
    static public string VILLAGE_SCENE = "Village";


    static public string roomName = "ABC";
    static public string color = "FakeColor";

    //static public bool isMasterClient;

    public const int minNumberPlayers = 2;
    public const int maxNumberPlayers = 5;

    static public float volumen = 1;
    public void SetVolumen(float newVolumen)
    {
        volumen = Mathf.Round(newVolumen * 100f) / 100f;
    }


    static private string playerName;
    static public void SetPlayerName(string name)
    {
        playerName = name;
    }

    static public string GetPlayerName()
    {
        if (playerName.IsNullOrEmpty())
        {
            playerName = "Peasant_" + Random.Range(0, 999);
        }
        return playerName;
    }

}
