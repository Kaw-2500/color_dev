using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private PlayerMove playerMove;
    [SerializeField] private PlayerColorManager playerColorManager;
    [SerializeField] private PlayerStateManager playerStateManager;

    void Update()
    {
        if (playerStateManager.IsTalking) return;

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
            playerMove.PlayerJump(playerColorManager.GetCurrentData().jumpForce);
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
