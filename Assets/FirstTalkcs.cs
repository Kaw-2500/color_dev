using System.Collections.Generic;
using UnityEngine;

public class FirstTalkcs : MonoBehaviour
{
    public bool IsFnFirstSpeak = false;

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
 

        if (IsFnFirstSpeak) return; //最初のチュートリアル会話が終わっている場合は何もしない
        talkSystem.isTalking = true; //会話中にする
        startdialog();
         
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


  
}
