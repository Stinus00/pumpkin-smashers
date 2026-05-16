using UnityEngine;
using System.Collections;

public class PlayerAttacking : MonoBehaviour
{    
    [Header("Controls")] 
    public KeyCode upKey = KeyCode.W;
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;
    public KeyCode downKey = KeyCode.S;

    [Header("Hitboxes Ground")]
    [SerializeField] private GameObject hitBoxNeutral;
    [SerializeField] private GameObject hitBoxHorizontal;
    [SerializeField] private GameObject hitBoxDown;

    [Header("Hitboxes Air")]
    [SerializeField] private GameObject hitBoxNeutralAir;
    [SerializeField] private GameObject hitBoxHorizontalAir;
    [SerializeField] private GameObject hitBoxDownAir;

    [Header("Hitboxes Heavy")]
    [SerializeField] private GameObject heavyHitboxHammer;
    [SerializeField] private GameObject heavyHitboxSlam;
    [SerializeField] private GameObject heavyHitboxSpin;

    private Vector3 targetPosition;
    private KeyCode lastHitKey;
    private bool initialGravity;
    private Rigidbody2D rb2D;

    private IPlayerActions playerActions;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        playerActions = FindObjectOfType<PlayerActions>();
    }

    void OnLightAttack()
    {
        if(playerActions.GetIsAttacking() || playerActions.GetIsDashing() || playerActions.GetIsInAnimation())
        {
            return;
        }

        bool isGrounded = playerActions.GetIsGrounded();

            if(isGrounded)
            {
                // Neutral Attack if attacking and upkey is held
                if(Input.GetKey(upKey))
                {
                    hitBoxNeutral.SetActive(true);
                }
                // Down Attack if attacking and downkey is held
                else if(Input.GetKey(downKey))
                {
                    hitBoxDown.SetActive(true);
                    // StartCoroutine(ActivateHitboxAttack(hitBoxDown, 0.0f));
                    // StartCoroutine(DeactivateHitboxAttack(hitBoxDown, downDelay));
                    // Debug.Log("Down ATTACK!");
                    // StartAnimation("DownGround");
                }
                // Side Attack if attacking and horizontal movement key is held
                else if(Input.GetKey(leftKey) || Input.GetKey(rightKey))
                {
                    hitBoxHorizontal.SetActive(true);
                    // StartCoroutine(ActivateHitboxAttack(hitBoxHorizontal, 0.0f));
                    // StartCoroutine(DeactivateHitboxAttack(hitBoxHorizontal, horizontalDelay));
                    // Debug.Log("Side ATTACK!");
                    // StartAnimation("SideGround");
                }
                // Neutral Attack if attacking
                else
                {
                    hitBoxNeutral.SetActive(true);
                } 
            }
            if(!isGrounded)
            {
                // Neutral Air Attack if attacking and upkey is held while airborne
                if(Input.GetKey(upKey))
                {
                    hitBoxNeutralAir.SetActive(true);
                } 
                // Down Air Attack if attacking and downkey is held while airborne
                else if(Input.GetKey(downKey))
                {
                    hitBoxDownAir.SetActive(true);
                }
                // Side Air Attack if attacking and horizontal movement key is held while airborne
                else if(Input.GetKey(leftKey) || Input.GetKey(rightKey))
                {
                    hitBoxHorizontalAir.SetActive(true);
                }
                // Neutral Air Attack if attacking while airborne
                else
                {
                    hitBoxNeutralAir.SetActive(true);
                } 
            }
    }

    void OnHeavyAttack()
    {
        if(playerActions.GetIsAttacking() || playerActions.GetIsDashing() || playerActions.GetIsInAnimation())
        {
            return;
        }

        bool isGrounded = playerActions.GetIsGrounded();

        // Donkey Kong Hammering
        if(isGrounded && (Input.GetKey(upKey) || Input.GetKey(downKey)))
        {
            // Charge up for longer hammering (1/3/5)
            heavyHitboxHammer.SetActive(true);
        }
        // Big Ground Slam
        // !! DISABLED !!
        // else if(!isGrounded && (Input.GetKey(upKey) || Input.GetKey(downKey)))
        // {
        //     // Longer falling = bigger earthquake = bigger damage
        //     StartCoroutine(ActivateHitboxAttack(heavyHitboxSlam));
        //     Debug.Log("Heavy Slam");
        // }
        // Big Spin Attack
        else if(Input.GetKey(leftKey) || Input.GetKey(rightKey))
        {
            // Charge up for longer spinning (2/4/6/8)
            heavyHitboxSpin.SetActive(true);
            Debug.Log("Heavy Spin");
        }
        else if(isGrounded)
        {
            // Charge up for longer hammering (1/3/5)
            heavyHitboxHammer.SetActive(true);
            Debug.Log("Heavy Hammering");
        }
        else if(!isGrounded)
        {
            // Longer falling = bigger earthquake = bigger damage
            heavyHitboxSpin.SetActive(true);
            Debug.Log("Heavy Slam");
        }
    }
}
