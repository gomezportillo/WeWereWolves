
using UnityEditor;
using UnityEngine;

public class GlobalVariables : MonoBehaviour
{

    static public string MAIN_MENU_SCENE = "Menu";
    static public string WAITING_ROOM_SCENE = "WaitingRoom";
    static public string VILLAGE_SCCENE = "Village";


    static public string roomCode = "ABC";
    static public string color = "FakeColor";

    //static public bool isMasterClient;

    public const int minNumberPlayers = 2;
    public const int maxNumberPlayers = 5;

    static public float volumen = 1;
    public void SetVolumen(float newVolumen)
    {
        volumen = Mathf.Round(newVolumen * 100f) / 100f;
        Debug.Log(volumen);
    }


    static private string playerName;
    static public void SetPlayerName(string name)
    {
        playerName = name;
    }

    static public string GetPlayerName()
    {
        if (string.IsNullOrEmpty(playerName))
        {
            playerName = "Peasant" + Random.Range(0, 999);
        }
        return playerName;
    }

}
