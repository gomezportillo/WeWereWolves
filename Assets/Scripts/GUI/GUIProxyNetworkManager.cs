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
    }

    public void ErrorJoiningRoom(string message)
    {
        errorText.text = message;
    }

    public void JoinedRoom(string roomCode)
    {
        GlobalVariables.roomCode = roomCode;
        GlobalVariables.SetPlayerName(playerNameInput.text);
    }

    public void ErrorCreatingRoom(string message)
    {
        errorText.text = message;
    }

    public void CreateRoom()
    {
        //string code = networkManager.GetComponent<NetworkManager>().CreateRoom(null);
        NetworkManager.instance.CreateRoom(null);
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
            //networkManager.GetComponent<NetworkManager>().JoinRoom(code);
            NetworkManager.instance.JoinRoom(code);
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
