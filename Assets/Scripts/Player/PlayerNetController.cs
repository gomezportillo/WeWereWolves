using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.U2D;

public class PlayerNetController : MonoBehaviourPun, IPunObservable
{
    [SerializeField]
    public int HumanSpeed = 5;

    [SerializeField]
    public int WerewolfSpeed = 8;

    private int Speed;

    [SerializeField]
    public int nameFontSize = 4;

    [SerializeField]
    public string playerNamesCanvasTag = "CanvasPlayerNames";

    [SerializeField]
    public string mainCameraTag = "MainCamera";

    private GameObject mainCameraObject;
    private TMPro.TextMeshProUGUI playerNameText;
    private TMPro.FontStyles masterClientFont = TMPro.FontStyles.Italic;

    private bool isFlipped = false;
    private bool isHuman = true;
    private bool wantsToEvolve = false;
    private bool wantsToAttack = false;

    private Vector3 initialScale;
    private static PlayerNetController instance;

    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        CreateNameText();
        Speed = HumanSpeed;
        initialScale = gameObject.transform.localScale;

        NetworkEventManager.instance.MasterClientSwitched += MasterClientSwitched; // TODO

        if (ThisIsLocalPlayer())
        {
            mainCameraObject = GameObject.FindWithTag(mainCameraTag);
        }
    }

    private void OnDestroy()
    {
        DestroyNameText();
    }

    void Update()
    {
        if (ThisIsLocalPlayer())
        {
            UpdateMovement();
            CheckWantsToEvolve();
            CheckWantsToAttack();
            UpdateCameraPosition();
        }

        UpdateNameTextPosition();
        UpdateFlip();
    }

    private void CheckWantsToEvolve()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            photonView.RPC("Evolve", RpcTarget.All);
        }
    }

    [PunRPC]
    private void Evolve()
    {
        if (isHuman)
        {
            TransformToWerewolf();
            ShowPixelExplosion();
        }
        else
        {
            TransformToHuman();
            ShowPixelExplosion();
        }
    }

    private void CheckWantsToAttack()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isHuman == false)
        {
            photonView.RPC("Attack", RpcTarget.All);
        }
    }

    [PunRPC]
    private void Attack()
    {
        if(!isHuman){
            gameObject.GetComponent<Animator>().SetTrigger("Attack");
        }
    }

    private void ShowPixelExplosion()
    {
        Animator ExplosionAnimator = transform.Find("Explosion").GetComponent<Animator>();
        ExplosionAnimator.SetTrigger("Explode");
    }

    private void TransformToWerewolf()
    {
        RuntimeAnimatorController WerewolfAnimator = Resources.Load("Animations/Werewolf/Werewolf") as RuntimeAnimatorController;
        gameObject.GetComponent<Animator>().runtimeAnimatorController = WerewolfAnimator;
        gameObject.transform.localScale = initialScale * 1.6f;
        Speed = WerewolfSpeed;
        isHuman = false;
    }

    private void TransformToHuman()
    {
        RuntimeAnimatorController HumanAnimator = Resources.Load("Animations/Human/Human") as RuntimeAnimatorController;
        gameObject.GetComponent<Animator>().runtimeAnimatorController = HumanAnimator;
        gameObject.transform.localScale = initialScale;
        Speed = HumanSpeed;
        isHuman = true;
    }

    private void UpdateFlip()
    {
        // can be flipped both by UpdateMovement() and Photon stream
        gameObject.GetComponent<SpriteRenderer>().flipX = isFlipped;
    }

    void UpdateMovement()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        if (inputX != 0 || inputY != 0)
        {
            Vector2 movement = new Vector2(Speed * inputX, Speed * inputY);

            movement *= Time.deltaTime;

            transform.Translate(movement);

            gameObject.GetComponent<Animator>().SetBool("isRunning", true);

            isFlipped = inputX < 0;
        }
        else
        {
            gameObject.GetComponent<Animator>().SetBool("isRunning", false);
        }
    }

    private bool ThisIsRemotePlayer()
    {
        return (photonView.IsMine == false && PhotonNetwork.IsConnected == true);
    }

    private bool ThisIsLocalPlayer()
    {
        return !ThisIsRemotePlayer();
    }

    private bool ThisIsMasterClient()
    {
        return photonView.Owner.IsMasterClient;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(isFlipped);
            stream.SendNext(isHuman);
            stream.SendNext(wantsToEvolve);
            stream.SendNext(wantsToAttack);
        }
        else
        {
            isFlipped = (bool)stream.ReceiveNext();
            isHuman = (bool)stream.ReceiveNext();
            wantsToEvolve = (bool)stream.ReceiveNext();
            wantsToAttack = (bool)stream.ReceiveNext();
        }
    }

    private void UpdateCameraPosition()
    {
        float deltaZ = -10;
        mainCameraObject.transform.position = this.transform.position + new Vector3(0, 0, deltaZ);
    }

    void UpdateNameTextPosition()
    {
        float deltaY = .9f;
        playerNameText.rectTransform.position = gameObject.transform.position + new Vector3(0, deltaY, 0);
    }

    void CreateNameText()
    {
        string playerName = photonView.Owner.NickName;

        GameObject nameTextGO = new GameObject("player_" + playerName);
        playerNameText = nameTextGO.AddComponent<TMPro.TextMeshProUGUI>();

        Canvas canvas = GameObject.FindWithTag(playerNamesCanvasTag).GetComponent<Canvas>();
        playerNameText.transform.SetParent(canvas.transform);

        playerNameText.text = playerName;
        playerNameText.font = Resources.Load("Fonts/gothic_pixel_SDF", typeof(TMPro.TMP_FontAsset)) as TMPro.TMP_FontAsset;
        playerNameText.rectTransform.localScale = new Vector3(1, 1, 1);
        playerNameText.alignment = TMPro.TextAlignmentOptions.Midline;
        playerNameText.rectTransform.localScale = new Vector2(0.1f, 0.1f);
        playerNameText.fontSize = nameFontSize;

        if (ThisIsMasterClient())
        {
            playerNameText.GetComponent<TMPro.TextMeshProUGUI>().fontStyle = masterClientFont;
        }

        UpdateNameTextPosition();
    }
    private void MasterClientSwitched(Player newMaster)
    {
        if (ThisIsMasterClient())
        {
            playerNameText.GetComponent<TMPro.TextMeshProUGUI>().fontStyle = masterClientFont;
        }
        else
        {
            playerNameText.GetComponent<TMPro.TextMeshProUGUI>().fontStyle ^= masterClientFont;
        }
    }

    private void DestroyNameText()
    {
        Destroy(playerNameText.transform.gameObject);
    }

}
