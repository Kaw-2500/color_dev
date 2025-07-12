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

      [Header("�F�f�[�^�ƎQ��")]
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
        
           currentState = PlayerColorState.Red;// ������Ԃ�Red�ɐݒ�
}

    public PlayerColorDataExtended GetCurrentData()
    {
        return currentState switch
        {
            PlayerColorState.Red => redData,
            PlayerColorState.Blue => blueData,
            PlayerColorState.Green => greenData,
            _ => throw new ArgumentOutOfRangeException()//�Q�[���̍���Ɋւ��̂ŁA�����ɓ��B�����ꍇ�A��O�𓊂���
        };
    }

    public void ChangeColor(PlayerColorState newState,float chargetime)
    {
        if (currentState == newState) return;

        currentState = newState;
        Debug.Log($"Color: {GetCurrentData().displayColor}");


        // �F�������ŕς��Ă���I�I�I
        playerSpriteRenderer.color = GetUnityColor(newState);

        // �N�[���^�C���J�n
        chargeBar.ChangeCoolTime(newState,chargetime);
    }

    private Color GetUnityColor(PlayerColorState color)
    {
        return GetCurrentData().displayColor; // SO��displayColor��Ԃ������ōς�
    }

    public PlayerColorState GetCurrentState()
    {
        return currentState;
    }
}
