using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;

public class BlueNormalAttack : MonoBehaviour
{

    //[SerializeField] private EventManager eventManager; // EventManagerの参照  

    private bool Attacked = false;

    private PlayerColorManager playerColorManager;

    Rigidbody2D rb2d;
    [SerializeField] bool Isfncharge = false;
   public float Speed;
    [SerializeField] float timer;
    [SerializeField] float Stucktimer;
    [SerializeField] float destroyTime = 5f;
    private GameObject Playerobj;

    private float direction = 1f; // 向きの初期値

    public float OffsetX;
    private Vector2 initialPosition;

    bool Isstuckcheck;
    private Vector2 Lastpos;
    private Vector2 Startpos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    void Start()
    {
        initialPosition = transform.position;
        Playerobj = GameObject.FindWithTag("Player"); // プレイヤーオブジェクトを取得

        playerColorManager = GameObject.FindWithTag("Player").GetComponent<PlayerColorManager>();

        rb2d = GetComponent<Rigidbody2D>(); 
        Stucktimer = 0; // タイマーをリセット 
    }
    void Update()
    {
     transform.localRotation = Quaternion.Euler(0, 0, 0); // 回転をリセット

        if (!Isfncharge)
        {
             FollowPlayer(); // プレイヤーを追従する関数を呼び出す
            rb2d.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY; // XとYをフリーズ  
        }

        if (!Isfncharge)
        {
            return;
        }

        rb2d.linearVelocity = new Vector2(Speed, rb2d.linearVelocity.y); // X軸の速度を設定

        rb2d.constraints = RigidbodyConstraints2D.FreezePositionY; // Y のみフリーズ
        timer += Time.deltaTime;
        Stucktimer += Time.deltaTime;

        Stuckthunder();

        if (timer >= destroyTime)
        {
            timer = 0; // タイマーをリセット
            Destroy(this.gameObject); // 自分を破壊
        }
    }

    void FollowPlayer()
    {
        Vector2 playerPosition = Playerobj.transform.position; // プレイヤーの位置を取得
        transform.position = new Vector2(playerPosition.x + OffsetX * direction, playerPosition.y); // プレイヤーの前に出るように位置を調整
    }

    void Stuckthunder()
    {
        if (Stucktimer >= 0.5)
        {
            Startpos = transform.position;
        }

        if (Stucktimer <= 1) return;
        
            Lastpos = transform.position; // 現在の位置を取得

            if (Vector2.Distance(Startpos, Lastpos) < 0.02f)
            {

                Destroy(this.gameObject); // 自分を破壊
                Debug.Log("スタックしている"); // スタックしている場合の処理を以下に記載
                Stucktimer = 0; // タイマーをリセット
            }
            else
            {     
                Stucktimer = 0; // タイマーをリセット
            }
    }

    void FinishCharge()
    {
        Isfncharge = true;

        Stucktimer = 0; // これも念のためリセット  
    }

    public void SetDirection(float newSpeed, float dir)
    {
        Speed = newSpeed;
        direction = dir;

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Sign(newSpeed) * Mathf.Abs(scale.x);
        transform.localScale = scale;

        transform.position = new Vector2(initialPosition.x + OffsetX * direction, initialPosition.y);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyHitDamage enemy = collision.gameObject.GetComponent<EnemyHitDamage>();
            if (Attacked) return;

            Debug.Log("Fireball hit an enemy: " + enemy.name);
            enemy.HitAttackDamageOnEnemy(playerColorManager.GetCurrentData().NormalAttackPower);
            Attacked = true; // 一度攻撃したらフラグを立てる

        }
        else if (collision.gameObject.tag == "Redfloor" ||
            collision.gameObject.tag == "Bluefloor" ||
            collision.gameObject.tag == "Greenfloor" ||
            collision.gameObject.tag == "naturalfloor")
        {

        }
        else
        {
       
        }
    }
}
