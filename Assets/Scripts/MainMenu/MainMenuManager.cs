using Photon.Pun;
using UnityEngine;
using WebSocketSharp;

public class MainMenuManager : MonoBehaviour
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

    private const string debugRoomName = "XYZ";

    void Start()
    {
        errorText.text = string.Empty;
        playerNameInput.text = GlobalVariables.GenerateRandomPlayerName();

        // [DEBUG]
        villajeCodeInput.text = debugRoomName;

        NetworkEventManager.instance.ConnectedToMaster += UpdateRegionText;
        NetworkEventManager.instance.JoinRoomFailed += ErrorJoiningRoom;
        NetworkEventManager.instance.CreateRoomFailed += ErrorCreatingRoom;
    }

    private void OnDestroy()
    {
        NetworkEventManager.instance.ConnectedToMaster -= UpdateRegionText;
        NetworkEventManager.instance.JoinRoomFailed -= ErrorJoiningRoom;
        NetworkEventManager.instance.CreateRoomFailed -= ErrorCreatingRoom;
    }

    public void ErrorJoiningRoom(short returnCode, string message)
    {
        errorText.text = message;
    }

    public void CreateRoom()
    {
        //NetworkManager.instance.CreateRoom(null);
        NetworkManager.instance.CreateRoom(debugRoomName);
    }

    public void ErrorCreatingRoom(short returnCode, string message)
    {
        errorText.text = message;
    }

    public void UpdateRegionText(string regionCode)
    {
        regionText.text = "Region: " + RegionCodeToText(regionCode);
    }

    public void ReadInputAndJoinRoom()
    {
        string code = villajeCodeInput.text.ToUpper();

        if (code.IsNullOrEmpty())
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
        if (nickname.IsNullOrEmpty())
        {
            // show error and disable back button
        }
        else
        {
            NetworkManager.instance.SetPlayerNickname(nickname);
        }
    }

    private string RegionCodeToText(string code)
    {
        switch (code)
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
