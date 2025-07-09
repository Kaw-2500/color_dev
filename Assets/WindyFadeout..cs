using UnityEngine;

// Windy（風弾）のフェードアウト処理
public class WindyFadeout : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private float fadeDuration;//animationclipに上書きされます
    private float timer = 0f;

    [SerializeField] private AnimationClip fadeAnimationClip;  

    public void SetFadeDuration()
    {
        if (fadeAnimationClip != null)
        {
            fadeDuration = fadeAnimationClip.length;
            Debug.Log($"WindyFadeout: fadeAnimationClipの長さは {fadeDuration} 秒です。");
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
            Debug.LogError("WindyFadeout: SpriteRendererが見つかりません。Windyプレハブに必ずアタッチしてください。");
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
}