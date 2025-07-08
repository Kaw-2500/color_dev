using UnityEngine;

public class WeakEnemy : MonoBehaviour
{
    private Rigidbody2D Rb2d;
    private Transform player;

    private PlayerRelativePosition playerRelativePosition; // 追加: enumで管理

    public float attackRange = 1.5f; // 攻撃範囲
    [SerializeField] private float chaseRange = 20f; // 追跡範囲
    [SerializeField] private float weakEnemyMaxSpeed = 5f; // 弱い敵の最高速度
    [SerializeField] private float moveForce = 10f; //加速度
    [SerializeField] public EnemyState currentState = EnemyState.Idle; // 現在の状態

    [SerializeField] private float RightRayLength = 0.5f; // 右側のレイの長さ
    [SerializeField] private float LeftRayLength = 0.5f; // 左側のレイの長さ   
    private bool isGround = false; // 地面にいるかどうかのフラグ

    [SerializeField] private bool isClimbing = false; // クライミング状態かどうかのフラグ  フラグ確認のためにシリアライズしています
    bool isWallUnder;
    bool isWallTop;

    void Start()
    {
        Rb2d = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentState = EnemyState.Idle;
    }

    public enum EnemyState
    {
        Idle,
        Climb,
        Chase,
        Attack,
        Dead
    }

    public enum PlayerRelativePosition
    {
        None,
        Right,
        Left,
        NearRight,
        NearLeft
    }

    private void Update()
    {
        CheckPlayerDirection();
        UpdateState();
        CheckClimbCondition();
    }

    void FixedUpdate()
    {
        MoveWeakEnemy(); // ここで物理処理
    }

    void CheckPlayerDirection()
    {
       
        float dx = player.position.x - transform.position.x;

        // 横距離の絶対値（距離の大きさだけ）
        float distanceX = Mathf.Abs(dx);


        bool isRight = dx >= 0;
        if (distanceX <= attackRange)
        {
            playerRelativePosition = isRight ? PlayerRelativePosition.NearRight : PlayerRelativePosition.NearLeft;
        }
        else
        {
            playerRelativePosition = isRight ? PlayerRelativePosition.Right : PlayerRelativePosition.Left;
        }
    }


    void UpdateState()//状態優先度は Attack > Climb > Chase > Idle と、かぶった場合はマージ
    {
        float distanceX = Mathf.Abs(transform.position.x - player.position.x);

        if (distanceX > chaseRange)
        {
            currentState = EnemyState.Idle;
        }
        else if (distanceX > attackRange)
        {
            if (isClimbing)
            {
                currentState = EnemyState.Climb;
                return;
            }

            currentState = EnemyState.Chase;
        }
        else
        {
            if (isClimbing && distanceX <= attackRange)
            {
                // 登れる状態で、攻撃範囲なら攻撃を優先
                currentState = EnemyState.Attack;
            }
            else if (isClimbing)
            {
                // 登れるけど攻撃範囲ではない → 登る
                currentState = EnemyState.Climb;
            }
            else
            {
                // 登れない → 攻撃
                currentState = EnemyState.Attack;
            }
        }
    }

    void ChangeDirection()
    {
        if (playerRelativePosition == PlayerRelativePosition.Right || playerRelativePosition == PlayerRelativePosition.NearRight)
        {
            Rb2d.transform.localScale = new Vector3(1f, 1f, 1f); // 右向き
        }
        else if (playerRelativePosition == PlayerRelativePosition.Left || playerRelativePosition == PlayerRelativePosition.NearLeft)
        {
            Rb2d.transform.localScale = new Vector3(-1f, 1f, 1f); // 左向き
        }
    }

    void MoveWeakEnemy()
    {
        float currentSpeed = Mathf.Abs(Rb2d.linearVelocity.x);
        float direction = (playerRelativePosition == PlayerRelativePosition.Right || playerRelativePosition == PlayerRelativePosition.NearRight) ? 1f : -1f;

        switch (currentState)
        {
            case EnemyState.Chase:
                ChangeDirection();
                if (currentSpeed < weakEnemyMaxSpeed)
                {
                    Rb2d.AddForce(new Vector2(moveForce * direction, 0), ForceMode2D.Force);
                }
                break;

            case EnemyState.Attack:
                Rb2d.linearVelocity = new Vector2(Rb2d.linearVelocity.x * 0.6f, Rb2d.linearVelocity.y);//急制動で止める
                break;

            case EnemyState.Climb:
                if (isWallUnder)
                {
                    Rb2d.MovePosition(Rb2d.position + new Vector2(0f, 0.15f));
                }
                else
                {
                    isClimbing = false; // クライミング状態を終了
                }
                break;

            default:
                Rb2d.linearVelocity = new Vector2(Rb2d.linearVelocity.x * 0.9f, Rb2d.linearVelocity.y);//じょじょに止まる
                break;
        }
    }

    public PlayerRelativePosition GetPlayerRelativePosition()
    {
        return playerRelativePosition;
    }


    void CheckClimbCondition()
    {
        isClimbing = false; // 初期化
        isWallUnder = false;

        float characterHeight = 0.95f; // キャラの高さ（実際の値に合わせて調整）
        Vector3 basePos = transform.position;

        Ray2D UnderRightRay = new Ray2D(new Vector3(basePos.x + 0.4f, basePos.y - characterHeight / 2f, basePos.z), Vector2.right);
        Ray2D UnderLeftRay = new Ray2D(new Vector3(basePos.x - 0.4f, basePos.y - characterHeight / 2f, basePos.z), Vector2.left);

        RaycastHit2D hitUnderRight = Physics2D.Raycast(UnderRightRay.origin, UnderRightRay.direction, RightRayLength, LayerMask.GetMask("floor"));
        RaycastHit2D hitUnderLeft = Physics2D.Raycast(UnderLeftRay.origin, UnderLeftRay.direction, LeftRayLength, LayerMask.GetMask("floor"));

        if ((playerRelativePosition == PlayerRelativePosition.Right) && hitUnderRight.collider != null)
        {
            isClimbing = true;
        }
        else if ((playerRelativePosition == PlayerRelativePosition.Left) && hitUnderLeft.collider != null)
        {
            isClimbing = true;
        }


        if (hitUnderLeft.collider != null || hitUnderRight.collider != null)
        {
            isWallUnder = true; // 下に壁がある   
        }

        // デバッグ表示（RayをScene上に表示）
        Debug.DrawRay(UnderRightRay.origin, UnderRightRay.direction * RightRayLength, Color.red); //右下
        Debug.DrawRay(UnderLeftRay.origin, UnderLeftRay.direction * LeftRayLength, Color.blue);   //左下
    }

    public EnemyState GetCurrentState()   
    {
        return currentState; // 現在の状態を返すメソッド
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Redfloor" ||
            collision.gameObject.tag == "Bluefloor" || 
            collision.gameObject.tag == "Greenfloor" || 
            collision.gameObject.tag == "naturalfloor")
        {
            isGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Redfloor" ||
            collision.gameObject.tag == "Bluefloor" ||
            collision.gameObject.tag == "Greenfloor" ||
            collision.gameObject.tag == "naturalfloor")
        {
            isGround = false;
        }
    }
}