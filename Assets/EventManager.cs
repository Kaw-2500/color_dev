using System.Collections;
using UnityEditor.Build;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    [SerializeField] public GameObject Player; // プレイヤーオブジェクト


    public bool IsFnFirst;//最初の会話が終了したかどうか
    public float PlayerHp; // プレイヤーのHP
    public Vector2 PlayerPosition; // プレイヤーの位置
    public bool IsPlAttack; // プレイヤーが攻撃したか

    [SerializeField] private float deadcamerawaittime = 1.0f; // 死亡カメラの待機時間

    [SerializeField] private PlayerStateManager playerStateManager; // プレイヤーの状態を管理するスクリプト
    [SerializeField] private Camerapos camerapos; // カメラの位置を管理するスクリプト


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

    public void Deadevent()
    {
        Time.timeScale = 0.1f; // スロー開始
        StartCoroutine(DeadZoomSequence());
    }

    private IEnumerator DeadZoomSequence()
    {
        // 死亡時のカメラズーム開始（リアルタイム時間で動く）
        yield return StartCoroutine(camerapos.PanAndZoomCoroutine(3.0f, deadcamerawaittime));

        // ズーム終了後に時間を戻す
        Time.timeScale = 1.0f;
    }

}
