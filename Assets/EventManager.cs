using System.Collections;
using UnityEditor.Build;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    [SerializeField] public GameObject Player; // �v���C���[�I�u�W�F�N�g


    public bool IsFnFirst;//�ŏ��̉�b���I���������ǂ���
    public float PlayerHp; // �v���C���[��HP
    public Vector2 PlayerPosition; // �v���C���[�̈ʒu
    public bool IsPlAttack; // �v���C���[���U��������

    [SerializeField] private float deadcamerawaittime = 1.0f; // ���S�J�����̑ҋ@����

    [SerializeField] private PlayerStateManager playerStateManager; // �v���C���[�̏�Ԃ��Ǘ�����X�N���v�g
    [SerializeField] private Camerapos camerapos; // �J�����̈ʒu���Ǘ�����X�N���v�g


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

    public void Deadevent()
    {
        Time.timeScale = 0.1f; // �X���[�J�n
        StartCoroutine(DeadZoomSequence());
    }

    private IEnumerator DeadZoomSequence()
    {
        // ���S���̃J�����Y�[���J�n�i���A���^�C�����Ԃœ����j
        yield return StartCoroutine(camerapos.PanAndZoomCoroutine(3.0f, deadcamerawaittime));

        // �Y�[���I����Ɏ��Ԃ�߂�
        Time.timeScale = 1.0f;
    }

}
