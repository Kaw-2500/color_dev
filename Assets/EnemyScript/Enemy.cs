using System;
using UnityEngine;

public class Enemy : MonoBehaviour 
{
    private Rigidbody2D rb2d;

    public bool canmove = true;
    private Transform player;

    [SerializeField] EnemyData enemyData; // Enemy�̃f�[�^��ێ�����N���X

    [SerializeField] private PlayerStateManager playerStateManager;
    [SerializeField] private EnemyHitDamage hitdamage;

    private float CurrentHp;
  
    private bool isClimbing = false;
    private bool isWallUnder;
    private EnemyStateManager stateManager; //Enemy�́u��ԊǗ��v�Ƃ����ӔC��EnemyStateManager�N���X�ɈϏ�

    public bool Isattacking = false;

    private IMovable movable; //�ړ��̐U�镑����IMovable�C���^�[�t�F�[�X�Œ��ۉ�
    private IAttackable attacker; //�U���̐U�镑����IAttackable�C���^�[�t�F�[�X�Œ��ۉ�

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

    // isFrozen�֘A�̃t�B�[���h�E���\�b�h���폜

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();

        player = GameObject.FindGameObjectWithTag("Player").transform;

        // �_���[�W���󂯂��Ƃ��̔����i�������o�Ȃǁj
        damageReaction = GetComponent<IReactionOnDamage>();
    }

    private void Start()
    {
        // ����HP�̐ݒ�
        CurrentHp = enemyData.EnemyHp;

        // ������U���̋�ۃN���X�𐶐��B�p�����[�^��ScriptableObject����ǂݍ���
        movable = new EnemyMovement(rb2d, enemyData.moveForce, enemyData.maxSpeed);
        attacker = new EnemyAttack(enemyData.NormalAttackPrefab, this);

        // ��ԃ}�l�[�W���[�̐����ƁA������ԁi�ҋ@�j�ւ̑J��
        stateManager = new EnemyStateManager(this);
        stateManager.SetState(new IdleState(stateManager));

        canmove = true;
    }

    void Update()
    {
        if (playerStateManager != null && playerStateManager.IsDead) return;
        // �v���C���[�ʒu�X�V�E�o������X�V
        UpdatePlayerRelativePosition();
        UpdateClimbCondition();
        stateManager.Update();
    }

    void FixedUpdate()
    {
        if (playerStateManager != null && playerStateManager.IsDead) return;
      

       if(!canmove) return;//��������͂��̂܂܁A���삾�������Ȃ�
        if (isKnockback) return; // ������΂�����AI��~
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
        CurrentHp = Mathf.Max(CurrentHp - damage, 0f);//�}�C�i�X�͌����I�I
        Debug.Log("enemy applydamage");
        damageReaction?.OnDamaged(CurrentHp);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var attackStrategy = collision.GetComponent<IAttackStrategy>();
        if (attackStrategy != null)
        {
            OnDamagedByPlayer?.Invoke();
            Debug.Log("Enemy.cs: �v���C���[����U�����ꂽ���Ƃ�ʒm");
        }
        if (attackStrategy == null)
        {
            Debug.Log("Strategy��������Ȃ�");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        var attackStrategy = collision.gameObject.GetComponent<IAttackStrategy>();
        if (attackStrategy != null)
        {
            OnDamagedByPlayer?.Invoke();
            Debug.Log("Enemy.cs: �v���C���[����U�����ꂽ���Ƃ�ʒm�iCollision�j");
        }
        if (attackStrategy == null)
        {
            Debug.Log("Strategy��������Ȃ��iCollision�j");
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

    // ��ԃN���X�����̃A�N�Z�T�i�J�v�Z�������ꂽ������ԃN���X�ɒ񋟁j
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
    public float GetNormalAttackOffsetY() => enemyData.normalAttackOffsetY; // �ʏ�U����Y�I�t�Z�b�g���擾   
    public float GetNormalAttackOffsetX() => enemyData.normalAttackOffsetX; // �ʏ�U����X�I�t�Z�b�g���擾
    public IMovable GetMovable() => movable;
    public IAttackable GetAttacker() => attacker;

    public bool CanMove() => canmove;

    public float GetAttackPower() => enemyData.AttackPower; 

    public float attackCooldownTimer = 0f; // �ǉ�

    public bool isKnockback = false;
}