using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Greencross : MonoBehaviour
{
    [SerializeField] private float startxpos;

    private bool Attacked = false;
    private PlayerColorManager playerColorManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.SetParent(GameObject.FindWithTag("Player").transform); // プレイヤーの子オブジェクトにする
        transform.Translate(Vector3.right * startxpos); //プレイヤーの前に出るようにする

        playerColorManager = GameObject.FindWithTag("Player").GetComponent<PlayerColorManager>();



    }

    // Update is called once per frame
    void Update()
    {

    }

    void FinishAttackDestroy()//animationから呼ぶ
    {
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
