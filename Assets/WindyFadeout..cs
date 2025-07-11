using UnityEngine;

// Windy（風弾）のフェードアウト処理
public class WindyFadeout : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private float fadeDuration;//animationclipに上書きされます
    private float timer = 0f;

    [SerializeField] private float DamageAmount = 10f;

    [SerializeField] private AnimationClip fadeAnimationClip;

    private bool isAttacked = false; // 攻撃したかどうか

    GameObject plyobj; // プレイヤーの参照

    private Playeroperate playercs;

    public void SetFadeDuration()
    {
        if (fadeAnimationClip != null)
        {
            fadeDuration = fadeAnimationClip.length;
            //Debug.Log($"WindyFadeout: fadeAnimationClipの長さは {fadeDuration} 秒です。");
        }
        else
        {
            Debug.LogWarning("WindyFadeout: fadeAnimationClipがセットされていません。0.5秒と仮定します。");
            fadeDuration = 0.5f;
        }



    }


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }
        if (spriteRenderer == null)
        {
            Debug.LogError("WindyFadeout: SpriteRendererが見つかりません。");
            Destroy(gameObject);
            return;
        }

        plyobj = GameObject.FindGameObjectWithTag("Player");
        if (plyobj == null)
        {
            Debug.LogError("WindyFadeout: Playerタグがついたオブジェクトが見つかりません。");
            Destroy(gameObject);
            return;
        }

        playercs = plyobj.GetComponent<Playeroperate>();
        if (playercs == null)
        {
            Debug.LogError("WindyFadeout: Playeroperate コンポーネントが見つかりません。");
            Destroy(gameObject);
            return;
        }
    }

    void Update()
    {
        if (spriteRenderer == null) return;

        timer += Time.deltaTime;

        float alpha = Mathf.Clamp01(1f - (timer / fadeDuration));

        Color c = spriteRenderer.color;
        c.a = alpha;
        spriteRenderer.color = c;

        if (timer >= fadeDuration)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Debug.Log("WindyFadeout: プレイヤーに衝突しました。ダメージを送信します。");
            SendDamage();
            playercs.HitWindEffectDamage(); // ヒットエフェクトを再生
        }
    }

    void SendDamage()
    {
        if (playercs == null || playercs.PlayerHp <= 0 || isAttacked) return;

        playercs.Hitdamage(DamageAmount);

        isAttacked = true; // 攻撃済みフラグを立てる
    }

}