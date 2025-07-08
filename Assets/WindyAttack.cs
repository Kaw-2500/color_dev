using UnityEngine;

// 敵が一定間隔でWindy（風弾）をプレイヤー方向へ攻撃するスクリプト
public class WindyAttack : MonoBehaviour
{
    [SerializeField] private WeakEnemy weakEnemy; // 参照先の敵
    [SerializeField] private GameObject WindyPrefab; // 風弾プレハブ

    [SerializeField] private float fadeDuration = 2f; // 透明になるまでの時間
    [SerializeField] private float attackInterval = 2f; // 攻撃間隔
    [SerializeField] private float windySpeed = 10f; // 風弾の速度
    [SerializeField]private float InstantiatePositionX = 0f; // 風弾の生成位置X座標の微調整

    private float timer = 0f;

    private bool isPlayerRight = true;

    void Start()
    {
        if (weakEnemy == null)
        {
            Debug.LogError("WindyAttack: weakEnemyが設定されていません。");
        }
        if (WindyPrefab == null)
        {
            Debug.LogError("WindyAttack: WindyPrefabが設定されていません。");
        }
    }

    void Update()
    {
        if (weakEnemy == null || WindyPrefab == null) return;

        timer += Time.deltaTime;






        if(weakEnemy.GetCurrentState() != WeakEnemy.EnemyState.Attack) return;

        if (timer >= attackInterval)
        {
            timer = 0f;
            AttackWindy();
        }
    }

    private void AttackWindy()
    {


        CheckPlayerDirection();//Xpos微調整メソッドはこのメソッドに依存しているので必ず先に呼び出す必要があります。


        // 風弾の生成
        GameObject windyInstance = Instantiate(WindyPrefab, weakEnemy.transform.position, Quaternion.identity);


        Xposfinetuning(windyInstance);
        // プレイヤーの位置を確認して攻撃方向を決定

        Rigidbody2D rb = windyInstance.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("WindyAttack: WindyPrefabにRigidbody2Dがありません。");
            Destroy(windyInstance);
            return;
        }

        // 風弾をプレイヤー方向へ力で発射
        Vector2 forceDirection = isPlayerRight ? Vector2.right : Vector2.left;
        rb.AddForce(forceDirection * windySpeed, ForceMode2D.Impulse);

        // フェードアウト設定
        WindyFadeout fadeout = windyInstance.GetComponent<WindyFadeout>();
        if (fadeout != null)
        {
            fadeout.SetFadeDuration(fadeDuration);
        }
        else
        {
            Debug.LogWarning("WindyAttack: WindyPrefabにWindyFadeoutがアタッチされていません。");
    
            Destroy(windyInstance);
        }

        // 必要に応じてスプライトの左右反転などなど、、、
        //windyInstance.transform.localScale = new Vector3(isPlayerRight ? 1 : -1, 1, 1);
    }

    private void CheckPlayerDirection()
    {
        var playerPos = weakEnemy.GetPlayerRelativePosition();

        if (playerPos == WeakEnemy.PlayerRelativePosition.Right ||
            playerPos == WeakEnemy.PlayerRelativePosition.NearRight)
        {
            isPlayerRight = true;
        }
        else if (playerPos == WeakEnemy.PlayerRelativePosition.Left ||
                 playerPos == WeakEnemy.PlayerRelativePosition.NearLeft)
        {
            isPlayerRight = false;
        }
        else
        {
            Debug.LogWarning("WindyAttack: プレイヤーの位置が不明です。攻撃方向を決定できません。左と仮定");
         isPlayerRight = false; 
        }
    }

    private void Xposfinetuning(GameObject wind)
    {
      


        wind.transform.localScale = new Vector3(isPlayerRight ? 1 : -1, 1, 1);

         if (InstantiatePositionX == 0f)
        {
            Debug.LogWarning("WindyAttack: InstantiatePositionXが0です。生成元オブジェクトと座標が被っている可能性があります");
            return;
        }

            wind.transform.position = new Vector2(
            wind.transform.position.x + (isPlayerRight ? InstantiatePositionX : -InstantiatePositionX),
            wind.transform.position.y
        );
    }
}
