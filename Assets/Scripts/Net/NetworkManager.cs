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

    private string gameVersion = "0.1";

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
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = gameVersion;

        PhotonNetwork.ConnectUsingSettings();
    }

    private void OnDestroy()
    {
        PhotonNetwork.Disconnect();
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

        RoomOptions roomOptions = new RoomOptions
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

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        NetworkEventManager.instance.OnLeftRoom();
    }

    public override void OnPlayerEnteredRoom(Player other)
    {
        base.OnPlayerEnteredRoom(other);
        NetworkEventManager.instance.OnPlayerEnteredRoom(other);
    }

    public override void OnPlayerLeftRoom(Player other)
    {
        base.OnPlayerLeftRoom(other);
        NetworkEventManager.instance.OnPlayerLeftRoom(other);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        base.OnMasterClientSwitched(newMasterClient);
        NetworkEventManager.instance.OnMasterClientSwitched(newMasterClient);
    }

    // Non-events
    public void JoinRoom(string roomName)
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRoom(roomName);
        }
        else
        {
            NetworkEventManager.instance.OnJoinRoomFailed(-1, "Not connected to master server");
        }
    }

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

    public void InstantiateNewPlayer(string prefabName)
    {
        PhotonNetwork.Instantiate(prefabName, new Vector3(0f, 0f, 0f), Quaternion.identity, 0);
    }
}
