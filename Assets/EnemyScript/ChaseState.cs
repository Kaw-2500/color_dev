using UnityEngine;

public class ChaseState : IEnemyState, IEnemyPhysicsState // ��SRP: �G�́u�ǐՁv��Ԃ̋�̓I�ȃ��W�b�N�݂̂ɐӔC������
{
    private EnemyStateManager manager;
    private Enemy enemy => manager.GetEnemy();

    public ChaseState(EnemyStateManager manager) => this.manager = manager;

    public void EnterState() { }

    public void UpdateState()
    {
        float dist = Vector2.Distance(enemy.transform.position, enemy.GetPlayer().position);

        if (dist <= enemy.GetAttackRange()) // �U���͈͓��ɓ�������
            manager.SetState(new AttackState(manager));
        else if (enemy.IsClimbing()) // �o������𖞂�������
            manager.SetState(new ClimbState(manager));
        else if (dist > enemy.GetChaseRange()) // �ǐՔ͈͊O�ɏo����
            manager.SetState(new IdleState(manager));
    }

    public void FixedUpdateState() // ��SRP: �����X�V�ɓ��������ړ����W�b�N
    {
        float dx = enemy.GetPlayer().position.x - enemy.transform.position.x;
        Vector2 dir = dx >= 0 ? Vector2.right : Vector2.left;
        enemy.GetMovable().Move(dir); // ��OOP: IMovable�C���^�[�t�F�[�X�o�R�ňړ�
    }

    public void ExitState() => enemy.GetMovable().Stop(); // ��Ԃ𔲂���Ƃ��Ɉړ����~
}