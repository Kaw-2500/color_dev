using UnityEngine;
using UnityEngine.Rendering;

public class BlueNormalAttack : MonoBehaviour
{

    [SerializeField] private EventManager eventManager; // EventManagerの参照  


    Rigidbody2D rb2d;
    [SerializeField] bool Isfncharge = false;
    [SerializeField] float Speed;
    [SerializeField] float timer;
    [SerializeField] float Stucktimer;
    [SerializeField] float destroyTime = 5f;
    private GameObject Playerobj;

    bool Isstuckcheck;
    private Vector2 Lastpos;
    private Vector2 Startpos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    void Start()
    {

        Playerobj = GameObject.Find("Player");


        rb2d = GetComponent<Rigidbody2D>(); 
        Stucktimer = 0; // タイマーをリセット
        transform.position = new Vector2(transform.position.x + 1.5f, transform.position.y); // プレイヤーとの距離を調整  

      

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

    // Update is called once per frame
    void Update()
    {
        transform.transform.localRotation = Quaternion.Euler(0, 0, 0); // 回転をリセット

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
        transform.position = new Vector2(playerPosition.x + 1f, playerPosition.y); // プレイヤーの前に出るように位置を調整
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Player"))
        {

        }
        else if (collision.gameObject.tag == "Redfloor" ||
            collision.gameObject.tag == "Bluefloor" ||
            collision.gameObject.tag == "Greenfloor" ||
            collision.gameObject.tag == "naturalfloor")
        {

        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
