using UnityEngine;

public class AttackState : IEnemyState // ��SRP: �G�́u�U���v��Ԃ̋�̓I�ȃ��W�b�N�݂̂ɐӔC������
{
    private EnemyStateManager manager;
    private Enemy enemy => manager.GetEnemy();
    private float attackCooldown = 1f; // �U���N�[���_�E��
    private float timer = 0f; // �U���^�C�}�[

    public AttackState(EnemyStateManager manager) => this.manager = manager;

    public void EnterState()
    {
        timer = 0;
        enemy.GetMovable().Stop(); // �U�����͈ړ����~
        enemy.GetAttacker().Attack(); // ��OOP: IAttackable�C���^�[�t�F�[�X�o�R�ōU�������s
    }

    public void UpdateState()
    {
        timer += Time.deltaTime;
        float dist = Vector2.Distance(enemy.transform.position, enemy.GetPlayer().position);

        if (timer > attackCooldown && dist > enemy.GetAttackRange()) // �N�[���_�E�����I���A�U���͈͊O�ɏo����
            manager.SetState(new ChaseState(manager)); // �ǐՏ�Ԃɖ߂�
    }

    public void ExitState() { }
}