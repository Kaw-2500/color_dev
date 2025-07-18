using UnityEngine;

public class Enemy : MonoBehaviour // MonoBehaviour���p�����AUnity��GameObject�ɃA�^�b�`�����R���|�[�l���g
{
    private Rigidbody2D rb2d;
    private Transform player;

    [SerializeField] EnemyData enemyData; // Enemy�̃f�[�^��ێ�����N���X

    [SerializeField] private PlayerStateManager playerStateManager;
    [SerializeField] private HitDamage hitdamage;

  
    private bool isClimbing = false;
    private bool isWallUnder;
    private EnemyStateManager stateManager; //Enemy�́u��ԊǗ��v�Ƃ����ӔC��EnemyStateManager�N���X�ɈϏ�

    private IMovable movable; //�ړ��̐U�镑����IMovable�C���^�[�t�F�[�X�Œ��ۉ�
    private IAttackable attacker; //�U���̐U�镑����IAttackable�C���^�[�t�F�[�X�Œ��ۉ�

    public enum PlayerRelativePosition { None, Right, Left, NearRight, NearLeft }
    private PlayerRelativePosition playerRelativePosition = PlayerRelativePosition.None;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // ��SRP & OOP: ��̓I�Ȏ����N���X�iEnemyMovement, EnemyAttack�j���C���X�^���X�����A�C���^�[�t�F�[�X�^�ŕێ�
        movable = new EnemyMovement(rb2d, enemyData.moveForce, enemyData.maxSpeed);
        attacker = new EnemyAttack(enemyData.NormalAttackPrefab,this);

        // ��SRP & OOP: ��ԃ}�l�[�W���[�̃C���X�^���X���BEnemy�̏�ԑJ�ڂ̐ӔC������
        stateManager = new EnemyStateManager(this);
        stateManager.SetState(new IdleState(stateManager)); // ������Ԃ̐ݒ�
    }

    void Update()
    {
        if (playerStateManager != null && playerStateManager.IsDead) return;

        // ��SRP: �e�ӔC�i�v���C���[�ʒu�X�V�A�o������X�V�j���ʂ̃v���C�x�[�g���\�b�h�ɕ���
        UpdatePlayerRelativePosition();
        UpdateClimbCondition();
        stateManager.Update(); // ��OOP: ��ԃ}�l�[�W���[��Update�������Ϗ�
    }

    void FixedUpdate()
    {
        if (playerStateManager != null && playerStateManager.IsDead) return;
        stateManager.FixedUpdate(); // ��OOP: ��ԃ}�l�[�W���[��FixedUpdate�������Ϗ�
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

        Debug.Log($"playerRelativePosition: {playerRelativePosition}");
        Debug.Log($"isClimbing: {isClimbing}");


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

    public void ApplyDamage(float damage)
    {
        enemyData.EnemyHp -= hitdamage.HitAttackDamage(damage); 
        Debug.Log($"�G����e, remaining HP: {enemyData.EnemyHp}");
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

    public float GetNormalAttackOffsetX() => enemyData.normalAttackOffsetX; // �ʏ�U����X�I�t�Z�b�g���擾
    public IMovable GetMovable() => movable;
    public IAttackable GetAttacker() => attacker;


}