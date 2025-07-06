using System.Collections.Generic;
using UnityEngine;

public class npc : MonoBehaviour
{
    public bool Istouch;//�v���C���[�Ƃ̐ڐG����p


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

        if (!Istouch) return; 

        if (Input.GetKeyDown(KeyCode.M))
        {
            startdialog();
        }
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

    private void OnCollisionEnter2D(Collision2D collision)//�v���C���[�Ƃ̐ڐG����p
    {
        if(collision.gameObject.CompareTag("Player")) 
        {
            Istouch = true; // �ڐG�t���O�𗧂Ă�
        }
    }

    private void OnCollisionExit2D(Collision2D collision)//�v���C���[�Ƃ̐ڐG����p
    {
     if(collision.gameObject.CompareTag("Player")) 
        {
            Istouch = false; 
          
        }
    }
}
