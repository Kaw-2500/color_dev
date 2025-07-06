using System.Collections.Generic;
using UnityEngine;

public class FirstTalkcs : MonoBehaviour
{
    public bool IsFnFirstSpeak = false;

    // base���A�^�b�`����悤�̔�
    [SerializeField] GameObject Talksystem_base;

    // �Z���t�̓��e�����X�g�^�ɑ��
    [SerializeField] List<string> myTalk;

    //talksystem.cs���A�^�b�`����悤�̔�
    [SerializeField] Talksystem talkSystem;

    //���̃L�����̖��O����͂��邽�߂̔�
    [SerializeField] new string name;


  
    void Start()
    {
        // �ŏ��͎����̃Z���tUI���\���ɂ��Ă���
        Talksystem_base.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        dialogcheck();
    }

    void dialogcheck()
    {

        if (talkSystem.isTalking) return;
 

        if (IsFnFirstSpeak) return; //�ŏ��̃`���[�g���A����b���I����Ă���ꍇ�͉������Ȃ�
        talkSystem.isTalking = true; //��b���ɂ���
        startdialog();
         
    }

    void startdialog()
    {
        //�Z���t��UI��\������
        Talksystem_base.SetActive(true);

        //talksystem.cs��name��n��
        talkSystem.SetName(name);

        //talksystemname.cs�ɃZ���t�̓��e��n��
        talkSystem.StartDialogue(myTalk);

    }


  
}
