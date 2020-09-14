using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGUILoader : MonoBehaviour
{
    [SerializeField]
    public TMPro.TextMeshProUGUI playerNameText;

    [SerializeField]
    public TMPro.TextMeshProUGUI roomCodeText;

    private void Start()
    {
        string playerName = GlobalVariables.playerName;
        if (string.IsNullOrEmpty(playerName))
        {
            playerName = "Peasant" + Random.Range(0, 9999);
        }

        playerNameText.text = playerName;


        roomCodeText.text = GlobalVariables.roomCode;
    }
}
