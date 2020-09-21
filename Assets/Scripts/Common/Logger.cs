using System.Diagnostics;
using UnityEngine;
using Photon.Realtime;

public class Logger : MonoBehaviour
{
    public static Logger instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        NetworkEventManager.instance.ConnectedToMaster += this.ConnectedToMaster;
        NetworkEventManager.instance.CreatedRoom += this.CreatedRoom;
        NetworkEventManager.instance.CreateRoomFailed += this.CreatedRoomFailed;
        NetworkEventManager.instance.JoinedRoom += this.JoinedRoom;
        NetworkEventManager.instance.JoinRoomFailed += this.JoinRoomFailed;
        NetworkEventManager.instance.PlayerEnteredRoom += this.PlayerEnteredRoom;
        NetworkEventManager.instance.PlayerLeftRoom += this.PlayerLeftRoom;
        NetworkEventManager.instance.MasterClientSwitched += this.MasterClientSwitched;
        NetworkEventManager.instance.LeftRoom += this.LeftRoom;
    }

    void OnDestroy()
    {
        // unsubscribe listeners 
    }

    void ConnectedToMaster(string region)
    {
        LogInfo("Connected to master on region: " + region);
    }

    void CreatedRoom(string roomName)
    {
        LogInfo("Created room: " + roomName);
    }

    void CreatedRoomFailed(short returnCode, string message)
    {
        LogError("Error creating room: " + message);
    }

    void JoinedRoom(string roomName)
    {
        LogInfo("Joined room: " + roomName);
    }

    void JoinRoomFailed(short returnCode, string message)
    {
        LogError("Error joining room: " + message);
    }

    void PlayerEnteredRoom(Player other)
    {
        LogInfo("Player entered room: " + other.NickName);
    }
    void PlayerLeftRoom(Player other)
    {
        LogInfo("Player left room: " + other.NickName);
    }
    void MasterClientSwitched(Player newMasterClient)
    {
        LogInfo("Master client switched. New one: " + newMasterClient.NickName);
    }

    void LeftRoom()
    {
        LogInfo("Room left");
    }

    // Log methods
    public void LogInfo(string message)
    {
        UnityEngine.Debug.Log(MessageHeader() + message);
    }

    public void LogError(string message)
    {
        UnityEngine.Debug.LogError(MessageHeader() + message);
    }

    public void LogWarning(string message)
    {
        UnityEngine.Debug.LogWarning(MessageHeader() + message);
    }

    private string MessageHeader()
    {
        const int DEPTH = 2;
        StackTrace stackTrace = new StackTrace();
        string callingMethod = stackTrace.GetFrame(DEPTH).GetMethod().Name;
        //string callingClass = stackTrace.GetFrame(DEPTH).GetType().Name;

        return "[" + callingMethod + "] ";
    }

}
