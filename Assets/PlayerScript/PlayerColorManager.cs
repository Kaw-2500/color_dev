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
            Debug.LogError("PlayerColorManager: SpriteRendererがありません");

        currentState = PlayerColorState.Red;
        ChangeColor(currentState);// 初期状態をRedに設定
        ApplyColor();
    }

    public PlayerColorDataExtended GetCurrentData()//これを呼ぶと今の色のデータを返してくれる
    {
        return currentState switch
        {
            PlayerColorState.Red => redData,
            PlayerColorState.Blue => blueData,
            PlayerColorState.Green => greenData,
            _ => throw new ArgumentOutOfRangeException()// 色切り替えはゲームの根底に関わるので、例外が来たらクラッシュさせる
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
            chargeBar.ChangeCoolTime(newState, data.chargeCoolTime,data.displayColor);
            IsColorChangeCool = true;
            // クール完了時にChargeBar側からPlayerColorManager.ResetColorChangeCool()を呼ぶ想定
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

    public void ResetColorChangeCool()//statemanagerに入れない、理由は色の挙動にしか関わらないから。
    {
        IsColorChangeCool = false;
    }
}
