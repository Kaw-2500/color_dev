using UnityEngine;
using UnityEngine.UIElements;

public class Fireball : MonoBehaviour, IAttackStrategy
{
    [SerializeField] private float startxpos = 0.9f;
    [SerializeField] public float speed = 10f;
    [SerializeField] public float destroyTime = 5f;

    private float timer;
    private bool Attacked = false;

    private PlayerColorManager playerColorManager;
    private Rigidbody2D rb2d;


    private float ScaleX; // スケールのX成分を保持するための変数

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        ScaleX = transform.localScale.x; // 初期スケールのX成分を保存
    }

    public void Execute(GameObject owner, float dir)
    {
        playerColorManager = owner.GetComponent<PlayerColorManager>();
        if (playerColorManager == null)
        {
            Debug.LogError("PlayerColorManagerが見つかりません。owner: " + owner.name);
            return;
        }

        Vector2 newpos = new Vector2(transform.position.x + startxpos * dir, transform.position.y);
        transform.position = newpos;
        rb2d.linearVelocity = new Vector2(speed * dir, 0);

        transform.localScale = new Vector3(ScaleX * dir,
                                     transform.localScale.y,
                                     transform.localScale.z); // 向きに応じてスケールを調整
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= destroyTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !Attacked)
        {
            if (playerColorManager == null)
            {
                Debug.LogError("playerColorManager is null in OnCollisionEnter2D");
                return;
            }

            var enemy = collision.gameObject.GetComponent<EnemyHitDamage>();
            if (enemy != null)
            {
                enemy.HitAttackDamageOnEnemy(playerColorManager.GetCurrentData().NormalAttackPower);
                Attacked = true;
            }
        }

        Destroy(gameObject);
    }
}
