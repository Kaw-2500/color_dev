using UnityEngine;

public class PlayerStateManager : MonoBehaviour//statusクラスの情報をまとめて、bool,floatなどの値を管理するクラス
                                               //ここに入れるのは、playerの全体にかかわる、貴重な情報のみ
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
    }

    private void UpdateGroundStatus()
    {
        GroundYpos = checkPlayerStatus.GroundYposCheck();

        bool nearGround = checkPlayerStatus.CheckNearFloor(GroundYpos);

        IsGround = Mathf.Abs(rb2d.linearVelocity.y) > 0.05f;

        if (nearGround)
        {
            IsJump = false;
            IsGround = true;
        }
        else if (!nearGround && rb2d.linearVelocity.y < 0)
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
