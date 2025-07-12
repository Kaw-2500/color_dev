using System;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerColorManager : MonoBehaviour
{
    //�F�̃f�[�^��ێ�����N���X�ł��B
    //�F�̃f�[�^�̏������s���A�F�؂�ւ��̏�������(PlayerInputHandler�̖��)�A�����I�ȓ���(PlayerInputhandler&move�̖��)�͍s���܂���B
    //UI�̏����͂��̃N���X�̒S���ł����AUI�ɓn���̂�currentData�݂̂ł��B
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
            Debug.LogError("PlayerColorManager: SpriteRenderer������܂���");

        currentState = PlayerColorState.Red;
        GetCurrentData(); // ������Ԃ̃f�[�^���擾���Ă���
        ApplyColor();
        Debug.Log("Start����jumpForce: " + GetCurrentData().jumpForce);
        Debug.Log("start����gravity: " + GetCurrentData().gravityScale);
        Debug.Log("sTART����JUMPFORCE: " + GetCurrentData().jumpForce);
    }

    public PlayerColorDataExtended GetCurrentData()//������ĂԂƍ��̐F�̃f�[�^��Ԃ��Ă����
    {
        return currentState switch
        {
            PlayerColorState.Red => redData,
            PlayerColorState.Blue => blueData,
            PlayerColorState.Green => greenData,
            _ => throw new ArgumentOutOfRangeException()// �F�؂�ւ��̓Q�[���̍���Ɋւ��̂ŁA��O��������N���b�V��������
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
            // �N�[����������ChargeBar������PlayerColorManager.ResetColorChangeCool()���Ăԑz��
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

    public void ResetColorChangeCool()//statemanager�ɓ���Ȃ��A���R�͐F�̋����ɂ����ւ��Ȃ�����B
    {
        IsColorChangeCool = false;
    }

}
