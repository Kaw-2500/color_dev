using UnityEditor.Build;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    [SerializeField] public GameObject Player; // プレイヤーオブジェクト


    public bool IsFnFirst;//最初の会話が終了したかどうか
    public float PlayerHp; // プレイヤーのHP
    public Vector2 PlayerPosition; // プレイヤーの位置
    public bool IsPlAttack; // プレイヤーが攻撃したか


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
        PlayerPosition = Player.transform.position; // プレイヤーの位置を取得
    }


}
