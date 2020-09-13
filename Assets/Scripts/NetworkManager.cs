using UnityEngine;
using Photon.Pun;
using WebSocketSharp;

public class NetworkManager : MonoBehaviourPunCallbacks
{

    [SerializeField]
    public int randomRoomNameLenght = 4;
    private const string GLYPHS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    [SerializeField]
    public GameObject menuProxyObject;
    private NetManagerProxyForMenu menuProxyScript;

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
        PhotonNetwork.ConnectUsingSettings();
        //PhotonNetwork.ConnectToRegion("eu");

        menuProxyScript = menuProxyObject.GetComponent<NetManagerProxyForMenu>();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("Connected to master server");
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

        PhotonNetwork.CreateRoom(roomName);

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

        menuProxyScript.ErrorCreatingRoom(message);

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
        Debug.Log("Cloud region: " + PhotonNetwork.CloudRegion);

        ChangeScene("Game");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);

        Debug.Log("Error joining room: " + message);

        menuProxyScript.ErrorJoiningRoom(message);
    }

    public void ChangeScene(string sceneName)
    {
        PhotonNetwork.LoadLevel(sceneName);

        Debug.Log("Changing scene to: " + sceneName);
    }
}
