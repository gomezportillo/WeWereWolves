using Photon.Pun;
using UnityEngine;
using WebSocketSharp;

public class GUIProxyNetworkManager : MonoBehaviour
{

    [SerializeField]
    public GameObject networkManager;

    [SerializeField]
    public TMPro.TMP_InputField villajeCodeInput;

    [SerializeField]
    public TMPro.TMP_InputField playerNameInput;

    [SerializeField]
    public TMPro.TextMeshProUGUI errorText;

    [SerializeField]
    public TMPro.TextMeshProUGUI regionText;

    void Start()
    {
        errorText.text = string.Empty;
        playerNameInput.text = GlobalVariables.GetPlayerName();

        NetworkEventManager.instance.ConnectedToMaster += UpdateRegionGUI;
        NetworkEventManager.instance.JoinedRoom += JoinedRoom;
        NetworkEventManager.instance.JoinRoomFailed += ErrorJoiningRoom;
        NetworkEventManager.instance.CreateRoomFailed += ErrorCreatingRoom;

    }

    public void JoinedRoom(string roomCode)
    {
        GlobalVariables.roomCode = roomCode;

        string playerName = playerNameInput.text; // comprobar isNullOrEmpty
        GlobalVariables.SetPlayerName(playerName);
        NetworkManager.instance.SetPlayerNickname(playerName);
    }

    public void ErrorJoiningRoom(short returnCode, string message)
    {
        errorText.text = "This village does not exist";
    }

    public void CreateRoom()
    {
        NetworkManager.instance.CreateRoom(null);
    }

    public void ErrorCreatingRoom(short returnCode, string message)
    {
        errorText.text = message;
    }

    public void UpdateRegionGUI(string regionCode)
    {
        regionText.text = "Region: " + RegionCodeToText(regionCode);
    }

    public void ReadInputAndJoinRoom()
    {
        string code = villajeCodeInput.text.ToUpper();

        if (string.IsNullOrEmpty(code))
        {
            errorText.text = "Village code cannot be empty";
        }
        else
        {
            NetworkManager.instance.JoinRoom(code);
        }
    }

    public void SetPlayerNickname(string nickname)
    {
        if (string.IsNullOrEmpty(nickname))
        {
            // mostrar error
        }
        else
        {
            NetworkManager.instance.SetPlayerNickname(nickname);
        }
    }

    private string RegionCodeToText(string code)
    {
        switch(code)
        {
            case "asia": return "Asia";
            case "au": return "Australia";
            case "cae": return "Canada East";
            case "cn": return "China";
            case "eu": return "Europe";
            case "in": return "India";
            case "jp": return "Japan";
            case "ru": return "Russia";
            case "rue": return "Russia, East";
            case "za": return "South Africa";
            case "sa": return "South America";
            case "kr": return "South Korea";
            case "us": return "USA East";
            case "usw": return "USA West";
            default: return code;
        }
    }
}
