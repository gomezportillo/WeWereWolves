using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingRoomGUILoader : MonoBehaviour
{
    [SerializeField]
    public TMPro.TextMeshProUGUI playerNameText;

    [SerializeField]
    public TMPro.TextMeshProUGUI roomCodeText;

    [SerializeField]
    public TMPro.TextMeshProUGUI waitingForPlayersText;
    private string waitingForPlayersBaseString;

    private void Start()
    {
        playerNameText.text = GlobalVariables.GetPlayerName();

        roomCodeText.text = "Village " + GlobalVariables.roomName;

        waitingForPlayersBaseString = waitingForPlayersText.text;
    }
}
