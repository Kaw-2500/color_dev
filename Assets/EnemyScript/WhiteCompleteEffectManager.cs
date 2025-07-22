using UnityEngine;
using System.Collections;

public class WhiteCompleteEffectManager : MonoBehaviour
{
    [SerializeField] private GameObject whiteCompleteEffectPrefab; // 完全白化エフェクトのプレハブ

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void WhiteCompleteEffect(Transform targetTransform)
    {
        GameObject whiteExplosion = Instantiate(whiteCompleteEffectPrefab, targetTransform.position, Quaternion.identity);    }


    public IEnumerator FadeOutCoroutine(SpriteRenderer spriteRenderer, GameObject targetObject)
    {
        float duration = 0.5f;
        float elapsed = 0f;

        Color originalColor = spriteRenderer.color;

        while (elapsed < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }

        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);

    
        Destroy(targetObject);
    }


}
