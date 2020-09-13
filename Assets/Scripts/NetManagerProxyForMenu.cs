using UnityEngine;
using WebSocketSharp;

public class NetManagerProxyForMenu : MonoBehaviour
{

    [SerializeField]
    public GameObject networkManager;

    [SerializeField]
    public GameObject villajeCodeInput;

    [SerializeField]
    public TMPro.TextMeshProUGUI errorText;

    void Start()
    {
        errorText.text = string.Empty;
    }

    public void ErrorJoiningRoom(string message)
    {
        errorText.text = message;
    }

    public void ErrorCreatingRoom(string message)
    {
        errorText.text = message;
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
            errorText.text = "Village code cannot be empty";
        }
        else
        {
            networkManager.GetComponent<NetworkManager>().JoinRoom(code);
        }
    }
}
