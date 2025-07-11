using UnityEngine;

public class InputManager : MonoBehaviour
{
    // 横方向の入力: -1（左）〜0〜1（右）
    public float Horizontal { get; private set; }

    // ジャンプした瞬間だけtrueになるフラグ
    public bool JumpPress { get; private set; }

    void Update()
    {
        // 横移動入力取得
        Horizontal = (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) ? 1f :
                     (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) ? -1f : 0f;

        // ジャンプボタン判定
        JumpPress = (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow));
    }
}
