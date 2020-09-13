using UnityEngine;
using WebSocketSharp;

public class NetManagerProxyForMenu : MonoBehaviour
{

    [SerializeField]
    public GameObject networkManager;

    [SerializeField]
    public GameObject villajeCodeInput;

    void Start()
    {
    }

    public void ErrorJoiningRoom(string message)
    {
        // escribirlo en la gui
    }

    public void ErrorCreatingRoom(string message)
    {
        // escribirlo en la gui
    }

    public void CreateRoom()
    {
        string code = networkManager.GetComponent<NetworkManager>().CreateRoom(null);
    }

    public void ReadInputAndJoinRoom()
    {
        string code = villajeCodeInput.GetComponent<TMPro.TextMeshProUGUI>().text;

        Debug.Log("CODE '" + code + "'");
        if (code == null )
        {
            Debug.Log("es null");
        }

        if (code == "")
        {
            Debug.Log("es ''");
        }
        if (code == string.Empty)
        {
            Debug.Log("es empty");
        }

        if (string.IsNullOrEmpty(code))
        {
            Debug.Log("null o vacia");
        }

        if (code == null || code == string.Empty)
        {
            // escribilo en la gui
            Debug.Log("The village code cannot be empy");
        }
        else
        {
            networkManager.GetComponent<NetworkManager>().JoinRoom(code);
        }
    }
}
