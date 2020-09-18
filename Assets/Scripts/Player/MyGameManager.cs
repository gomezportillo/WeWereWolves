using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGameManager : MonoBehaviour
{
    [SerializeField]
    public GameObject playerPrefab;
    
    public static MyGameManager instance;

    void Start()
    {
        instance = this;

        if (playerPrefab == null)
        {
            Logger.instance.LogError("Missing player prefab");
        }
        else
        {
            NetworkManager.instance.InstantiateNewPlayer(this.playerPrefab.name);
        }
    }
}
