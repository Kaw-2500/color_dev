using UnityEngine;

public class ClimbState : IEnemyState, IEnemyPhysicsState //�G�́u�o��v��Ԃ̋�̓I�ȃ��W�b�N�݂̂ɐӔC������
{
    private EnemyStateManager manager;
    private Enemy enemy => manager.GetEnemy();

    public ClimbState(EnemyStateManager manager) => this.manager = manager;

    public void EnterState() { }

    public void UpdateState()
    {
        if (!enemy.IsClimbing()) // �o������𖞂����Ȃ��Ȃ�����
            manager.SetState(new ChaseState(manager)); // �ǐՏ�Ԃɖ߂�
        else if (Vector2.Distance(enemy.transform.position, enemy.GetPlayer().position) > enemy.GetChaseRange()) // �v���C���[���ǐՔ͈͊O�ɏo����
            manager.SetState(new IdleState(manager)); // �ҋ@��Ԃɖ߂�
    }

    public void FixedUpdateState()
    {
        if (enemy.IsWallUnder()) // �����ɕǂ�����ꍇ
            enemy.GetMovable().Move(Vector2.up); //IMovable�C���^�[�t�F�[�X�o�R�ŏ�����Ɉړ�
        Debug.Log($"ClimbState FixedUpdateState called. isClimbing={enemy.IsClimbing()}, isWallUnder={enemy.IsWallUnder()}");
    }

    public void ExitState() => enemy.GetMovable().Stop(); // ��Ԃ𔲂���Ƃ��Ɉړ����~
}