using System;
using UnityEngine;

public class PlayerColorManager : MonoBehaviour
{
    public enum PlayerColorState
    {
        Red,
        Blue,
        Green
    }

    [SerializeField] private PlayerColorDataExtended redData;
    [SerializeField] private PlayerColorDataExtended blueData;
    [SerializeField] private PlayerColorDataExtended greenData;

    [SerializeField] private SpriteRenderer playerSpriteRenderer;
    [SerializeField] private ChargeBar chargeBar;

    private PlayerColorState currentState;

    void Start()
    {
        if (playerSpriteRenderer == null)
            playerSpriteRenderer = GetComponent<SpriteRenderer>();

        if (playerSpriteRenderer == null)
            Debug.LogError("PlayerColorManager: SpriteRenderer‚ª‚ ‚è‚Ü‚¹‚ñ");

        currentState = PlayerColorState.Red;
        ApplyColor();
    }

    public PlayerColorDataExtended GetCurrentData()
    {
        return currentState switch
        {
            PlayerColorState.Red => redData,
            PlayerColorState.Blue => blueData,
            PlayerColorState.Green => greenData,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public void ChangeColor(PlayerColorState newState, float chargeTime)
    {
        if (currentState == newState) return;

        currentState = newState;
        ApplyColor();

        if (chargeBar != null)
            chargeBar.ChangeCoolTime(newState, chargeTime);
    }

    private void ApplyColor()
    {
        if (playerSpriteRenderer != null)
        {
            playerSpriteRenderer.color = GetCurrentData().displayColor;
        }
    }

    public PlayerColorState GetCurrentState()
    {
        return currentState;
    }
}
