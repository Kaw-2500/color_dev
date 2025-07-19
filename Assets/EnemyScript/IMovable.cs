using UnityEngine;

public interface IMovable //オブジェクトの「移動」という振る舞いを抽象化するインターフェース
{
    void Move(Vector2 direction); // 指定された方向に移動するメソッド
    void Stop(); // 移動を停止するメソッド
}