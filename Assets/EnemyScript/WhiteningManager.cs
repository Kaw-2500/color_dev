using UnityEngine;

public class WhiteningManager
{
    public enum WhiteningStage { None, Slight, Moderate, Heavy, Complete }

    private WhiteningStage currentStage = WhiteningStage.None;
    private float maxHp;
    private float currentHp;

    public WhiteningManager(float maxHp)
    {
        this.maxHp = maxHp;
        currentHp = maxHp;
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
        WhiteningStage.Complete => 1.7f,
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
}
