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

    public bool IsColorChangeCool { get; private set; } = false;

    void Start()
    {
        if (playerSpriteRenderer == null)
            playerSpriteRenderer = GetComponent<SpriteRenderer>();

        if (playerSpriteRenderer == null)
            Debug.LogError("PlayerColorManager: SpriteRendererÇ™Ç†ÇËÇ‹ÇπÇÒ");

        currentState = PlayerColorState.Red;
        ChangeColor(currentState);// èâä˙èÛë‘ÇRedÇ…ê›íË
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

    public void ChangeColor(PlayerColorState newState)
    {
        if (currentState == newState) return;

        currentState = newState;
        ApplyColor();

        if (chargeBar != null)
        {
            var data = GetCurrentData();
            chargeBar.ChangeCoolTime(newState, data.chargeCoolTime);
            IsColorChangeCool = true;
            // ÉNÅ[ÉãäÆóπéûÇ…ChargeBarë§Ç©ÇÁPlayerColorManager.ResetColorChangeCool()ÇåƒÇ‘ëzíË
        }
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

    public void ResetColorChangeCool()
    {
        IsColorChangeCool = false;
    }
}
