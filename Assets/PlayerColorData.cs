using UnityEngine;

[CreateAssetMenu(fileName = "PlayerColorDataExtended", menuName = "Player/ColorDataExtended")]
public class PlayerColorDataExtended : ScriptableObject
{
    [Header("��{�X�e�[�^�X")]
    public Color displayColor;          // �v���C���[�̌����ڂ̐F
    public float maxSpeed;              // �ő呖�鑬��
    public float runForce;              // �����x
    public float gravityScale;          // �d�͔{��
    public float jumpForce;             // �W�����v��
    public string colorName;            // �F�̖��O�Ȃ�

    [Header("���F������̃_���[�W��")]
    public float hitDamageOnFloor;      // ���ɂ��_���[�W��

    [Header("�F���L�̊�{�s���v���ӂ���")]
    public GameObject attackEffectPrefab; // �U���G�t�F�N�g�v���n�u
}
