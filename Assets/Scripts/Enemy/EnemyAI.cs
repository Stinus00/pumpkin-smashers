using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum State
    {
        Idle,
        Spotted,
        Stunned
    }
    
    CapsuleCollider myCapsuleCollider;
    Rigidbody2D myRigidbody2D;
    
    [Header("Stats")] 
    [Range(5.0f, 9.0f)]
    [SerializeField] private float enemySpeed;
    [Range(2.0f, 25.0f)]
    [SerializeField] private float jumpHeight;
    [SerializeField] private float enemyStunTime = 3f;
    [SerializeField] private State currentState = State.Idle;
    [Range(0.0f, 2.5f)]
    [SerializeField] private float secondsToWait = 0f;
    [Range(0.0f, 2.5f)]
    [SerializeField] private float secondsToWaitJump = 3f;

    [SerializeField] private bool small = false;

    [Header("Stun")]
    [SerializeField] private int currentStunCounter;
    [SerializeField] private int maxStunCounter;
    [SerializeField] private float stunTimer;

    [Header("Animator")]
    [SerializeField] private Animator animator;

    private bool enemyInCameraView = false;
    private bool isPlayerOnRight;
    private bool enemyMovementStarted = false;
    bool firstTimeJump = true;
    [SerializeField] private bool isGrounded;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myCapsuleCollider = GetComponent<CapsuleCollider>();
        myRigidbody2D = GetComponent<Rigidbody2D>();

        if(small)
        {
            enemySpeed = UnityEngine.Random.Range(5.0f, 9.0f);
            jumpHeight = UnityEngine.Random.Range(15.0f, 25.0f);
            secondsToWaitJump = UnityEngine.Random.Range(1f, 2.5f);
        }
        

        if(!small)
        {
            enemySpeed = UnityEngine.Random.Range(5.0f, 8.0f);
            jumpHeight = UnityEngine.Random.Range(4.0f, 8.0f);
            secondsToWait = UnityEngine.Random.Range(0.1f, 2.5f);
            secondsToWaitJump = UnityEngine.Random.Range(0.1f, 2.5f);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = false;
            stopEnemyMovementCycle();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "LightAttackHitbox")
        {
            currentState = State.Stunned;
            StopCoroutine("StartEnemyStunTime");
            StartCoroutine("StartEnemyStunTime");
        }
    }

    private IEnumerator StartEnemyStunTime()
    {
        animator.SetTrigger("Stun");
        yield return new WaitForSecondsRealtime(enemyStunTime);
        animator.ResetTrigger("Stun");
        animator.SetTrigger("StopStun");
        firstTimeJump = true;
        currentState = State.Spotted;
    }

    private IEnumerator startEnemyMovementCycle()
    {
        if (!small)
        {
            if (!firstTimeJump)
            {
                yield return new WaitForSeconds(secondsToWait);
            }
            else {
                yield return new WaitForSeconds(0.1f);
                firstTimeJump = false;
            }
        }
        else {
            yield return new WaitForSeconds(secondsToWait);
        }
        Debug.Log(secondsToWait);
        yield return StartCoroutine("moveEnemy");
    }

    private IEnumerator moveEnemy()
    {
        Debug.Log("Moving enemy");
        // animator.SetTrigger("Jump");
        Vector2 movementForce = transform.up * jumpHeight;
        Vector2 movementForceRight = transform.right * enemySpeed;
        Vector2 movementForceLeft = -transform.right * enemySpeed;
        movementForce = isPlayerOnRight ? movementForce + movementForceRight : movementForce + movementForceLeft;
        myRigidbody2D.AddForce(movementForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(secondsToWaitJump);
        yield return StartCoroutine("startEnemyMovementCycle");
    }

    private void checkEnemyInCameraView()
    {
        Vector3 viewPosition = Camera.main.WorldToViewportPoint(transform.position);
        if (((viewPosition.x > 0 && viewPosition.x < 1 && viewPosition.y > 0 && viewPosition.y < 1) || enemyInCameraView) && currentState == State.Idle)
        {
            currentState = State.Spotted;
            enemyInCameraView = true;
        }
    }

    private void moveTowardsPlayer()
    {
        Vector3 playerPosition = GameObject.Find("Player").transform.position;
        Vector3 selfPosition = transform.position;
        isPlayerOnRight = playerPosition.x > selfPosition.x;
        if (!enemyMovementStarted)
        {
            StartCoroutine("startEnemyMovementCycle");
            enemyMovementStarted = true;
        }
    }

    private void stopEnemyMovementCycle()
    {
        StopCoroutine("startEnemyMovementCycle");
        StopCoroutine("moveEnemy");
        enemyMovementStarted = false;
    }

    public State GetCurrentState()
    {
        return currentState;
    }

    // Update is called once per frame
    void Update()
    {
        checkEnemyInCameraView();
        switch (currentState)
        {
            case State.Idle:
                // Debug.Log("Idle");
                break;
            case State.Spotted:
                // Debug.Log("Spotted");
                if(isGrounded)
                {
                    moveTowardsPlayer();
                }
                break;
            case State.Stunned:
                // Debug.Log("Stunned");
                stopEnemyMovementCycle();
                break;
        }
    }
}
