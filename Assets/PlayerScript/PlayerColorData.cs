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
    public float chargeCoolTime;        // �N�[���^�C���i�b
    public float defensePower;          // �h���(0.3�Ȃ�A�_���[�W��30%����������)
    public float levity;                // �m�b�N�o�b�N���̂͂�����鋭��(�y��)(1�Œʏ�A1.5��1.5�{�̌y���A0.5�Ŕ����̌y��)
    public float NormalAttackCoolTime; // �ʏ�U���̃N�[���^�C���i�b�j
    public float NormalAttackPower;     // �ʏ�U���̃_���[�W��
    [Header("���F������̃_���[�W��")]
    public float hitDamageOnFloor;      // ���ɂ��_���[�W��

    [Header("�F���L�̊�{�s���v���ӂ���")]
    public GameObject NormalattackPrefab; // �U���G�t�F�N�g�v���n�u



}
