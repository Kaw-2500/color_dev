using UnityEngine;

public class PlayerInputHandler : MonoBehaviour

//ここは、inputManagerからの入力を受け取り、条件分岐するクラスです。
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private PlayerMove playerMove;
    [SerializeField] private PlayerColorManager playerColorManager;
    [SerializeField] private PlayerStateManager playerStateManager;
    [SerializeField] private PlayerAttack playerAttack;
    [SerializeField] private PlayerAttackCooltimeManager playerAttackCooltimeManager;

    void Update()
    {
        if (playerStateManager.IsTalking) return;

        if (inputManager.NormalAttack && !playerAttackCooltimeManager.IsOnCooldown(playerColorManager.GetCurrentState()))
        {
            playerAttack.normalAttack(playerColorManager.GetCurrentData().NormalattackPrefab);
            playerAttackCooltimeManager.StartCooldown(playerColorManager.GetCurrentState());
        }

        if (!playerColorManager.IsColorChangeCool)
        {
            if (inputManager.ChangeToRed)
                playerColorManager.ChangeColor(PlayerColorManager.PlayerColorState.Red);
            else if (inputManager.ChangeToBlue)
                playerColorManager.ChangeColor(PlayerColorManager.PlayerColorState.Blue);
            else if (inputManager.ChangeToGreen)
                playerColorManager.ChangeColor(PlayerColorManager.PlayerColorState.Green);
        }

        if (inputManager.JumpPress && playerStateManager.IsGround)
        {
            //Debug.Log("PlayerInputHandler: Jump Pressed");
            playerMove.PlayerJump(playerColorManager.GetCurrentData().jumpForce,playerColorManager.GetCurrentData().gravityScale);
            playerStateManager.SetJumping();
        }
    }

    void FixedUpdate()
    {
        if (playerStateManager.IsTalking) return;

        float horizontalInput = inputManager.Horizontal;
        var data = playerColorManager.GetCurrentData();

        if (horizontalInput != 0f && Mathf.Abs(playerMove.CurrentVelocity.x) < data.maxSpeed)
        {
            playerMove.HandleMove(data.runForce, data.maxSpeed, data.gravityScale, horizontalInput);
        }
    }
}
