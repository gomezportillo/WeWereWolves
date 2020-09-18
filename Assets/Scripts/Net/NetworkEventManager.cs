using Photon.Realtime;
using System;
using UnityEngine;

public class NetworkEventManager : MonoBehaviour
{
    public static NetworkEventManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(this);
        }
    }

    public event Action<string> ConnectedToMaster;
    public event Action<string> CreatedRoom;
    public event Action<short, string> CreateRoomFailed;
    public event Action<string> JoinedRoom;
    public event Action<short, string> JoinRoomFailed;
    public event Action<Player> PlayerEnteredRoom;
    public event Action<Player> PlayerLeftRoom;
    public event Action<Player> MasterClientSwitched;
    public event Action LeftRoom;

    public void OnConnectedToMaster(string regionCode)
    {
        ConnectedToMaster?.Invoke(regionCode);
    }

    public void OnCreatedRoom(string roomName)
    {
        CreatedRoom?.Invoke(roomName);
    }

    public void OnCreateRoomFailed(short returnCode, string message)
    {
        CreateRoomFailed?.Invoke(returnCode, message);
    }

    public void OnJoinedRoom(string roomName)
    {
        JoinedRoom?.Invoke(roomName);
    }

    public void OnJoinRoomFailed(short returnCode, string message)
    {
        JoinRoomFailed?.Invoke(returnCode, message);
    }

    public void OnPlayerEnteredRoom(Player other)
    {
        PlayerEnteredRoom?.Invoke(other);
    }

    internal void OnPlayerLeftRoom(Player other)
    {
        PlayerLeftRoom?.Invoke(other);
    }

    internal void OnMasterClientSwitched(Player newMasterClient)
    {
        MasterClientSwitched?.Invoke(newMasterClient);
    }

    internal void OnLeftRoom()
    {
        LeftRoom?.Invoke();
    }
}
