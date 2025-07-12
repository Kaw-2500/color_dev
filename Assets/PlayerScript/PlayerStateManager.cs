using UnityEngine;

public class PlayerStateManager : MonoBehaviour
    //status�N���X�̏����܂Ƃ߂āAbool,float�Ȃǂ̒l���Ǘ�����N���X
  �@//�����ɓ����̂́Aplayer�̑S�̂ɂ������A�M�d�ȏ��̂�
{
    [SerializeField] private CheckPlayerstatus checkPlayerStatus;
    [SerializeField] private Rigidbody2D rb2d;
    [SerializeField] private Talksystem talksystem;

   [SerializeField]   public bool IsGround { get; private set; } = true;
    public bool IsJump { get; private set; } = false;
    public bool IsTalking => talksystem.isTalking;

    public float GroundYpos { get; private set; } = 0f;

    void Update()
    {
        UpdateGroundStatus();
        Debug.Log(IsGround);
    }

    private void UpdateGroundStatus()
    {
        GroundYpos = checkPlayerStatus.GroundYposCheck();
        bool nearGround = checkPlayerStatus.CheckNearFloor(GroundYpos);
        float velocityY = rb2d.linearVelocity.y;

        if (nearGround)
        {
            IsJump = false;
            IsGround = true;
        }
        else if (velocityY < -0.05f)
        {
            IsGround = false;
        }
    }


    public void SetJumping()
    {
        IsJump = true;
        IsGround = false;
    }
}
