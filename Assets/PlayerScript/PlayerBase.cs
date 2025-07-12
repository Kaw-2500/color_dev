using Unity.Android.Gradle.Manifest;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    [SerializeField] PlayerColorManager playerColorManager;
    [SerializeField] PlayerMove playerMove;
    [SerializeField] PlayerHitDamage playerHitDamage;
    [SerializeField] PlayerAttack playerAttack;
    [SerializeField] private InputManager inputManager;
    [SerializeField] CheckPlayerstatus checkPlayerStatus;

    [SerializeField] private Talksystem talksystem;

    public float groundypos = 0f; 

    public bool IsColorChangeCool = false;

    private Rigidbody2D rb2d;
    private SpriteRenderer spriteRenderer;

   [SerializeField] private bool IsGround = true;
   [SerializeField] private bool IsJump = false;

    public float GroundYpos;
 
    bool NearGround = false; // 近接判定のフラグ

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        if (rb2d == null) Debug.Log("Player��rigidbody2d���A�^�b�`���Ă�������");

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) Debug.Log("Player��SpriteRenderer���A�^�b�`���Ă�������");
    }

    // Update is called once per frame
    void Update()
    {
        TryColorChange();
     
       groundypos = checkPlayerStatus.GroundYposCheck(); 

        NearGround = checkPlayerStatus.CheckNearFloor(groundypos);
        IsJump = Mathf.Abs(rb2d.linearVelocity.y) > 0.05f; //閾値を設定して、y軸の速度が小さい場合はジャンプしていないと判断する
        if ((NearGround))
        {
            IsJump = false;
            IsGround = true; // 近接判定で地面にいると判断
        }
    }
    private void FixedUpdate()
    {
        TryMove();
    }
    void TryMove()
    {
        if (talksystem != null && talksystem.isTalking) return;

        float input = inputManager.Horizontal; // InputManagerから

        var data = playerColorManager.GetCurrentData();

        TryJump(data); 

        if (input != 0 && Mathf.Abs(rb2d.linearVelocity.x) < data.maxSpeed)
        {
            playerMove.HandleMove(
                data.runForce,
                data.maxSpeed,
                data.jumpForce,
                data.gravityScale,
                rb2d,
                input
            );
        }        
    }

    void TryJump(PlayerColorDataExtended data)
    {
        if (!IsGround) return;
        if (inputManager.JumpPress)
        {
            IsJump = true;
            IsGround = false;

            playerMove.PlayerJump(
            data.jumpForce,
            rb2d
            );
        }
    }

    void TryColorChange()
    {
        if (talksystem != null && talksystem.isTalking) return;
        if (IsColorChangeCool) return;

        var data = playerColorManager.GetCurrentData();

        if (inputManager.ChangeToRed)
        playerColorManager.ChangeColor(PlayerColorManager.PlayerColorState.Red,data.chargeCoolTime);

        else if (inputManager.ChangeToBlue)
            playerColorManager.ChangeColor(PlayerColorManager.PlayerColorState.Blue,data.chargeCoolTime);
        else if (inputManager.ChangeToGreen)
            playerColorManager.ChangeColor(PlayerColorManager.PlayerColorState.Green,data.chargeCoolTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsFloorTag(collision))
        {
            //IsJump = false;
            IsGround = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (IsFloorTag(collision))
        {
            IsGround = true;
           
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (IsFloorTag(collision))
        {
            IsGround = false;
        }
    }
    private bool IsFloorTag(Collider2D collision)
    {
        string[] floorTags = { "Redfloor", "Bluefloor", "Greenfloor", "naturalfloor" };
        foreach (var tag in floorTags)
        {
            if (collision.CompareTag(tag)) return true;
        }
        return false;
    }
}