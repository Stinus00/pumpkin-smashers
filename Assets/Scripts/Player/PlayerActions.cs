using UnityEngine;

public class PlayerActions : MonoBehaviour, IPlayerActions
{
    [SerializeField] private bool isAttacking;
    [SerializeField] private bool isDashing;
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool hasDoubleJump;
    [SerializeField] private bool isWalking;
    [SerializeField] private bool isFalling;
    [SerializeField] private bool isJumping;
    [SerializeField] private bool isInAnimation;
    [SerializeField] private bool isLookingRight;

    public bool GetIsAttacking()
    {
        return isAttacking;
    }
    public bool GetIsDashing()
    {
        return isDashing;
    }
    public bool GetIsGrounded()
    {
        return isGrounded;
    }
    public bool GetHasDoubleJump()
    {
        return hasDoubleJump;
    }
    public bool GetIsWalking()
    {
        return isWalking;
    }
    public bool GetIsJumping()
    {
        return isJumping;
    }
    public bool GetIsFalling()
    {
        return isFalling;
    }
    public bool GetIsInAnimation()
    {
        return isInAnimation;
    }
    public bool GetIsLookingRight()
    {
        return isLookingRight;
    }

    public void SetIsAttacking(bool value)
    {
        isAttacking = value;
    }
    public void SetIsDashing(bool value)
    {
        isDashing = value;
    }
    public void SetIsGrounded(bool value)
    {
        isGrounded = value;
        if(value)
            hasDoubleJump = true;
    }
    public void SetHasDoubleJump(bool value)
    {
        hasDoubleJump = value;
    }
    public void SetIsWalking(bool value)
    {
        isWalking = value;
    }
    public void SetIsJumping(bool value)
    {
        isJumping = value;
    }
    public void SetIsFalling(bool value)
    {
        isFalling = value;
    }
    public void SetIsInAnimation(bool value)
    {
        isInAnimation = value;
    }
    public void SetIsLookingRight(bool value)
    {
        isLookingRight = value;
    }
}
