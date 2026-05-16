using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Stats")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float gravityMultJumpHeight = 2f;
    public float gravityMultFalling = 2f;

    [Header("Hitbox Checkers")]
    [SerializeField] private List<SpriteRenderer> hitboxCheckers;

    [Header("Miscellanious")]
    private float moveInput;
    private bool isWalled;

    private Rigidbody2D rb;
    private float initialGravity;

    private GameObject platformChecker;

    private IPlayerActions playerActions;

    [SerializeField] private GameObject interact;

    void Awake()
    { 
        playerActions = FindObjectOfType<PlayerActions>();
        Debug.Log(playerActions);

        playerActions.SetIsGrounded(false);
        playerActions.SetIsAttacking(false);
        playerActions.SetIsDashing(false);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initialGravity = rb.gravityScale;

        foreach(SpriteRenderer checker in hitboxCheckers)
            checker.enabled = false;
        
        platformChecker = GameObject.Find("PlatformChecker");
    }

    void Update()
    {
        // Handle horizontal movement
        moveInput = Input.GetAxis("Horizontal");
            

        Flip();
        Fall();
    }

    private void FixedUpdate()
    {
        if (!isWalled && !playerActions.GetIsDashing())
        {
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        }
        if (rb.linearVelocity.y < 0 && !playerActions.GetIsGrounded())
        {
            playerActions.SetIsFalling(true);
            playerActions.SetIsJumping(false);
        }
    }

    private void OnMove(InputValue value)
    {
        var v = value.Get<Vector2>();
        if(v.x != 0)
            playerActions.SetIsWalking(true);
        if(v.x == 0)
            playerActions.SetIsWalking(false);
    }

    private void Flip()
    {
        // See if player is attacking
        if(!playerActions.GetIsAttacking() && !playerActions.GetIsInAnimation())
        {
            // Mirror if walking other way
            if(moveInput < 0 && !playerActions.GetIsLookingRight())
            {
                playerActions.SetIsLookingRight(true);
                transform.Rotate(Vector3.up * 180);
            }
            if(moveInput > 0 && playerActions.GetIsLookingRight())
            {
                playerActions.SetIsLookingRight(false);
                transform.Rotate(Vector3.up * 180);
            }
        }
    }

    private void OnJump()
    {
        // Handle jumping & double jumping
        if (playerActions.GetHasDoubleJump())
        {
            if(!playerActions.GetIsGrounded())
            {
                playerActions.SetHasDoubleJump(false);
            }

            playerActions.SetIsFalling(false);
            playerActions.SetIsJumping(true);

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

            StartCoroutine(CheckForSpace());
        }
    }

    private void OnPause()
    {
        GameObject menu = GameObject.Find("MenuManager");
        menu.GetComponent<ManageMenu>().OnPause();
    }

    private void OnInteract()
    {
        if(interact != null)
        {
            Interactable interactable = interact.GetComponent<Interactable>();
            Debug.Log(interactable);
            interactable.Interact();
        }    
    }

    IEnumerator CheckForSpace()
    {
        while (!playerActions.GetIsGrounded())
        {
            if (!Input.GetKey(KeyCode.Space) 
             && rb.linearVelocity.y > 0
            )
                rb.linearVelocity += Vector2.down * gravityMultJumpHeight * Time.deltaTime;
            yield return null;
        }
    }

    private void Fall()
    {
        // Disable PlatformChecker
        if(Input.GetKeyDown(KeyCode.S))
        {
            platformChecker.active = false;
        }

        // Double gravity scale when holding S in the air
        if(!playerActions.GetIsGrounded() && Input.GetKeyDown(KeyCode.S))
        {
            rb.gravityScale = rb.gravityScale * gravityMultFalling;
        }

        // Set gravity scale back to original
        if(Input.GetKeyUp(KeyCode.S))
        {
            rb.gravityScale = initialGravity;
            platformChecker.active = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Interactable"))
        {
            interact = other.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Interactable"))
        {
            interact = null;
        }
    }

    public void SetIsWalled(bool value)
    {
        isWalled = value;
    }
}