using Photon.Pun;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.U2D;

public class PlayerController : MonoBehaviourPun, IPunObservable
{
    [SerializeField]
    public int speed = 100;

    [SerializeField]
    public int nameFontSize = 4;

    [SerializeField]
    public string playerNamesCanvasTag = "CanvasPlayerNames";

    [SerializeField]
    public string mainCameraTag = "MainCamera";

    private GameObject mainCameraObject;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private TMPro.TextMeshProUGUI playerNameText;

    private bool isFlipped = false;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        CreateNameText();

        if (IsRemotePlayer())
        {
            return;
        }

        mainCameraObject = GameObject.FindWithTag(mainCameraTag);
    }


    void Update()
    {
        if (!IsRemotePlayer())
        { 
            UpdateMovement();
            UpdateCameraPosition();
        }
        UpdateNameTextPosition();
        UpdateFlip();
    }

    private void UpdateFlip()
    {
        // updatable both by UpdateMovement() and photon 
        spriteRenderer.flipX = isFlipped;
    }

    void UpdateMovement()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        if (inputX != 0 || inputY != 0)
        {
            Vector2 movement = new Vector2(speed * inputX, speed * inputY);

            movement *= Time.deltaTime;

            transform.Translate(movement);

            animator.SetBool("isRunning", true);

            isFlipped = inputX < 0;
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
    }

    private bool IsRemotePlayer()
    {
        return (photonView.IsMine == false && PhotonNetwork.IsConnected == true);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(isFlipped);
        }
        else
        {
            isFlipped = (bool)stream.ReceiveNext();
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
        string playerName = GlobalVariables.GetPlayerName();

        GameObject nameTextGO = new GameObject("text_" + playerName);
        playerNameText = nameTextGO.AddComponent<TMPro.TextMeshProUGUI>();

        Canvas canvas = GameObject.FindWithTag(playerNamesCanvasTag).GetComponent<Canvas>();
        playerNameText.transform.SetParent(canvas.transform);

        playerNameText.text = GlobalVariables.GetPlayerName();
        playerNameText.font = Resources.Load("gothic_pixel SDF", typeof(TMPro.TMP_FontAsset)) as TMPro.TMP_FontAsset;
        playerNameText.rectTransform.localScale = new Vector3(1, 1, 1);
        playerNameText.alignment = TMPro.TextAlignmentOptions.Midline;
        playerNameText.rectTransform.localScale = new Vector2(0.1f, 0.1f);

        playerNameText.fontSize = nameFontSize;

        UpdateNameTextPosition();
    }

}
