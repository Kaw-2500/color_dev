using UnityEditor.Build;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    [SerializeField] public GameObject Player; // �v���C���[�I�u�W�F�N�g


    public bool IsFnFirst;//�ŏ��̉�b���I���������ǂ���
    public float PlayerHp; // �v���C���[��HP
    public Vector2 PlayerPosition; // �v���C���[�̈ʒu
    public bool IsPlAttack; // �v���C���[���U��������


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
         
    }

    // Update is called once per frame
    void Update()
    {
        GetPlayerPos();
    }


    void GetPlayerPos()
    {
        if (!IsFnFirst)
        {
            return;
        }
        PlayerPosition = Player.transform.position; // �v���C���[�̈ʒu���擾
    }


}
