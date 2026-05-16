using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeavyAttackSlam : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private GameObject hitboxHammer;
    [SerializeField] private List<GameObject> groundAttack;
    [SerializeField] private float shakeDuration = 0.1f;
    [SerializeField] private float gravityIncrease = 0.1f;
    
    [Header("Timing")]
    [SerializeField] private float airTiming = 0.1f;
    [SerializeField] private float groundTiming = 0.2f;
    [SerializeField] private float chargeSpeed = 1.5f;

    private GameObject camera;
    private Vector3 targetPosition;
    private Transform parent;
    private Rigidbody2D rb2D;
    private IEnumerator chargeCounting;
    [SerializeField] private float oldGravity = 3f;
    private float startGravity;
    private bool attacking;
    private bool isGrounded;
    private int impact;

    private IPlayerActions playerActions;

    void Awake()
    {
        parent = this.transform.parent;
        rb2D = parent.gameObject.GetComponent<Rigidbody2D>();
        camera = GameObject.Find("Main Camera");
        playerActions = FindObjectOfType<PlayerActions>();
    }

    void OnEnable()
    {
        attacking = false;
        startGravity = rb2D.gravityScale;
        rb2D.linearVelocity = new Vector2(0,0);
    }

    void FixedUpdate()
    {
        // Stop moving during attack
        rb2D.linearVelocity = new Vector2(0, rb2D.linearVelocity.y);
    }

    void Update()
    {
        if(isGrounded && playerActions.GetIsAttacking())
        {
            Debug.Log(rb2D.gravityScale);
            
            playerActions.SetIsGrounded(true);

            QuackeImpact();

            for(int i = 0; i < impact; i++)
            {
                StartCoroutine(ActivateHitboxAttack(groundAttack[i], 0.2f*i));
                StartCoroutine(DeactivateHitboxAttack(groundAttack[i], 0.4f + 0.2f*i));
            }

            StartCoroutine(Disable(0.3f*impact));
        }
        else
        {
            rb2D.gravityScale = rb2D.gravityScale + gravityIncrease;
        }
    }

    void QuackeImpact()
    {
        if(rb2D.gravityScale < 20)
        {
            impact = 1;
        }
        else if(rb2D.gravityScale < 30)
        {
            impact = 2;
        }
        else
        {
            impact = 3;
        }
    }

    IEnumerator ActivateHitboxAttack(GameObject attack, float delayTime = 0.0f)
    {
        yield return new WaitForSeconds(delayTime);
        camera.GetComponent<ShakeScreen>().TriggerShake(shakeDuration);
        attack.SetActive(true);
    }

    IEnumerator DeactivateHitboxAttack(GameObject attack, float delayTime = 0.0f)
    {
        yield return new WaitForSeconds(delayTime);
        attack.SetActive(false);
    }

    void OnCollisionEnter2D(Collision2D collision)
     {
         // Check if the player is on the ground
         if (collision.gameObject.CompareTag("Ground"))
             isGrounded = true;
     }

     void OnCollisionExit2D(Collision2D collision)
     {
         // Check if the player is no longer on the ground
         if (collision.gameObject.CompareTag("Ground"))
             isGrounded = false;
     }

     void CoyoteTime()
     {
         isGrounded = false;
     }

    void DeactivateHitboxVoid(GameObject attack)
    {
        attack.SetActive(false);
    }
    
    IEnumerator Disable(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
                
        rb2D.gravityScale = oldGravity;

        foreach(GameObject ground in groundAttack)
        {
            DeactivateHitboxVoid(ground);
        }

        playerActions.SetIsAttacking(false);

        gameObject.SetActive(false);
    }
}
