using System;
using UnityEngine;

public class Enemy : MonoBehaviour 
{
    private Rigidbody2D rb2d;

    public bool canmove = true;
    private Transform player;

    [SerializeField] EnemyData enemyData; // Enemyのデータを保持するクラス

    [SerializeField] private PlayerStateManager playerStateManager;
    [SerializeField] private EnemyHitDamage hitdamage;

    private float CurrentHp;
  
    private bool isClimbing = false;
    private bool isWallUnder;
    private EnemyStateManager stateManager; //Enemyの「状態管理」という責任をEnemyStateManagerクラスに委譲

    public bool Isattacking = false;

    private IMovable movable; //移動の振る舞いをIMovableインターフェースで抽象化
    private IAttackable attacker; //攻撃の振る舞いをIAttackableインターフェースで抽象化

    public enum PlayerRelativePosition { None, Right, Left, NearRight, NearLeft }
    public PlayerRelativePosition playerRelativePosition = PlayerRelativePosition.None;

    private IReactionOnDamage damageReaction;

    public event Action OnDamagedByPlayer;

    public enum WhiteningStage
    {
        None,
        Slight,
        Moderate,
        Heavy,
        Complete
    }

    // isFrozen関連のフィールド・メソッドを削除

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();

        player = GameObject.FindGameObjectWithTag("Player").transform;

