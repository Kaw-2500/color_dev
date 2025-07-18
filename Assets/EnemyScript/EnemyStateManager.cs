using UnityEngine;

public class EnemyStateManager // ��SRP: �G�́u��ԑJ�ځv�Ɓu���݂̏�Ԃ̊Ǘ��v�݂̂ɐӔC������
{
    private IEnemyState currentState; // ��OOP: �C���^�[�t�F�[�X�^�ŏ�Ԃ�ێ����A��̓I�ȏ�ԃN���X�Ɉˑ����Ȃ�
    private Enemy enemy; // ��ԃ}�l�[�W���[�����䂷��Enemy�C���X�^���X

    public EnemyStateManager(Enemy enemy) // �R���X�g���N�^��Enemy�C���X�^���X���󂯎��
    {
        this.enemy = enemy;
    }

    public void SetState(IEnemyState newState) // ��SRP: ��Ԃ̐؂�ւ����W�b�N���W��
    {
        //Debug.Log($"State changed from {currentState?.GetType().Name ?? "null"} to {newState.GetType().Name}");
        currentState?.ExitState(); // ���݂̏�Ԃ̏I���������Ăяo��
        currentState = newState; // �V������Ԃɐݒ�
        currentState.EnterState(); // �V������Ԃ̊J�n�������Ăяo��
    }

    public void Update() => currentState?.UpdateState(); // ��OOP: ���݂̏�ԃI�u�W�F�N�g�ɍX�V�������Ϗ��i�|�����[�t�B�Y���j

    public void FixedUpdate() // ��SRP: �����X�V������FixedUpdate�ɕ���
    {
        // ��OOP: �C���^�[�t�F�[�X�̃L���X�g�ɂ��A�����X�V���K�v�ȏ�Ԃ݂̂������i�|�����[�t�B�Y���j
        if (currentState is IEnemyPhysicsState phys)
            phys.FixedUpdateState();
    }

    public Enemy GetEnemy() => enemy; // �Ǘ��Ώۂ�Enemy����ԃN���X�ɒ�
}