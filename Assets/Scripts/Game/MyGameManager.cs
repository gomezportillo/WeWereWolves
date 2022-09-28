using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
//using Hashtable = ExitGames.Client.Photon.Hashtable;

public class MyGameManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    public string playerPrefabName = "Player";

    public static MyGameManager instance;
    private GameObject playerGO;

    void Start()
    {
        instance = this;
        InstantiateNewPlayer();
    }

    private void InstantiateNewPlayer()
    {
        float randomX = Random.Range(-1f, 1f);
        float randomY = Random.Range(-1f, 1f);
        Vector3 position = new Vector3(randomX, randomY, 0f);

        playerGO = NetworkManager.instance.InstantiateNewPlayer(playerPrefabName, position);
    }

    private void DestroyPlayer()
    {
        NetworkManager.instance.Destroy(playerGO); // pun deletes player on exit
    }

    public void LeaveRoom()
    {
        NetworkManager.instance.LeaveRoom();
    }


    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(GlobalVariables.MAIN_MENU_SCENE);
    }
}
