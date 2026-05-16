using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChangeSpriteAnimation : MonoBehaviour
{
    private IPlayerActions playerActions;
    [SerializeField] private Animator animator;

    void Awake()
    { 
        playerActions = FindObjectOfType<PlayerActions>();
    }

    void FixedUpdate()
    {
        animator.SetBool("isWalking", playerActions.GetIsWalking());
        animator.SetBool("isJumping", playerActions.GetIsJumping());
        animator.SetBool("isFalling", playerActions.GetIsFalling());
        animator.SetBool("isDashing", playerActions.GetIsDashing());
    }
}
