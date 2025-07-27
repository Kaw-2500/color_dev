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
    public float chargeCoolTime;        // クールタイム（秒
    public float defensePower;          // 防御力(0.3なら、ダメージを30%減少させる)
    public float levity;                // ノックバック時のはじかれる強さ(軽さ)(1で通常、1.5で1.5倍の軽さ、0.5で半分の軽さ)
    public float NormalAttackCoolTime; // 通常攻撃のクールタイム（秒）
    public float NormalAttackPower;     // 通常攻撃のダメージ量
    [Header("同色床からのダメージ量")]
    public float hitDamageOnFloor;      // 床によるダメージ量

    [Header("色特有の基本行動プレふぁぶ")]
    public GameObject NormalattackPrefab; // 攻撃エフェクトプレハブ



}
