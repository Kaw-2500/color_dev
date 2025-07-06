using System.Collections.Generic;
using UnityEngine;

public class npc : MonoBehaviour
{
    public bool Istouch;//プレイヤーとの接触判定用


    // baseをアタッチするようの箱
    [SerializeField] GameObject Talksystem_base;

    // セリフの内容をリスト型に代入
    [SerializeField] List<string> myTalk;

    //talksystem.csをアタッチするようの箱
    [SerializeField] Talksystem talkSystem;

    //このキャラの名前を入力するための箱
    [SerializeField] new string name; 


  
    void Start()
    {
        // 最初は自分のセリフUIを非表示にしておく
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
        //セリフのUIを表示する
        Talksystem_base.SetActive(true);

        //talksystem.csにnameを渡す
        talkSystem.SetName(name);

        //talksystemname.csにセリフの内容を渡す
        talkSystem.StartDialogue(myTalk);

    }

    private void OnCollisionEnter2D(Collision2D collision)//プレイヤーとの接触判定用
    {
        if(collision.gameObject.CompareTag("Player")) 
        {
            Istouch = true; // 接触フラグを立てる
        }
    }

    private void OnCollisionExit2D(Collision2D collision)//プレイヤーとの接触判定用
    {
     if(collision.gameObject.CompareTag("Player")) 
        {
            Istouch = false; 
          
        }
    }
}
