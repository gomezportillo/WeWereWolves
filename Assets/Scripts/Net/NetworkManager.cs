using UnityEngine;
using Photon.Pun;
using WebSocketSharp;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class NetworkManager : MonoBehaviourPunCallbacks
{

    [SerializeField]
    public int randomRoomNameLenght = 4;
    private const string GLYPHS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    [SerializeField]
    public GameObject guiProxyObject;
    private GUIProxyNetworkManager guiProxyScript;

    public static NetworkManager instance;

    void Awake()
    {
        if (instance != null && instance != this) {
            gameObject.SetActive(false);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        guiProxyScript = guiProxyObject.GetComponent<GUIProxyNetworkManager>();

        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        string regionCode = PhotonNetwork.CloudRegion;
        NetworkEventManager.instance.OnConnectedToMaster(regionCode);
    }

    public string CreateRoom(string roomName)
    {
        if (roomName.IsNullOrEmpty())
        {
            for (int i = 0; i < randomRoomNameLenght; i++)
            {
                roomName += GLYPHS[Random.Range(0, GLYPHS.Length)];
            }
        }

        Photon.Realtime.RoomOptions roomOptions = new Photon.Realtime.RoomOptions
        {
            IsVisible = false,
            MaxPlayers = GlobalVariables.maxNumberPlayers,
        };

        PhotonNetwork.CreateRoom(roomName, roomOptions);
        
        return roomName;
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();

        string roomName = PhotonNetwork.CurrentRoom.Name;
        NetworkEventManager.instance.OnCreatedRoom(roomName);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        NetworkEventManager.instance.OnCreateRoomFailed(returnCode, message);
    }

    public void JoinRoom(string roomName)
    {
        Logger.instance.LogInfo("Joining room " + roomName + "...");

        PhotonNetwork.JoinRoom(roomName);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        string roomName = PhotonNetwork.CurrentRoom.Name;
        NetworkEventManager.instance.OnJoinedRoom(roomName);

        ChangeScene(GlobalVariables.WAITING_ROOM_SCENE);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        NetworkEventManager.instance.OnJoinRoomFailed(returnCode, message);
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player other)
    {
        base.OnPlayerEnteredRoom(other);
        NetworkEventManager.instance.OnPlayerEnteredRoom(other);
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player other)
    {
        base.OnPlayerLeftRoom(other);
        NetworkEventManager.instance.OnPlayerLeftRoom(other);
    }

    public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    {
        base.OnMasterClientSwitched(newMasterClient);
        NetworkEventManager.instance.OnMasterClientSwitched(newMasterClient);
    }

    // Non-events
    public void ChangeScene(string sceneName)
    {
        Logger.instance.LogInfo("Changing scene to: " + sceneName);
        PhotonNetwork.LoadLevel(sceneName);
    }

    public string GetCurrentRoom()
    {
        return PhotonNetwork.CurrentRoom.Name;
    }

    public void SetPlayerNickname(string nickname)
    {
        Logger.instance.LogInfo("New name: " + nickname);
        PhotonNetwork.NickName = nickname;
    }
}
