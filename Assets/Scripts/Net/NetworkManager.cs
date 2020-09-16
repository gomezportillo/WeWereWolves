using UnityEngine;
using Photon.Pun;
using WebSocketSharp;

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
        Debug.Log("Connected to master server");

        guiProxyScript.UpdateRegionGUI(PhotonNetwork.CloudRegion);
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

        Debug.Log("Created room: " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);

        Debug.Log("Error creating room: " + message);

        guiProxyScript.ErrorCreatingRoom(message);
    }

    public void JoinRoom(string roomName)
    {
        Debug.Log("Joining room " + roomName + "...");

        PhotonNetwork.JoinRoom(roomName);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        Debug.Log("Joined room: " + PhotonNetwork.CurrentRoom.Name);

        guiProxyScript.JoinedRoom(PhotonNetwork.CurrentRoom.Name);

        PhotonNetwork.NickName = GlobalVariables.GetPlayerName();

        ChangeScene(GlobalVariables.WAITING_ROOM_SCENE);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);

        Debug.Log("Error joining room: " + message);

        guiProxyScript.ErrorJoiningRoom("This village does not exist");
    }

    public void ChangeScene(string sceneName)
    {
        Debug.Log("Changing scene to: " + sceneName);

        PhotonNetwork.LoadLevel(sceneName);
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player other)
    {
        Debug.LogFormat("New player: " + other.NickName);
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player other)
    {
        Debug.LogFormat("Player left: " + other.NickName);
    }

    public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    {
        Debug.LogFormat("Old master cliente left. New one is: " + newMasterClient.NickName);
    }

    public string GetCurrentRoom()
    {
        return PhotonNetwork.CurrentRoom.Name;
    }
}
