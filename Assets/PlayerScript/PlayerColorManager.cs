using System;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerColorManager : MonoBehaviour
{
    //色のデータを保持するクラスです。
    //色のデータの処理を行い、色切り替えの条件分岐(PlayerInputHandlerの役目)、物理的な動作(PlayerInputhandler&moveの役目)は行いません。
    //UIの処理はこのクラスの担当ですが、UIに渡すのはcurrentDataのみです。
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
        GetCurrentData(); // 初期状態のデータを取得しておく
        ApplyColor();
        Debug.Log("Start時のjumpForce: " + GetCurrentData().jumpForce);
        Debug.Log("start時のgravity: " + GetCurrentData().gravityScale);
        Debug.Log("sTARTじのJUMPFORCE: " + GetCurrentData().jumpForce);
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
            chargeBar.ChangeCoolTime(data);
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
