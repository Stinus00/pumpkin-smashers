using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerDashing : MonoBehaviour
{

    [Header("Dodge Stats")]
    public float dodgeSpeed = 10f;
    public float dodgeTime = 1f;
    public float dodgeInvincibilityTimer = 5f;
    public float dodgeBackTime = 5f;
    private bool canDodge = true;
    private bool isDashing = false;

    [Header("Dash Stats")]
    public float dashSpeed = 20f;
    public float dashTime = 1f;
    [SerializeField] private bool canDash = true;
    
    [Header("Objects")]
    [SerializeField] private GameObject dodge;
    [SerializeField] private GameObject dash;
    [SerializeField] private PlayerHurtBox hurtBox;

    private Rigidbody2D rb2D;
    private Vector2 moveVelocity;
    float originalGravity = 3f;

    private IPlayerActions playerActions;

    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        playerActions = FindObjectOfType<PlayerActions>();
    }

    void OnDash()
    {
        if(!playerActions.GetIsAttacking() && !playerActions.GetIsInAnimation() && canDash)
        {
            if(playerActions.GetIsGrounded() && Input.GetAxisRaw("Horizontal") != 0)
                StartCoroutine("Dash");
            else if(canDodge)
                StartCoroutine("Dodge");
        }
    }

    IEnumerator Dash()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        moveVelocity = new Vector2(moveInput * dashSpeed, 0);
        rb2D.linearVelocity = moveVelocity;

        canDash = false;
        playerActions.SetIsDashing(true);

        dash.SetActive(true);

        yield return new WaitForSeconds(dashTime);
        canDash = true;

        playerActions.SetIsDashing(false);
        dash.SetActive(false);
    }

    IEnumerator Dodge()
    {
        rb2D.gravityScale = 0f;

        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if(Input.GetAxisRaw("Horizontal") != 0 && Input.GetAxisRaw("Vertical") != 0)
            moveVelocity = moveInput * dodgeSpeed * 0.7f;
        else
            moveVelocity = moveInput * dodgeSpeed;

        rb2D.linearVelocity = moveVelocity;

        canDodge = false;
        playerActions.SetIsDashing(true);

        hurtBox.AddInvincibility(dodgeInvincibilityTimer);
        
        dodge.SetActive(true);
        yield return new WaitForSeconds(dodgeTime);
        rb2D.gravityScale = originalGravity;

        playerActions.SetIsDashing(false);
        dodge.SetActive(false);
        yield return new WaitForSeconds(dodgeBackTime);
        canDodge = true;
    }

    public void StopDashImmediate()
    {
        rb2D.gravityScale = originalGravity;
        playerActions.SetIsDashing(false);
        dash.SetActive(false);
        dodge.SetActive(false);
    }
}
