using UnityEngine;

public class YellowSlashManager : MonoBehaviour , IAttackComponent
{
    [SerializeField] private float damageAmount = 10f;

    private bool isAttacked = false;
    Rigidbody2D rb2d;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()//初期化処理はAwakeで行う
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Init(Vector2 direction, float force, float yOffset, float damage)
    {
        rb2d.position = new Vector2(rb2d.position.x, rb2d.position.y + yOffset);
        rb2d.AddForce(direction.normalized * force, ForceMode2D.Impulse);
            damageAmount = damage;
    }



    void Destoroy()//animationEventから呼び出される
    {
        Destroy(gameObject);
    }

   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isAttacked) return;
        if (!collision.gameObject.CompareTag("Player")) return;
        if(collision.gameObject.GetComponent<PlayerStateManager>() == null) Debug.LogWarning("yellow: PlayerStateManager component not found on player.");


        Debug.Log($"yellow: OnTriggerEnter2D called with collision: {collision.gameObject.name}");
        var hitDamage = collision.gameObject.GetComponent<PlayerHitDamage>();
        if (hitDamage != null)
        {
            // 攻撃者（風）のX座標とプレイヤーのX座標を比較
            float directionX = transform.position.x > collision.transform.position.x ? -1f : 1f;

            Debug.Log($"yellow: PlayerHitDamage component found on player. Applying damage: {damageAmount}");
            hitDamage.OnHitDamage(damageAmount);

            isAttacked = true;
        }
        else
        {
            Debug.LogWarning("yellow: PlayerHitDamage component not found on player.");
        }
    }
}
