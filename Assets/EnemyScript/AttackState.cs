using UnityEngine;

public class AttackState : IEnemyState // �G�́u�U���v��Ԃ̋�̓I�ȃ��W�b�N�݂̂ɐӔC������
{
    private EnemyStateManager manager;
    private Enemy enemy => manager.GetEnemy();

    private float timer = 0f; // �N���X�t�B�[���h�Ƃ��ĕێ�
    private float CoolDowntimer => manager.GetEnemy().GetAttackCooldown();

    public AttackState(EnemyStateManager manager) => this.manager = manager;

    public void EnterState()
    {
        enemy.GetMovable().Stop(); // �U�����͈ړ����~
    }

    public void UpdateState()
    {
        timer += Time.deltaTime;
        float dist = Vector2.Distance(enemy.transform.position, enemy.GetPlayer().position);

        if (dist > enemy.GetAttackRange()) // �N�[���_�E�����I���A�U���͈͊O�ɏo����
            manager.SetState(new ChaseState(manager)); 
        else if (timer >= CoolDowntimer)
        {
            timer = 0;
            enemy.GetAttacker().Attack(); 
        }
    }

    public void ExitState() { }
}