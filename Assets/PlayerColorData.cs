using UnityEngine;

[CreateAssetMenu(fileName = "PlayerColorDataExtended", menuName = "Player/ColorDataExtended")]
public class PlayerColorDataExtended : ScriptableObject
{
    [Header("基本ステータス")]
    public Color displayColor;          // プレイヤーの見た目の色
    public float maxSpeed;              // 最大走る速さ
    public float runForce;              // 加速度
    public float gravityScale;          // 重力倍率
    public float jumpForce;             // ジャンプ力
    public string colorName;            // 色の名前など

    [Header("同色床からのダメージ量")]
    public float hitDamageOnFloor;      // 床によるダメージ量

    [Header("色特有の基本行動プレふぁぶ")]
    public GameObject attackEffectPrefab; // 攻撃エフェクトプレハブ
}
