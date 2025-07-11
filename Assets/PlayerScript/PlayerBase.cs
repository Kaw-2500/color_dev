using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    [SerializeField] PlayerColorManager playerColorManager;
    [SerializeField] PlayerMove playerMove;
    [SerializeField] PlayerHitDamage playerHitDamage;
    [SerializeField] PlayerAttack playerAttack;

    [SerializeField] private Talksystem talksystem;

    [SerializeField] private PlayerColorDataExtended redData;
    [SerializeField] private PlayerColorDataExtended blueData;
    [SerializeField] private PlayerColorDataExtended greenData;

    private PlayerColorDataExtended currentColorData;

    private Rigidbody2D rb2d;
    private SpriteRenderer spriteRenderer;

    private bool IsGround = false;
    private bool IsJump = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        if (rb2d == null) Debug.Log("Playerにrigidbody2dをアタッチしてください");

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) Debug.Log("PlayerにSpriteRendererをアタッチしてください");


    }

    // Update is called once per frame
    void Update()
    {

    }
    private void FixedUpdate()
    {
        TryMove();
    }
    void TryMove()
    {
        if (talksystem.isTalking == true) return;


        float input = (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) ? 1f :
                   (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) ? -1f : 0f;//inputの値で、加速度を左右に分けます。

        if (input != 0 && Mathf.Abs(rb2d.linearVelocity.x) < currentColorData.maxSpeed)
        {
            playerMove.HandleMove(currentColorData.runForce,
                        currentColorData.maxSpeed,
                        currentColorData.jumpForce,
                        currentColorData.gravityScale,
                        rb2d,
                        input
                        );
        }
          


        if (!IsGround || IsJump) return;
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            IsJump = true;
            IsGround = false;

            playerMove.PlayerJump(
                 currentColorData.jumpForce,
                 rb2d
                 );
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsFloorTag(collision))
        {
            IsJump = false;
            IsGround = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (IsFloorTag(collision))
        {
            IsGround = false;
            IsJump = true;  // 地面から離れたらジャンプ状態に戻す
        }
    }
    private bool IsFloorTag(Collider2D collision)
    {
        string[] floorTags = { "Redfloor", "Bluefloor", "Greenfloor", "naturalfloor" };//床のタグ増やしたらここをいじる
        foreach (var tag in floorTags)
        {
            if (collision.CompareTag(tag)) return true;
        }
        return false;
    }
}