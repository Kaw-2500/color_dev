using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class Talksystem : MonoBehaviour
{
    public static Talksystem Instance; // ���X�N���v�g����Ăяo����悤�ɂ���

    [SerializeField] Text dialogueText;
    [SerializeField] GameObject Talksystem_base;
    [SerializeField] Text NameText;

    [SerializeField] FirstTalkcs firstTalkcs; // �ŏ��̃`���[�g���A����b���Ǘ�����X�N���v�g
    [SerializeField] FirstSpeakUi FirstTalkUI; // �`���[�g���A����b��UI���Ǘ�����X�N���v�g


    private List<string> dialogue; // �Z���t�ꗗ�i�O����n���j
    private int dialogNumber = 0;//���̃Z���t�̔ԍ�
    public bool isTalking = false;

    private void Awake()
    {
        // �����ݒ�
        Talksystem_base.SetActive(false); // �ŏ��͔�\��
    }

    void Update()
    {
        if (!isTalking) return;//��b���łȂ��ꍇ�͉������Ȃ�

        if (Input.GetKeyDown(KeyCode.Space))
        {
            dialogNumber++;

          

            if (dialogNumber < dialogue.Count)
            {
                dialogueText.text = dialogue[dialogNumber];

                if (!firstTalkcs.IsFnFirstSpeak)
                {
                    FirstTalkUI.NowDialogFirstCount++; // �`���[�g���A����b�̃J�E���g�𑝂₷
                    Debug.Log("�`���Z���t�̉�b�ԍ���: " + FirstTalkUI.NowDialogFirstCount);
                }
            }
            else//�O�̃Z���t���ǉ������Z���t�̐��Ɠ����������ꍇ
            {
                EndDialogue();
            }
        }
    }

    public void StartDialogue(List<string> newDialogue)//�L���������������Z���t���󂯎��A�������A���̂̂���update�֐��ŏ���
    {
        dialogue = newDialogue;
        dialogNumber = 0;
        isTalking = true;
        Talksystem_base.SetActive(true);
        dialogueText.text = dialogue[dialogNumber];
    }

    public void EndDialogue()
    {
        firstTalkcs.IsFnFirstSpeak = true; //�ŏ��̃`���[�g���A����b���I��������Ƃ��L�^����

        isTalking = false;
        Talksystem_base.SetActive(false);//���ɉ�b�V�X�e�����쓮����܂Ŕ�\��
    }

    public void SetName(string name)
    {
        NameText.text = name;  // ���O��\������Text�ɃZ�b�g
    }
}


