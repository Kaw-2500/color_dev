using UnityEngine;

public class EnemyAttack : IAttackable // ��SRP: IAttackable�C���^�[�t�F�[�X���������A�u�U���v�̋�̓I�ȃ��W�b�N�݂̂ɐӔC������
{
    public void Attack() // IAttackable�C���^�[�t�F�[�X��Attack���\�b�h�̎���
    {
        Debug.Log("Enemy Attack!"); // �����_�ł͍U�����O���o�͂��邾��
        // �����I�ɂ́A�U���A�j���[�V�����̍Đ��A�_���[�W�����ASE�Đ��Ȃǂ̃��W�b�N�������ɒǉ������
    }
}