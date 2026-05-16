using UnityEngine;

public interface IPlayerActions
{
    bool GetIsAttacking();
    bool GetIsDashing();
    bool GetIsGrounded();
    bool GetHasDoubleJump();
    bool GetIsWalking();
    bool GetIsJumping();
    bool GetIsFalling();
    bool GetIsInAnimation();
    bool GetIsLookingRight();

    void SetIsAttacking(bool value);
    void SetIsDashing(bool value);
    void SetIsGrounded(bool value);
    void SetHasDoubleJump(bool value);
    void SetIsWalking(bool value);
    void SetIsJumping(bool value);
    void SetIsFalling(bool value);
    void SetIsInAnimation(bool value);
    void SetIsLookingRight(bool value);
}
