using UnityEngine;

public class Enemy : MonoBehaviour 
{
    private Rigidbody2D rb2d;
    private Transform player;

    [SerializeField] EnemyData enemyData; // Enemy�̃f�[�^��ێ�����N���X

    [SerializeField] private PlayerStateManager playerStateManager;
    [SerializeField] private EnemyHitDamage hitdamage;

    private float CurrentHp;
  
    private bool isClimbing = false;
    private bool isWallUnder;
    private EnemyStateManager stateManager; //Enemy�́u��ԊǗ��v�Ƃ����ӔC��EnemyStateManager�N���X�ɈϏ�

    private IMovable movable; //�ړ��̐U�镑����IMovable�C���^�[�t�F�[�X�Œ��ۉ�
    private IAttackable attacker; //�U���̐U�镑����IAttackable�C���^�[�t�F�[�X�Œ��ۉ�

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
        CurrentHp = enemyData.EnemyHp; // ����HP��ݒ�

        rb2d = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        //��̓I�Ȏ����N���X�iEnemyMovement, EnemyAttack�j���C���X�^���X�����A�C���^�[�t�F�[�X�^�ŕێ�
        movable = new EnemyMovement(rb2d, enemyData.moveForce, enemyData.maxSpeed);
        attacker = new EnemyAttack(enemyData.NormalAttackPrefab,this);

        //��ԃ}�l�[�W���[�̃C���X�^���X���BEnemy�̏�ԑJ�ڂ̐ӔC������
        stateManager = new EnemyStateManager(this);
        stateManager.SetState(new IdleState(stateManager)); // ������Ԃ̐ݒ�
    }

    void Update()
    {
        if (playerStateManager != null && playerStateManager.IsDead) return;

        //�e�ӔC�i�v���C���[�ʒu�X�V�A�o������X�V�j���ʂ̃v���C�x�[�g���\�b�h�ɕ���
        UpdatePlayerRelativePosition();
        UpdateClimbCondition();
        stateManager.Update(); //��ԃ}�l�[�W���[��Update�������Ϗ�
    }

    void FixedUpdate()
    {
        if (playerStateManager != null && playerStateManager.IsDead) return;
        stateManager.FixedUpdate(); //��ԃ}�l�[�W���[��FixedUpdate�������Ϗ�
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
        Debug.Log($"�G����e, remaining HP: {CurrentHp}");

        UpdateWhiteningStage();
        UpdateWhiteningEffects();
    }



    // ��ԃN���X�����̃A�N�Z�T�i�J�v�Z�������ꂽ������ԃN���X�ɒ񋟁j
    public Transform GetPlayer() => player;
    public PlayerRelativePosition GetPlayerRelativePosition() => playerRelativePosition;
    public bool IsClimbing() => isClimbing;
    public bool IsWallUnder() => isWallUnder;
    public float GetAttackRange() => enemyData.attackRange;
    public float GetAttackCooldown() => enemyData.attackCooldown; 
    public float GetChaseRange() => enemyData.chaseRange;

    public float GetNormalAttackForce() => enemyData.normalAttackForce; 

 
    public float GetNormalAttackOffsetY() => enemyData.normalAttackOffsetY; // �ʏ�U����Y�I�t�Z�b�g���擾   
    public float GetNormalAttackOffsetX() => enemyData.normalAttackOffsetX; // �ʏ�U����X�I�t�Z�b�g���擾
    public IMovable GetMovable() => movable;
    public IAttackable GetAttacker() => attacker;

    public float GetAttackPower() => enemyData.AttackPower; 


}