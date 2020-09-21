using UnityEngine;
using Photon.Pun;
using WebSocketSharp;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{

    [SerializeField]
    public int randomRoomNameLenght = 4;
    private const string GLYPHS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    private string gameVersion = "0.1";

    public static NetworkManager instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
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

    public int GetCurrentNumberOfPlayers()
    {
        return PhotonNetwork.CurrentRoom.PlayerCount;
    }

    public Player[] GetListOfPlayers()
    {
        return PhotonNetwork.PlayerList;
    }

    private void OnDestroy()
    {
        PhotonNetwork.Disconnect();
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
            MaxPlayers = GlobalVariables.MAX_NUMBER_PLAYERS,
        };

        PhotonNetwork.CreateRoom(roomName, roomOptions);

        return roomName;
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    #region Pun events
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        string regionCode = PhotonNetwork.CloudRegion;
        NetworkEventManager.instance.OnConnectedToMaster(regionCode);
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

    #endregion

    #region Remote operations
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
        if (PhotonNetwork.IsMasterClient) { 
            //PhotonNetwork.CurrentRoom.IsOpen = false;
            Logger.instance.LogInfo("Changing scene to: " + sceneName);
            PhotonNetwork.LoadLevel(sceneName);
        }
    }

    public string GetCurrentRoomName()
    {
        return PhotonNetwork.CurrentRoom.Name;
    }

    public void SetPlayerNickname(string nickname)
    {
        Logger.instance.LogInfo("New name: " + nickname);
        PhotonNetwork.NickName = nickname;
    }

    public string GetPlayerNickName()
    {
        return PhotonNetwork.NickName;
    }

    public GameObject InstantiateNewPlayer(string prefabName, Vector3 position)
    {

        return PhotonNetwork.Instantiate(prefabName, position, Quaternion.identity, 0);
    }

    public void Destroy(GameObject go)
    {
        PhotonNetwork.Destroy(go);
    }

    #endregion
}
