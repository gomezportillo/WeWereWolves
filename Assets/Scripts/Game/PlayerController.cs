using System.Collections;
using UnityEngine;
using UnityEngine.U2D;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    public int speed = 100;

    private TMPro.TextMeshProUGUI playerNameText;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb2d;

    private void Start()
    {
        // create name
        CreateNameTag();

        animator = gameObject.GetComponent<Animator>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        if ( inputX != 0 || inputY != 0) {
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


    }
    void CreateNameTag()
    {
        playerNameText = new TMPro.TextMeshProUGUI();
        playerNameText = gameObject.AddComponent<TMPro.TextMeshProUGUI>();
        playerNameText.text = "RandomPeasant";
        playerNameText.font = Resources.Load("gothic_pixel SDF", typeof(TMPro.TMP_FontAsset)) as TMPro.TMP_FontAsset;
        playerNameText.fontSize = 3;
    }
}