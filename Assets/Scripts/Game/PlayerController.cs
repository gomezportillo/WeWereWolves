using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.U2D;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    public int speed = 100;

    [SerializeField]
    public int nameFontSize = 4;

    [SerializeField]
    public Canvas canvas;

    private TMPro.TextMeshProUGUI playerNameText;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb2d;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();

        CreateNameText();

        Debug.Log("Nombre de la room " + NetworkManager.instance.GetCurrentRoom());
    }

    void Update()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        if (inputX != 0 || inputY != 0)
        {
            Vector2 movement = new Vector2(speed * inputX, speed * inputY);

            movement *= Time.deltaTime;

            transform.Translate(movement);

            //Debug.Log(transform.position);

            animator.SetBool("isRunning", true);

            spriteRenderer.flipX = inputX < 0;

            if (rb2d.velocity.magnitude < 5)
            {
                rb2d.velocity = new Vector2(0, 0);
            }
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
        
        UpdateNameTextPosition();
    }

    void CreateNameText()
    {
        string playerName = GlobalVariables.GetPlayerName();

        GameObject nameTextGO = new GameObject("text_" + playerName);
        playerNameText = nameTextGO.AddComponent<TMPro.TextMeshProUGUI>();

        //playerNameText = new TMPro.TextMeshProUGUI();
        playerNameText.transform.SetParent(canvas.transform);

        playerNameText.text = GlobalVariables.GetPlayerName();
        playerNameText.font = Resources.Load("gothic_pixel SDF", typeof(TMPro.TMP_FontAsset)) as TMPro.TMP_FontAsset;
        playerNameText.rectTransform.localScale = new Vector3(1, 1, 1);
        playerNameText.alignment = TMPro.TextAlignmentOptions.Midline;
        playerNameText.rectTransform.localScale = new Vector2(0.1f, 0.1f);

        playerNameText.fontSize = nameFontSize;

        UpdateNameTextPosition();
    }

    void UpdateNameTextPosition()
    {
        float Ydelta = .9f;
        playerNameText.rectTransform.position = gameObject.transform.position + new Vector3(0, Ydelta, 0);
    }
}