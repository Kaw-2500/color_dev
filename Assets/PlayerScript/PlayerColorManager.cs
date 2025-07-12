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

    private SpriteRenderer spriteRenderer;

    private PlayerColorState currentState;

      [Header("色データと参照")]
    public SpriteRenderer playerSpriteRenderer;
    public ChargeBar chargeBar;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer is not found on this GameObject.");
            return;
        }
        
           currentState = PlayerColorState.Red;// 初期状態をRedに設定
}

    public PlayerColorDataExtended GetCurrentData()
    {
        return currentState switch
        {
            PlayerColorState.Red => redData,
            PlayerColorState.Blue => blueData,
            PlayerColorState.Green => greenData,
            _ => throw new ArgumentOutOfRangeException()//ゲームの根底に関わるので、ここに到達した場合、例外を投げる
        };
    }

    public void ChangeColor(PlayerColorState newState,float chargetime)
    {
        if (currentState == newState) return;

        currentState = newState;
        Debug.Log($"Color: {GetCurrentData().displayColor}");


        // 色をここで変えている！！！
        playerSpriteRenderer.color = GetUnityColor(newState);

        // クールタイム開始
        chargeBar.ChangeCoolTime(newState,chargetime);
    }

    private Color GetUnityColor(PlayerColorState color)
    {
        return GetCurrentData().displayColor; // SOのdisplayColorを返すだけで済む
    }

    public PlayerColorState GetCurrentState()
    {
        return currentState;
    }
}
