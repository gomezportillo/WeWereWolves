using UnityEngine;
using Photon.Realtime;

public class WaitingRoomGUIManager : MonoBehaviour
{
    [SerializeField]
    public TMPro.TextMeshProUGUI playerNameText;

    [SerializeField]
    public TMPro.TextMeshProUGUI roomCodeText;

    [SerializeField]
    public TMPro.TextMeshProUGUI waitingForPlayersText;
    private string waitingForPlayersBaseString;

    private void Awake()
    {
        NetworkEventManager.instance.PlayerEnteredRoom += UpdateWaitingForPlayersMessage;
        NetworkEventManager.instance.PlayerEnteredRoom += UpdateWaitingForPlayersMessage;
    }

    private void OnDestroy()
    {
        NetworkEventManager.instance.PlayerEnteredRoom -= UpdateWaitingForPlayersMessage;
        NetworkEventManager.instance.PlayerEnteredRoom -= UpdateWaitingForPlayersMessage;
    }

    private void Start()
    {
        playerNameText.text = NetworkManager.instance.GetPlayerNickName();
        roomCodeText.text = "Village " + NetworkManager.instance.GetCurrentRoomName();
        waitingForPlayersBaseString = waitingForPlayersText.text;

        UpdateWaitingForPlayersMessage(null);
    }

    void UpdateWaitingForPlayersMessage(Player newPlayer)
    {
        int currentNumberOfPlayers = NetworkManager.instance.GetCurrentNumberOfPlayers();
        string playerStatistics = "(" + currentNumberOfPlayers + "/" + GlobalVariables.MIN_NUMBER_PLAYERS + ")";

        waitingForPlayersText.text = waitingForPlayersBaseString + playerStatistics;
    }
}