        // ダメージを受けたときの反応（白化演出など）
        damageReaction = GetComponent<IReactionOnDamage>();
    }

    private void Start()
    {
        // 初期HPの設定
        CurrentHp = enemyData.EnemyHp;

        // 動きや攻撃の具象クラスを生成。パラメータはScriptableObjectから読み込む
        movable = new EnemyMovement(rb2d, enemyData.moveForce, enemyData.maxSpeed);
        attacker = new EnemyAttack(enemyData.NormalAttackPrefab, this);

        // 状態マネージャーの生成と、初期状態（待機）への遷移
        stateManager = new EnemyStateManager(this);
        stateManager.SetState(new IdleState(stateManager));

        canmove = true;
    }

    void Update()
    {
        if (playerStateManager != null && playerStateManager.IsDead) return;
        // プレイヤー位置更新・登り条件更新
        UpdatePlayerRelativePosition();
        UpdateClimbCondition();
        stateManager.Update();
    }

    void FixedUpdate()
    {
        if (playerStateManager != null && playerStateManager.IsDead) return;
      

       if(!canmove) return;//物理判定はそのまま、動作だけさせない
        if (isKnockback) return; // 吹き飛ばし中はAI停止
        stateManager.FixedUpdate();
    }

    private void UpdatePlayerRelativePosition()
    {
        float dx = player.position.x - transform.position.x;
        float distanceX = Mathf.Abs(dx);
        bool isRight = dx >= 0;

        if (distanceX <= enemyData.attackRange)
            playerRelativePosition = isRight ? PlayerRelativePosition.NearRight : PlayerRelativePosition.NearLeft;
        else
            playerRelativePosition = isRight ? PlayerRelativePosition.Right : PlayerRelativePosition.Left;
    }

    public float floatPlayerRelativePosition()
    {
        float a = 1;

        switch (playerRelativePosition)
        {
            case PlayerRelativePosition.Left:
                a = -1; break;
            case PlayerRelativePosition.Right:
                a = 1 ; break;
            case PlayerRelativePosition.NearLeft:
                a = -1 ; break;
            case PlayerRelativePosition.NearRight:
                a = 1 ; break;
        }
        return a;
    }

    private void UpdateClimbCondition()
    {
        isClimbing = false;
        isWallUnder = false;

        float height = 0.98f;
        Vector3 basePos = transform.position;

        RaycastHit2D hitRight = Physics2D.Raycast(basePos + new Vector3(0.4f, -height / 2f), Vector2.right, enemyData.rightRayLength, LayerMask.GetMask("floor"));
        RaycastHit2D hitLeft = Physics2D.Raycast(basePos + new Vector3(-0.4f, -height / 2f), Vector2.left, enemyData.leftRayLength, LayerMask.GetMask("floor"));

        if ((playerRelativePosition == PlayerRelativePosition.Right || playerRelativePosition == PlayerRelativePosition.NearRight) && hitRight.collider != null)
            isClimbing = true;
        else if ((playerRelativePosition == PlayerRelativePosition.Left || playerRelativePosition == PlayerRelativePosition.NearLeft) && hitLeft.collider != null)
            isClimbing = true;

        if (hitRight.collider != null || hitLeft.collider != null)
            isWallUnder = true;

        Debug.DrawRay(basePos + new Vector3(0.4f, -height / 2f), Vector2.right * enemyData.rightRayLength, Color.red);
        Debug.DrawRay(basePos + new Vector3(-0.4f, -height / 2f), Vector2.left * enemyData.leftRayLength, Color.blue);

        //Debug.Log($"RightHit: {(hitRight.collider != null)} / LeftHit: {(hitLeft.collider != null)}");
    }


    public void ApplyDamage(float damage)
    {
        CurrentHp = Mathf.Max(CurrentHp - damage, 0f);//マイナスは嫌い！！
        Debug.Log("enemy applydamage");
        damageReaction?.OnDamaged(CurrentHp);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var attackStrategy = collision.GetComponent<IAttackStrategy>();
        if (attackStrategy != null)
        {
            OnDamagedByPlayer?.Invoke();
            Debug.Log("Enemy.cs: プレイヤーから攻撃されたことを通知");
        }
        if (attackStrategy == null)
        {
            Debug.Log("Strategyが見つからない");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        var attackStrategy = collision.gameObject.GetComponent<IAttackStrategy>();
        if (attackStrategy != null)
        {
            OnDamagedByPlayer?.Invoke();
            Debug.Log("Enemy.cs: プレイヤーから攻撃されたことを通知（Collision）");
        }
        if (attackStrategy == null)
        {
            Debug.Log("Strategyが見つからない（Collision）");
        }

        if (isKnockback)
        {
            string tag = collision.gameObject.tag;
            if (tag == "Redfloor" || tag == "Bluefloor" || tag == "Greenfloor" || tag == "naturalfloor")
            {
                isKnockback = false;
            }
        }
    }

    // 状態クラス向けのアクセサ（カプセル化された情報を状態クラスに提供）
    public Transform GetPlayer() => player;
    public PlayerRelativePosition GetPlayerRelativePosition() => playerRelativePosition;

    public float GetPlayerRelativeFloat() => floatPlayerRelativePosition();

    public bool IsClimbing() => isClimbing;
    public bool IsWallUnder() => isWallUnder;
    public float GetAttackRange() => enemyData.attackRange;
    public float GetAttackCooldown() => enemyData.attackCooldown; 
    public float GetChaseRange() => enemyData.chaseRange;

    public float GetEnemyLevity() => enemyData.Enemylevity;

    public float GetNormalAttackForce() => enemyData.normalAttackForce; 

    public float GetRigidBody2dVelocityY() => rb2d.linearVelocity.y;

    public float GetWeaknessRed() => enemyData.weaknessRed;
    public float GetWeaknessBlue() => enemyData.weaknesBlue;
    public float GetWeaknessGreen() => enemyData.weaknesGreen;


    public EnemyHitDamage GetEnemyHitObject() => hitdamage;
    public float GetNormalAttackOffsetY() => enemyData.normalAttackOffsetY; // 通常攻撃のYオフセットを取得   
    public float GetNormalAttackOffsetX() => enemyData.normalAttackOffsetX; // 通常攻撃のXオフセットを取得
    public IMovable GetMovable() => movable;
    public IAttackable GetAttacker() => attacker;

    public bool CanMove() => canmove;

    public float GetAttackPower() => enemyData.AttackPower; 

    public float attackCooldownTimer = 0f; // 追加

    public bool isKnockback = false;
}