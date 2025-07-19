//using System.Collections;
//using UnityEngine;

//public class Playeroperate : MonoBehaviour
//{
//    public float PlayerHp = 100f;

//    [SerializeField] private float flywindHorizontalAmount = 3f; // X軸吹き飛ばし量
//    [SerializeField] private float flywindVerticalAmount = 5f;   // Y軸吹き飛ばし量


//    [SerializeField] private float HitDamageOnfloor = 1f;
//    [SerializeField] private float Defaultgravity = 10f;
//    [SerializeField] private float Lightgravity = 7f;
//    [SerializeField] private float AddRunforceply;
//    [SerializeField] private float AddJumpforceply;
//    [SerializeField] private float MaxRunforceply;

//    [SerializeField] private bool IstouchGreen;
//    [SerializeField] private bool IstouchBlue;
//    [SerializeField] private bool IstouchRed;

//    [SerializeField] private bool IsGreen;
//    [SerializeField] private bool IsBlue;
//    [SerializeField] private bool IsRed;

//    private bool isRunningHitDamageCoroutine = false;

//    [SerializeField] private EventManager eventManager;

//    private Rigidbody2D rb2d;
//    [SerializeField] private bool IsGround = false;
//    [SerializeField] private bool IsJump = false;

//    [SerializeField] private float MaxGreenSpeed;
//    [SerializeField] private float GreemAddRunforceply;
//    [SerializeField] private float MaxBlueSpeed;
//    [SerializeField] private float BlueAddRunforceply;
//    [SerializeField] private float MaxRedSpeed;
//    [SerializeField] private float RedAddRunforceply;

//    [SerializeField] private float GreenJumpSpeed;
//    [SerializeField] private float BlueJumpSpeed;
//    [SerializeField] private float RedJumpSpeed;

//    [SerializeField] private Talksystem talksystem;

//    public bool IsFinishColorChangeCoolTime = false;
//    public bool IsColorChangeCoolTime = false;
//    [SerializeField] public ChargeBar chargeBar;

//    public GameObject Redfire;
//    public GameObject Bluethunder;
//    public GameObject GreenCross;

//    public float groundypos;

//    private PlayerColorState ColorPlayer = PlayerColorState.Red;

//    bool Isfly = false;
//    private enum PlayerColorState
//    {
//        Green,
//        Blue,
//        Red,
//    }

//    void Start()
//    {
//        GetComponent<SpriteRenderer>().color = Color.red;// 初期色を赤に設定
//        IsRed = true;//初期色の常態に合わせること
//        IsFinishColorChangeCoolTime = true;
//        Application.targetFrameRate = 60;
//        rb2d = GetComponent<Rigidbody2D>();
//    }

//    void Update()
//    {
//        GroundYposCheck();
//        if (talksystem != null && talksystem.isTalking) return;// talksystemは攻撃せず、色を変えない

//        changeColor();
//        CheckAttack();
//    }

//    void FixedUpdate()
//    {
//        if (talksystem != null && talksystem.isTalking) return;//talksystem起動時は移動しない
//        Move();
//    }

//    private void Move()
//    {
//        if (ColorPlayer == PlayerColorState.Red)
//            HandleMovementForColor(MaxRedSpeed, RedAddRunforceply, Defaultgravity, RedJumpSpeed);
//        else if (ColorPlayer == PlayerColorState.Green)
//            HandleMovementForColor(MaxGreenSpeed, GreemAddRunforceply, Lightgravity, GreenJumpSpeed);
//        else if (ColorPlayer == PlayerColorState.Blue)
//            HandleMovementForColor(MaxBlueSpeed, BlueAddRunforceply, Defaultgravity, BlueJumpSpeed);
//    }

//    private void HandleMovementForColor(float maxSpeed, float addForce, float gravity, float jumpForce)
//    {
//        rb2d.gravityScale = gravity;

//        // 入力チェック
//        float input = (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) ? 1f :
//                      (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) ? -1f : 0f;

//        //ジャンプ処理は常に呼ぶ（入力に関係なく）
//        TryJump(jumpForce);

//        // 横移動処理
//        if (input != 0 && Mathf.Abs(rb2d.linearVelocity.x) < maxSpeed)
//        {
//            rb2d.AddForce(new Vector2(input * addForce, 0), ForceMode2D.Impulse);
//        }
//    }


//    private void TryJump(float jumpForce)
//    {
//        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && !IsJump && IsGround && rb2d.linearVelocity.y <= AddJumpforceply)
//        {
//            IsJump = true;
//            IsGround = false;
//            rb2d.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
//        }
//    }

//    private void changeColor()
//    {
//        if (Input.GetKeyDown(KeyCode.Alpha1))
//            TryChangeColor(PlayerColorState.Red, Color.red, PlayerColorManager.PlayerColorState.Red, ref IsRed);
//        if (Input.GetKeyDown(KeyCode.Alpha2))
//            TryChangeColor(PlayerColorState.Blue, Color.blue, PlayerColorManager.PlayerColorState.Blue, ref IsBlue);
//        if (Input.GetKeyDown(KeyCode.Alpha3))
//            TryChangeColor(PlayerColorState.Green, Color.green, PlayerColorManager.PlayerColorState.Green, ref IsGreen);

//    }

//    private void TryChangeColor(PlayerColorState newColor, Color unityColor, PlayerColorManager.PlayerColorState colorEnum, ref bool isCurrentColor)
//    {
//        if (ColorPlayer == newColor) return;
//        if (!IsFinishColorChangeCoolTime) return;
//        GetComponent<SpriteRenderer>().color = unityColor;
//        IsRed = IsBlue = IsGreen = false;
//        isCurrentColor = true;

