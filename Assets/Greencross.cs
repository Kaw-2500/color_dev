using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Greencross : MonoBehaviour, IAttackStrategy
{
    [SerializeField] private float startxpos;

    private bool Attacked = false;
    private PlayerColorManager playerColorManager;
    private GameObject owner;
    private float direction = 1f; // 向きの初期値

    private float ScaleX; // スケールのX成分を保持するための変数
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    void Awake()
    {
                ScaleX = transform.localScale.x; // 初期スケールのX成分を保存
    }
    
    public void Execute(GameObject Owner, float dir)
    {
        // 先にownerをセットしないとNullReferenceになる
        owner = Owner;
        direction = dir;

        if (owner == null)
        {
            Debug.LogError("Executeに渡されたownerがnullです。");
            return;
        }

        playerColorManager = owner.GetComponent<PlayerColorManager>();
        if (playerColorManager == null)
        {
            Debug.LogError("PlayerColorManagerが見つかりません。owner: " + owner.name);
            return;
        }

        Vector2 newpos = new Vector2(transform.position.x + startxpos * direction, transform.position.y);
        transform.position = newpos;

         transform.localScale = new Vector3(ScaleX * direction,
                                  transform.localScale.y, 
                                  transform.localScale.z); // 向きに応じてスケールを調整

        this.transform.SetParent(owner.transform);
    }

    void FinishAttackDestroy()//animationから呼ぶ
    {
        this.transform.SetParent(null); // 親を解除してから破壊
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyHitDamage enemy = collision.gameObject.GetComponent<EnemyHitDamage>();
            if (Attacked) return;

            Debug.Log("Fireball hit an enemy: " + enemy.name);
            enemy.HitAttackDamageOnEnemy(playerColorManager.GetCurrentData().NormalAttackPower);
            Attacked = true; // 一度攻撃したらフラグを立てる

        }
        else
        {
        
        }
    }
}
