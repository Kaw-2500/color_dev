using UnityEngine;

public class Fireball : MonoBehaviour
{

  private EventManager eventManager; // EventManagerの参照  

    [SerializeField]private float startxpos = 0.9f; // プレイヤーとの距離を調整するためのX座標の微調整

 private PlayerColorManager playerColorManager;

    public float speed = 10f;

    private bool Attacked = false;

    public float timer;
    public float destroyTime = 5f; // �΂̋ʂ̎���

    Rigidbody2D rb2d;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        transform.position = new Vector2(transform.position.x + startxpos, transform.position.y); // プレイヤーとの距離を調整

      

        playerColorManager = GameObject.FindWithTag("Player").GetComponent<PlayerColorManager>();
        eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
        if (eventManager == null)
        {
            Debug.LogError("EventManager not found in the scene.");
        }
        else
        {
            //eventManager.IsPlAttack = true; // プレイヤーの攻撃状態を設定
        }


    }

    void Update()
    {
        rb2d.linearVelocity = new Vector2(speed, 0);
        timer += Time.deltaTime;

        if (timer >= destroyTime)
        {
            timer = 0;
            Destroy(this.gameObject); 
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
       EnemyHitDamage enemy =  collision.gameObject.GetComponent<EnemyHitDamage>();
            if (Attacked) return;

            Debug.Log("Fireball hit an enemy: " + enemy.name);
            enemy.HitAttackDamageOnEnemy(playerColorManager.GetCurrentData().NormalAttackPower);
            Attacked = true; // 一度攻撃したらフラグを立てる

        }
        else
        {
            Destroy(this.gameObject);
        }
    }


}

