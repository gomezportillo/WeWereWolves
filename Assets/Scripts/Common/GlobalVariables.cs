
using UnityEditor;
using UnityEngine;
using WebSocketSharp;

public class GlobalVariables : MonoBehaviour
{

    public const string MAIN_MENU_SCENE = "MainMenu";
    public const string WAITING_ROOM_SCENE = "WaitingRoom";
    public const string VILLAGE_SCENE = "Village";

    public const KeyCode EVOLVE_KEY = KeyCode.T;
    public const KeyCode ATTACK_KEY = KeyCode.Space;

    public const int MIN_NUMBER_PLAYERS = 2;
    public const int MAX_NUMBER_PLAYERS = 5;

    static public float volumen = 1;
    public void SetVolumen(float newVolumen)
    {
        volumen = Mathf.Round(newVolumen * 100f) / 100f;
    }

    static public string GenerateRandomPlayerName()
    {
        return "Peasant_" + Random.Range(0, 999);
    }
}
