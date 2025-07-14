using UnityEngine;

public class PlayerStateManager : MonoBehaviour
    //statusクラスの情報をまとめて、bool,floatなどの値を管理するクラス
  　//ここに入れるのは、playerの全体にかかわる、貴重な情報のみ
{
    [SerializeField] private CheckPlayerstatus checkPlayerStatus;
    [SerializeField] private Rigidbody2D rb2d;
    [SerializeField] private Talksystem talksystem;
    [SerializeField]private PlayerColorManager playerColorManager;

    [SerializeField] private float playerHp;

     PlayerbioState playerbioState = PlayerbioState.alive;

    TouchGroundColor touchGroundColor = TouchGroundColor.natural;

    public bool IsDead => playerbioState == PlayerbioState.dead;


    public TouchGroundColor CurrentTouchGroundColor => touchGroundColor;

    [SerializeField]   public bool IsGround { get; private set; } = true;
    public bool IsJump { get; private set; } = false;
    public bool IsTalking => talksystem.isTalking;

    public float GroundYpos { get; private set; } = 0f;

    private bool isBlown = false;
    public bool IsBlown => isBlown;

  public   enum PlayerbioState
    {
        alive,
        dead
    }

    public enum TouchGroundColor
    {
        natural,
        red,
        blue,
        green
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
        // 防御力やバフがあればここで処理

        playerHp -= dmg * (1f - playerColorManager.GetCurrentData().defensePower);
        // 例: defensePower = 0.3の時、ダメージは30%減少(dmg * 1 -0.3)
        playerHp = Mathf.Max(playerHp, 0); // HPの下限をは0

        if (playerHp <= 0f)
        {
            playerbioState = PlayerbioState.dead;
            Debug.Log("Player is dead");
            // 死亡時の処理
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

    private void OnCollisionStay2D(Collision2D collision)
    {
        string tag = collision.collider.tag;

        if (tag == "Redfloor")
            touchGroundColor = TouchGroundColor.red;
        else if (tag == "Bluefloor")
            touchGroundColor = TouchGroundColor.blue;
        else if (tag == "Greenfloor")
            touchGroundColor = TouchGroundColor.green;
        else
            touchGroundColor = TouchGroundColor.natural;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        string tag = collision.collider.tag;

        if (tag == "Redfloor" || tag == "Bluefloor" || tag == "Greenfloor")
        {
            touchGroundColor = TouchGroundColor.natural;
        }
    }

}
