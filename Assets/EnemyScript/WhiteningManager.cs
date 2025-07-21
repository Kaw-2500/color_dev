using UnityEngine;

public class WhiteningManager
{
    public enum WhiteningStage { None, Slight, Moderate, Heavy, Complete }

    private WhiteningStage currentStage = WhiteningStage.None;
    private float maxHp;
    private float currentHp;

    private Color baseColor; // ← 初期色を保持

    public WhiteningManager(float maxHp, Color originalColor)
    {
        this.maxHp = maxHp;
        currentHp = maxHp;
        baseColor = originalColor; // 初期色を保存
    }

    public WhiteningStage CurrentStage => currentStage;

    public void UpdateHp(float newHp)
    {
        currentHp = Mathf.Clamp(newHp, 0f, maxHp);
        UpdateStage();
    }

    private void UpdateStage()
    {
        float hpRatio = currentHp / maxHp;

        if (hpRatio <= 0f)
            currentStage = WhiteningStage.Complete;
        else if (hpRatio <= 0.25f)
            currentStage = WhiteningStage.Heavy;
        else if (hpRatio <= 0.5f)
            currentStage = WhiteningStage.Moderate;
        else if (hpRatio <= 0.75f)
            currentStage = WhiteningStage.Slight;
        else
            currentStage = WhiteningStage.None;
    }

    public float SpeedMultiplier => currentStage switch
    {
        WhiteningStage.None => 0.8f,
        WhiteningStage.Slight => 1.0f,
        WhiteningStage.Moderate => 1.2f,
        WhiteningStage.Heavy => 1.5f,
        WhiteningStage.Complete => 0f,// 完全白化状態では移動不可
        _ => 1f,
    };

    public float AttackMultiplier => currentStage switch
    {
        WhiteningStage.None => 1f,
        WhiteningStage.Slight => 0.9f,
        WhiteningStage.Moderate => 0.7f,
        WhiteningStage.Heavy => 0.5f,
        WhiteningStage.Complete => 0f,
        _ => 1f,
    };

    public void UpdateWhiteColor(SpriteRenderer spriteRenderer)
    {
        Color targetColor = Color.white;

        float t = currentStage switch
        {
            WhiteningStage.None => 0f,
            WhiteningStage.Slight => 0.25f,
            WhiteningStage.Moderate => 0.5f,
            WhiteningStage.Heavy => 0.75f,
            WhiteningStage.Complete => 1f,
            _ => 0f,
        };

        // 初期色から白へ補間
        Color newColor = Color.Lerp(baseColor, targetColor, t);
        newColor.a = baseColor.a;//aを保存して、baseColorのrgb値を保持

        spriteRenderer.color = newColor;

        Debug.Log($"[Whitening] Stage: {currentStage}, Color RGB: ({newColor.r:F2}, {newColor.g:F2}, {newColor.b:F2})");
    }

}
