using UnityEngine;

public class PlayerStateManager : MonoBehaviour
    //status�N���X�̏����܂Ƃ߂āAbool,float�Ȃǂ̒l���Ǘ�����N���X
  �@//�����ɓ����̂́Aplayer�̑S�̂ɂ������A�M�d�ȏ��̂�
{
    [SerializeField] private CheckPlayerstatus checkPlayerStatus;
    [SerializeField] private Rigidbody2D rb2d;
    [SerializeField] private Talksystem talksystem;
    [SerializeField]private PlayerColorManager playerColorManager;

    [SerializeField] private float playerHp;

     PlayerbioState playerbioState = PlayerbioState.alive;

    [SerializeField]   public bool IsGround { get; private set; } = true;
    public bool IsJump { get; private set; } = false;
    public bool IsTalking => talksystem.isTalking;

    public float GroundYpos { get; private set; } = 0f;

    private bool isBlown = false;
    public bool IsBlown => isBlown;

    enum PlayerbioState
    {
        alive,
        dead
    }

    void Update()
    {
        UpdateGroundStatus();
    }

    private void UpdateGroundStatus()
    {
        GroundYpos = checkPlayerStatus.GroundYposCheck();
        bool nearGround = checkPlayerStatus.CheckNearFloor(GroundYpos);

        if (nearGround)
        {
            IsGround = true;
            IsJump = false;
        }
        else
        {
            IsGround = false;
        }
    }


    public float GetHp() => playerHp;
    public void ApplyDamage(float dmg)
    {
        // �h��͂�o�t������΂����ŏ���

        playerHp -= dmg * (1f - playerColorManager.GetCurrentData().defensePower);
        // ��: defensePower = 0.3�̎��A�_���[�W��30%����(dmg * 1 -0.3)
        playerHp = Mathf.Max(playerHp, 0); // HP�̉�������0

        if (playerHp <= 0f)
        {
            playerbioState = PlayerbioState.dead;
            Debug.Log("Player is dead");
            // ���S���̏���
        }
        else
        {
            playerbioState = PlayerbioState.alive;
            Debug.Log($"Player took damage, remaining HP: {playerHp}");
        }
    }



    public void SetJumping()
    {
        IsJump = true;
        IsGround = false;
    }

    public void SetBlown(bool value)
    {
        isBlown = value;
    }
}
