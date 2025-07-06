using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class Talksystem : MonoBehaviour
{
    public static Talksystem Instance; // 他スクリプトから呼び出せるようにする

    [SerializeField] Text dialogueText;
    [SerializeField] GameObject Talksystem_base;
    [SerializeField] Text NameText;

    [SerializeField] FirstTalkcs firstTalkcs; // 最初のチュートリアル会話を管理するスクリプト
    [SerializeField] FirstSpeakUi FirstTalkUI; // チュートリアル会話のUIを管理するスクリプト


    private List<string> dialogue; // セリフ一覧（外から渡す）
    private int dialogNumber = 0;//今のセリフの番号
    public bool isTalking = false;

    private void Awake()
    {
        // 初期設定
        Talksystem_base.SetActive(false); // 最初は非表示
    }

    void Update()
    {
        if (!isTalking) return;//会話中でない場合は何もしない

        if (Input.GetKeyDown(KeyCode.Space))
        {
            dialogNumber++;

          

            if (dialogNumber < dialogue.Count)
            {
                dialogueText.text = dialogue[dialogNumber];

                if (!firstTalkcs.IsFnFirstSpeak)
                {
                    FirstTalkUI.NowDialogFirstCount++; // チュートリアル会話のカウントを増やす
                    Debug.Log("冒頭セリフの会話番号は: " + FirstTalkUI.NowDialogFirstCount);
                }
            }
            else//前のセリフが追加したセリフの数と同じだった場合
            {
                EndDialogue();
            }
        }
    }

    public void StartDialogue(List<string> newDialogue)//キャラからもらったセリフを受け取り、初期化、そののちにupdate関数で処理
    {
        dialogue = newDialogue;
        dialogNumber = 0;
        isTalking = true;
        Talksystem_base.SetActive(true);
        dialogueText.text = dialogue[dialogNumber];
    }

    public void EndDialogue()
    {
        firstTalkcs.IsFnFirstSpeak = true; //最初のチュートリアル会話が終わったことを記録する

        isTalking = false;
        Talksystem_base.SetActive(false);//次に会話システムが作動するまで非表示
    }

    public void SetName(string name)
    {
        NameText.text = name;  // 名前を表示するTextにセット
    }
}


