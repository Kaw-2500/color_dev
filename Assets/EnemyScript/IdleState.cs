using UnityEngine;

public class IdleState : IEnemyState //�G�́u�ҋ@�v��Ԃ̋�̓I�ȃ��W�b�N�݂̂ɐӔC������
{
    private EnemyStateManager manager;
    private Enemy enemy => manager.GetEnemy(); // ��ԃ}�l�[�W���[����Enemy�C���X�^���X���擾

    public IdleState(EnemyStateManager manager) => this.manager = manager;

    public void EnterState() => enemy.GetMovable().Stop(); //IMovable�C���^�[�t�F�[�X�o�R�ňړ����~

    public void UpdateState()
    {
        float dist = Vector2.Distance(enemy.transform.position, enemy.GetPlayer().position);
        if (dist < enemy.GetChaseRange()) // �v���C���[���ǐՔ͈͂ɓ�������
            manager.SetState(new ChaseState(manager)); //  ��ԑJ�ڂ�EnemyStateManager�Ɉ˗�
    }

    public void ExitState() { }
}