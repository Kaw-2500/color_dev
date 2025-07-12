//using UnityEngine;

//public class PlayerBase : MonoBehaviour
//{
//    [SerializeField] private PlayerColorManager playerColorManager;
//    [SerializeField] private PlayerMove playerMove;
//    [SerializeField] private PlayerHitDamage playerHitDamage;
//    [SerializeField] private PlayerAttack playerAttack;
//    [SerializeField] private InputManager inputManager;
//    [SerializeField] private CheckPlayerstatus checkPlayerStatus;
//    [SerializeField] private Talksystem talksystem;

//    private Rigidbody2D rb2d;
//    private SpriteRenderer spriteRenderer;

//    //状態フラグたち
//    public bool IsColorChangeCool { get; set; } = false;

//    public bool IsGround { get; private set; } = true;
//    public bool IsJump { get; private set; } = false;

//    public float GroundYpos { get; private set; } = 0f;

//    void Start()
//    {
//        rb2d = GetComponent<Rigidbody2D>();
//        if (rb2d == null) Debug.LogError("PlayerBase: Rigidbody2Dがアタッチされていません。");

//        spriteRenderer = GetComponent<SpriteRenderer>();
//        if (spriteRenderer == null) Debug.LogError("PlayerBase: SpriteRendererがアタッチされていません。");

        
//        if (playerColorManager == null) Debug.LogError("PlayerBase: PlayerColorManagerを設定してください。");
        
//        if (playerMove == null) Debug.LogError("PlayerBase: PlayerMoveを設定してください。");
        
//        if (inputManager == null) Debug.LogError("PlayerBase: InputManagerを設定してください。");
        
//        if (checkPlayerStatus == null) Debug.LogError("PlayerBase: CheckPlayerstatusを設定してください。");
        
//        if (talksystem == null) Debug.LogError("PlayerBase: Talksystemを設定してください。");
//    }

//    void Update()
//    {
//        if (talksystem.isTalking) return;

//        UpdateGroundStatus();

//        if (!IsColorChangeCool)
//        {
//            TryColorChange();
//        }
//    }

//    void FixedUpdate()
//    {
//        if (talksystem.isTalking) return;

//        TryMove();
//    }

//    private void UpdateGroundStatus()
//    {
//        GroundYpos = checkPlayerStatus.GroundYposCheck();

//        bool nearGround = checkPlayerStatus.CheckNearFloor(GroundYpos);

//        IsJump = Mathf.Abs(rb2d.linearVelocity.y) > 0.05f;

//        if (nearGround)
//        {
//            IsJump = false;
//            IsGround = true;
//        }
//        else if (!nearGround && rb2d.linearVelocity.y < 0)
//        {
//            IsGround = false;
//        }
//    }

//    private void TryMove()
//    {
//        float input = inputManager.Horizontal;

//        var data = playerColorManager.GetCurrentData();

//        TryJump(data);

//        if (input != 0f && Mathf.Abs(rb2d.linearVelocity.x) < data.maxSpeed)
//        {
//            playerMove.HandleMove(data.runForce, data.maxSpeed, data.jumpForce, data.gravityScale, rb2d, input);
//        }
//    }

//    private void TryJump(PlayerColorDataExtended data)
//    {
//        if (!IsGround) return;

//        if (inputManager.JumpPress)
//        {
//            IsJump = true;
//            IsGround = false;
//            playerMove.PlayerJump(data.jumpForce, rb2d);
//        }
//    }

//    private void TryColorChange()
//    {
//        var data = playerColorManager.GetCurrentData();

//        if (inputManager.ChangeToRed)
//            playerColorManager.ChangeColor(PlayerColorManager.PlayerColorState.Red, data.chargeCoolTime);
//        else if (inputManager.ChangeToBlue)
//            playerColorManager.ChangeColor(PlayerColorManager.PlayerColorState.Blue, data.chargeCoolTime);
//        else if (inputManager.ChangeToGreen)
//            playerColorManager.ChangeColor(PlayerColorManager.PlayerColorState.Green, data.chargeCoolTime);
//    }

//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        if (IsFloorTag(collision))
//            IsGround = true;
//    }

//    private void OnTriggerStay2D(Collider2D collision)
//    {
//        if (IsFloorTag(collision))
//            IsGround = true;
//    }

//    private void OnTriggerExit2D(Collider2D collision)
//    {
//        if (IsFloorTag(collision))
//            IsGround = false;
//    }

//    private bool IsFloorTag(Collider2D collision)
//    {
//        return collision.CompareTag("Redfloor") ||
//               collision.CompareTag("Bluefloor") ||
//               collision.CompareTag("Greenfloor") ||
//               collision.CompareTag("naturalfloor");
//    }
//}
