using UnityEngine;

public class WindyFadeout : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private float fadeDuration;
    private float timer = 0f;
    [SerializeField] private float knockbackForceX = 5f;
    [SerializeField] private float knockbackForceY = 5f;
    [SerializeField] private AnimationClip fadeAnimationClip;

    private bool isAttacked = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (fadeAnimationClip != null)
            fadeDuration = fadeAnimationClip.length;
        else
            fadeDuration = 0.5f;
    }

    private void Update()
    {
        if (spriteRenderer == null) return;

        timer += Time.deltaTime;
        float alpha = Mathf.Clamp01(1f - (timer / fadeDuration));
        var c = spriteRenderer.color;
        c.a = alpha;
        spriteRenderer.color = c;

        if (timer >= fadeDuration) Destroy(gameObject);
    }
    public void SetFadeDuration()
    {
        if (fadeAnimationClip != null)
        {
            fadeDuration = fadeAnimationClip.length;
        }
        else
        {
            Debug.LogWarning("WindyFadeout: fadeAnimationClipがセットされていません。0.5秒と仮定します。");
            fadeDuration = 0.5f;
        }
    }

    private void UpYpos()//animationから呼び出す。なぜなら
    //途中でanimationが少し大きくなるので、床に添わせるようにするためです、
    {
     Rigidbody2D rb2d = GetComponent<Rigidbody2D>();
     rb2d.position = new Vector2(rb2d.position.x, rb2d.position.y + 2.8f); // Y座標を0.2上げる
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isAttacked) return;
        if (!collision.gameObject.CompareTag("Player")) return;

        var hitDamage = collision.gameObject.GetComponent<PlayerHitDamage>();
        if (hitDamage != null)
        {
            // 攻撃者（風）のX座標とプレイヤーのX座標を比較
            float directionX = transform.position.x > collision.transform.position.x ? -1f : 1f;

            Vector2 knockback = new Vector2(directionX * Mathf.Abs(knockbackForceX), knockbackForceY);

           WindyAttack windyAttack = this.GetComponent<WindyAttack>();

            hitDamage.OnHitDamage(windyAttack.GetDamage());
            hitDamage.ApplyWindKnockback(knockback);

            isAttacked = true;
        }
        else
        {
            Debug.LogWarning("WindyFadeout: PlayerHitDamage component not found on player.");
        }
    }
}
