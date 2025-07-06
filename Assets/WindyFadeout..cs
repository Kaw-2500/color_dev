using UnityEngine;

// Windy（風弾）のフェードアウト処理
public class WindyFadeout : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private float fadeDuration;//すぐに上書きされるので初期値は不要
    private float timer = 0f;

    public void SetFadeDuration(float duration)
    {
        fadeDuration = duration;
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("WindyFadeout: SpriteRendererがありません。Windyプレハブに必ずアタッチしてください。");
            // 無理に動作させず安全に破棄
            Destroy(gameObject, fadeDuration);
        }
    }

    void Update()
    {
        if (spriteRenderer == null) return;

        timer += Time.deltaTime;

        // 透明度は0-1に絶対値で制限
        float alpha = Mathf.Clamp01(1f - (timer / fadeDuration));

        Color c = spriteRenderer.color;
        c.a = alpha;
        spriteRenderer.color = c;

        // fadeDuration経過で破棄
        if (timer >= fadeDuration)
        {
            Destroy(gameObject);
        }
    }
}
