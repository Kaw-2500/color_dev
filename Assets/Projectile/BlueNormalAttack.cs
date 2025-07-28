using UnityEngine;
using UnityEngine.UIElements;

public class BlueNormalAttack : MonoBehaviour, IAttackStrategy, Icolor
{
    private bool Attacked = false;

    private PlayerColorManager playerColorManager;

    private float ScaleX; // スケールのX成分を保持するための変数

    Rigidbody2D rb2d;
    private bool Isfncharge = false;
    public float Speed;
    [SerializeField] float timer;
    [SerializeField] float Stucktimer;
    [SerializeField] float destroyTime = 5f;

    private GameObject owner;

    private bool IsInitial = false;

    private float direction = 1f; // 向きの初期値

    [SerializeField] private float startxpos = 0.9f;

    private Vector2 Lastpos;
    private Vector2 Startpos;

    void Awake()
    {
        IsInitial = false; // 初期はfalseに
        ScaleX = transform.localScale.x; // 初期スケールのX成分を保存
    }

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        Stucktimer = 0; // タイマーをリセット 
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
        IsInitial = true;  // Update処理を開始するためにフラグON
    }

    void Update()
    {
        if (!IsInitial) return;

        transform.localRotation = Quaternion.Euler(0, 0, 0); // 回転をリセット

        if (!Isfncharge)
        {
            FollowPlayer(); // プレイヤーを追従する関数を呼び出す
            rb2d.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY; // XとYをフリーズ  
            return; // ここで止める
        }

        rb2d.linearVelocity = new Vector2(Speed * direction, rb2d.linearVelocity.y); // X軸の速度を設定

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
        if (owner == null)
        {
            Debug.LogError("Player not found. Make sure the player has the 'Player' tag.");
            return;
        }
        Vector2 playerPosition = owner.transform.position; // プレイヤーの位置を取得

        transform.position = new Vector2(playerPosition.x + startxpos * direction,
            playerPosition.y); // プレイヤーの前に出るように位置を調整
    }

    void Stuckthunder()
    {
        if (Stucktimer >= 0.5f)
        {
            Startpos = transform.position;
        }

        if (Stucktimer <= 1f) return;

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

    void FinishCharge()//animationイベントから呼び出される関数
    {
        Isfncharge = true;

        Stucktimer = 0; // これも念のためリセット  
    }

    public ThisColor ReturnThisColor()
    {
        return ThisColor.Blue; // BlueNormalAttackは青属性
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyHitDamage enemy = collision.gameObject.GetComponent<EnemyHitDamage>();
            if (Attacked) return;
            Debug.Log($"BlueNormalAttack color: {ReturnThisColor()}"); // 攻撃色を出力
            enemy.HitAttackDamageOnEnemy(playerColorManager.GetCurrentData().NormalAttackPower, ReturnThisColor());
            Attacked = true; // 一度攻撃したらフラグを立てる
        }
    }
}
