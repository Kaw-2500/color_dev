using UnityEngine;

public class Enemy : MonoBehaviour 
{
    private Rigidbody2D rb2d;
    private Transform player;

    [SerializeField] EnemyData enemyData; // Enemyのデータを保持するクラス

    [SerializeField] private PlayerStateManager playerStateManager;
    [SerializeField] private EnemyHitDamage hitdamage;

    private float CurrentHp;
  
    private bool isClimbing = false;
    private bool isWallUnder;
    private EnemyStateManager stateManager; //Enemyの「状態管理」という責任をEnemyStateManagerクラスに委譲

    private IMovable movable; //移動の振る舞いをIMovableインターフェースで抽象化
    private IAttackable attacker; //攻撃の振る舞いをIAttackableインターフェースで抽象化

    public enum PlayerRelativePosition { None, Right, Left, NearRight, NearLeft }
    private PlayerRelativePosition playerRelativePosition = PlayerRelativePosition.None;

    private WhiteningStage whiteningStage = WhiteningStage.None;


    public enum WhiteningStage
    {
        None,
        Slight,
        Moderate,
        Heavy,
        Complete
    }

    void Start()
    {
        CurrentHp = enemyData.EnemyHp; // 初期HPを設定

        rb2d = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        //具体的な実装クラス（EnemyMovement, EnemyAttack）をインスタンス化し、インターフェース型で保持
        movable = new EnemyMovement(rb2d, enemyData.moveForce, enemyData.maxSpeed);
        attacker = new EnemyAttack(enemyData.NormalAttackPrefab,this);

        //状態マネージャーのインスタンス化。Enemyの状態遷移の責任を持つ
        stateManager = new EnemyStateManager(this);
        stateManager.SetState(new IdleState(stateManager)); // 初期状態の設定
    }

    void Update()
    {
        if (playerStateManager != null && playerStateManager.IsDead) return;

        //各責任（プレイヤー位置更新、登り条件更新）を個別のプライベートメソッドに分割
        UpdatePlayerRelativePosition();
        UpdateClimbCondition();
        stateManager.Update(); //状態マネージャーにUpdate処理を委譲
    }

    void FixedUpdate()
    {
        if (playerStateManager != null && playerStateManager.IsDead) return;
        stateManager.FixedUpdate(); //状態マネージャーにFixedUpdate処理を委譲
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

    private void UpdateClimbCondition()
    {
        isClimbing = false;
        isWallUnder = false;

        float height = 0.95f;
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

    private void UpdateWhiteningStage()
    {
        float hpRatio = CurrentHp / enemyData.EnemyHp;

        if (hpRatio <= 0f)
            whiteningStage = WhiteningStage.Complete;
        else if (hpRatio <= 0.25f)
            whiteningStage = WhiteningStage.Heavy;
        else if (hpRatio <= 0.5f)
            whiteningStage = WhiteningStage.Moderate;
        else if (hpRatio <= 0.75f)
            whiteningStage = WhiteningStage.Slight;
        else
            whiteningStage = WhiteningStage.None;
    }

    public void UpdateWhiteningEffects()
    {
        float speedMultiplier = whiteningStage switch
        {
            WhiteningStage.None => 1f,
            WhiteningStage.Slight => 0.8f,
            WhiteningStage.Moderate => 0.6f,
            WhiteningStage.Heavy => 0.4f,
            WhiteningStage.Complete => 0f,
            _ => 1f,
        };
        if (movable is EnemyMovement movementImpl)
            movementImpl.SetSpeedMultiplier(speedMultiplier);

        float attackMultiplier = whiteningStage switch
        {
            WhiteningStage.None => 1f,
            WhiteningStage.Slight => 0.9f,
            WhiteningStage.Moderate => 0.7f,
            WhiteningStage.Heavy => 0.5f,
            WhiteningStage.Complete => 0f,
            _ => 1f,
        };
        if (attacker is EnemyAttack attackImpl)
            attackImpl.SetAttackPowerMultiplier(attackMultiplier);
    }


    public void ApplyDamage(float damage)
    {
        CurrentHp -= damage;
        Debug.Log($"敵が被弾, remaining HP: {CurrentHp}");

        UpdateWhiteningStage();
        UpdateWhiteningEffects();
    }



    // 状態クラス向けのアクセサ（カプセル化された情報を状態クラスに提供）
    public Transform GetPlayer() => player;
    public PlayerRelativePosition GetPlayerRelativePosition() => playerRelativePosition;
    public bool IsClimbing() => isClimbing;
    public bool IsWallUnder() => isWallUnder;
    public float GetAttackRange() => enemyData.attackRange;
    public float GetAttackCooldown() => enemyData.attackCooldown; 
    public float GetChaseRange() => enemyData.chaseRange;

    public float GetNormalAttackForce() => enemyData.normalAttackForce; 

 
    public float GetNormalAttackOffsetY() => enemyData.normalAttackOffsetY; // 通常攻撃のYオフセットを取得   
    public float GetNormalAttackOffsetX() => enemyData.normalAttackOffsetX; // 通常攻撃のXオフセットを取得
    public IMovable GetMovable() => movable;
    public IAttackable GetAttacker() => attacker;

    public float GetAttackPower() => enemyData.AttackPower; 


}