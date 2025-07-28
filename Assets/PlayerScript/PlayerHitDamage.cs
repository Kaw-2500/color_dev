using System.Collections;
using UnityEditor;
using UnityEngine;
using static PlayerColorManager;
using static PlayerStateManager;

public class PlayerHitDamage : MonoBehaviour
{
    [SerializeField] PlayerStateManager playerStateManager;
    [SerializeField] private PlayerColorManager playerColorManager;
    private GameObject player;

    [SerializeField] private float groundDamageInterval = 4; // 床ダメージのタイマー

    private float groundDamageTimer = 0f; // ← クラスフィールドに移動

    Rigidbody2D rb2d;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = this.gameObject;

        rb2d = player.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        TryHitGroundDamage();
    }

    public void OnHitDamage(float damage)
    {
        playerStateManager.ApplyDamage(damage);
    }

    public void ApplyWindKnockback(Vector2 knockback)
    {
        if (playerStateManager.IsBlown) return;

        float levity;

        levity = playerColorManager.GetCurrentData().levity;

        knockback.x *= levity; // 風の吹き飛ばし力に軽さを反映
        knockback.y *= levity; // 風の吹き飛ばし力に軽さを反映

        // 吹き飛ばしの物理的な力を加える
        rb2d.AddForce(knockback, ForceMode2D.Impulse);

        float torqueImpulse = knockback.x * -0.2f; //0.2が回転係数の最適解
        rb2d.AddTorque(torqueImpulse, ForceMode2D.Impulse);

        playerStateManager.SetBlown(true);
        StartCoroutine(ResetBlown(0.5f));
    }

    private IEnumerator ResetBlown(float delay)
    {
        yield return new WaitForSeconds(delay);
        playerStateManager.SetBlown(false);
    }

   private void TryHitGroundDamage()
{
    if (playerStateManager.IsDead) return;
    if (!playerStateManager.IsGround) return;

    var currentColor = playerColorManager.GetCurrentState();
    var groundColor = playerStateManager.CurrentTouchGroundColor;

    bool isDamageColor = false;

    switch (currentColor)
    {
        case PlayerColorState.Red:
            isDamageColor = (groundColor == TouchGroundColor.blue);
            break;
        case PlayerColorState.Blue:
            isDamageColor = (groundColor == TouchGroundColor.green);
            break;
        case PlayerColorState.Green:
            isDamageColor = (groundColor == TouchGroundColor.red);
            break;
    }

    if (!isDamageColor)
    {
        groundDamageTimer = 0f;
        Debug.Log("ダメージはゆかから食らわない");
        return;
    }

    groundDamageTimer += Time.deltaTime;
    if (groundDamageTimer < groundDamageInterval) return;

    float damage = playerColorManager.GetCurrentData().hitDamageOnFloor;
    playerStateManager.ApplyDamage(damage);
    Debug.Log("床ダメ食らわせる");
    groundDamageTimer = 0f;
}


}