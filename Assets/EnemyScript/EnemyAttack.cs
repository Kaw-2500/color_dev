using UnityEngine;

public class EnemyAttack : IAttackable // ��SRP: IAttackable�C���^�[�t�F�[�X���������A�u�U���v�̋�̓I�ȃ��W�b�N�݂̂ɐӔC������
{
    private GameObject normalAttackPrefab; // �ʏ�U���̃v���n�u��ێ�����t�B�[���h
    private Enemy enemy; // Enemy�̎Q�Ƃ�ێ�����t�B�[���h

    public EnemyAttack(GameObject normalprefab,Enemy enemycs)
    {
        normalAttackPrefab = normalprefab;
        enemy = enemycs; // Enemy�̎Q�Ƃ�ێ�
    }

    public void Attack()
    {
        Debug.Log("Enemy Attack!");

        Vector3 spawnPos = enemy.transform.position;
        float offsetX = 0f;

        switch (enemy.GetPlayerRelativePosition())
        {
            case Enemy.PlayerRelativePosition.NearRight:
            case Enemy.PlayerRelativePosition.Right:
                offsetX = enemy.GetNormalAttackOffsetX();
                break;
            case Enemy.PlayerRelativePosition.NearLeft:
            case Enemy.PlayerRelativePosition.Left:
                offsetX = -enemy.GetNormalAttackOffsetX();
                break;
            default:
                offsetX = 0f;
                Debug.LogWarning("EnemyAttack: �v���C���[�̈ʒu���s���ł��B�U���ʒu�𒆉��ɐݒ肵�܂��B");
                break;
        }

        spawnPos += new Vector3(offsetX, 0, 0);

        GameObject attackInstance = Object.Instantiate(normalAttackPrefab, spawnPos, Quaternion.identity);

     
    }
}