//        ColorPlayer = newColor;
//        //chargeBar.ChangeCoolTime(colorEnum); // enumで呼ぶ
//        IsFinishColorChangeCoolTime = false;
//        IsColorChangeCoolTime = true;
//    }


//    private void CheckAttack()
//    {
//        if (!Input.GetMouseButtonDown(0)) return;

//        switch (ColorPlayer)
//        {
//            case PlayerColorState.Red:
//                Debug.Log("赤の攻撃");
//                Instantiate(Redfire, transform.position, Quaternion.identity);
//                break;
//            case PlayerColorState.Blue:
//                Debug.Log("青の攻撃");
//                Instantiate(Bluethunder, transform.position, Quaternion.identity);
//                break;
//            case PlayerColorState.Green:
//                Debug.Log("緑の攻撃");
//                Instantiate(GreenCross, transform.position, Quaternion.identity);
//                break;
//        }
//    }

//    private void GroundYposCheck()
//    {
//        LayerMask layerMask = ~LayerMask.GetMask("Player");
//        Ray2D Toground = new Ray2D(transform.position, Vector2.down);
//        RaycastHit2D under = Physics2D.Raycast(Toground.origin, Toground.direction, 100f, layerMask);
//        Debug.DrawRay(Toground.origin, Toground.direction * 100f, Color.red);

//        if (under.collider != null &&
//            (under.collider.CompareTag("Redfloor") || under.collider.CompareTag("Bluefloor") ||
//             under.collider.CompareTag("Greenfloor") || under.collider.CompareTag("naturalfloor")))
//        {
//            groundypos = under.point.y;
//        }
//    }

//    private IEnumerator HitDamageGrond()
//    {
//        isRunningHitDamageCoroutine = true;
//        while ((IsRed && IstouchRed) || (IsBlue && IstouchBlue) || (IsGreen && IstouchGreen))
//        {

//            PlayerHp -= HitDamageOnfloor;

//            Debug.Log($"残りHP: {PlayerHp}");
//            if (PlayerHp <= 0)
//            {
//                Debug.Log("プレイヤーは死んだ");
//                isRunningHitDamageCoroutine = false;
//                yield break;
//            }
//            yield return new WaitForSeconds(1f);
//        }
//        isRunningHitDamageCoroutine = false;
//    }

//    public void Hitdamage(float HitDamage)
//    {
//        PlayerHp -= HitDamage;
//        Debug.Log($"残りHP: {PlayerHp}");
//    }

//    public void HitWindEffectDamage()
//    {
//        if (Isfly) return; // 風で浮いてるときはreturn

//        Isfly = true; // 風で吹き飛ばされる状態にする
//        rb2d.AddForce(new Vector2(0, flywindVerticalAmount), ForceMode2D.Impulse); // 風のダメージで上に吹き飛ばす

//        GameObject wind = GameObject.FindGameObjectWithTag("Windy");
//        if (wind == null)
//        {
//            Debug.LogWarning("WindyAttack: Windyタグがついたオブジェクトが見つかりません。");
//        }
//        if (wind.transform.position.x > transform.position.x)
//        {

//            Debug.Log("風が右側にいるので右方向に力を加えます。");
//            rb2d.AddForce(new Vector2(-Mathf.Abs(flywindHorizontalAmount), 0), ForceMode2D.Impulse);
//        }
//        else
//        {

//            Debug.Log("風が左側にいるので左方向に力を加えます。");
//            rb2d.AddForce(new Vector2(Mathf.Abs(flywindHorizontalAmount), 0), ForceMode2D.Impulse);
//        }

//        // 一定時間後にIsflyをリセットするコルーチン開始
//        StartCoroutine(ResetIsFlyAfterDelay(0.5f));
//    }

//    private IEnumerator ResetIsFlyAfterDelay(float delay)
//    {
//        yield return new WaitForSeconds(delay);
//        Isfly = false;
//    }



//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        if (collision.CompareTag("Redfloor") || collision.CompareTag("Bluefloor") ||
//            collision.CompareTag("Greenfloor") || collision.CompareTag("naturalfloor"))
//        {
//            IsJump = false;
//            IsGround = true;


//            IstouchRed = collision.CompareTag("Redfloor");
//            IstouchBlue = collision.CompareTag("Bluefloor");
//            IstouchGreen = collision.CompareTag("Greenfloor");

//            if (!isRunningHitDamageCoroutine)
//                StartCoroutine(HitDamageGrond());
//        }


//    }

//    private void OnTriggerStay2D(Collider2D collision)
//    {
//        if (collision.CompareTag("Redfloor") || collision.CompareTag("Bluefloor") ||
//            collision.CompareTag("Greenfloor") || collision.CompareTag("naturalfloor"))
//        {
//            IsGround = true;

//            IstouchRed = collision.CompareTag("Redfloor");
//            IstouchBlue = collision.CompareTag("Bluefloor");
//            IstouchGreen = collision.CompareTag("Greenfloor");
//        }
//    }

//    private void OnTriggerExit2D(Collider2D collision)
//    {
//        if (collision.CompareTag("Redfloor") || collision.CompareTag("Bluefloor") ||
//            collision.CompareTag("Greenfloor") || collision.CompareTag("naturalfloor"))
//        {
//            IsGround = false;

//            if (collision.CompareTag("Redfloor")) IstouchRed = false;
//            if (collision.CompareTag("Bluefloor")) IstouchBlue = false;
//            if (collision.CompareTag("Greenfloor")) IstouchGreen = false;
//        }
//    }
//}