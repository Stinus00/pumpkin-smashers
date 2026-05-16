using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class MinibossEnemyAI : MonoBehaviour
{
    public enum State
    {
        Idle,
        Attack,
        Exhausted,
        Stunned
    }
    CapsuleCollider myCapsuleCollider;
    Rigidbody2D myRigidbody2D;
    GameObject bossLanding;

    [Header("Stats")]
    [SerializeField] float enemySpeed = 5f;
    [SerializeField] float jumpHeight = 5f;
    [SerializeField] float enemyExhaustTime = 3f;
    [SerializeField] State currentState = State.Idle;
    [SerializeField] float secondsToWait = 0.001f;
    [SerializeField] float secondsToWaitJump = 0.01f;
    [SerializeField] float enemyGravityScale = 5;

    [Header("Stun")]
    [SerializeField] private int currentStunCounter = 0;
    [SerializeField] private int maxStunCounter = 5;
    [SerializeField] private float stunTimer = 4.5f;

    [Header("Animator")]
    [SerializeField] private Animator animator;
    [SerializeField] private float toIdleAnimationDuration = 1f;

    [SerializeField] bool enemyTriggered = false;
    bool isPlayerOnRight;
    bool enemyMovementStarted = false;
    bool isGrounded;
    [SerializeField] bool enemyExhausted = false;
    int timesJumped = 0;
    float bossLandingPositionZ;
    bool readyToBeExhausted = false;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip landClip;
    [SerializeField] private AudioClip snoringClip;
    [SerializeField] private AudioClip stunnedClip;
    [SerializeField] private AudioClip wakeupClip;


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
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BossLanding")
        {
            bossLanding.SetActive(false);
            audioSource.clip = landClip;
            audioSource.Play();
        }
    }

    IEnumerator StartEnemyMovementCycle()
    {
        yield return new WaitForSecondsRealtime(secondsToWait);
        if (isGrounded)
        {
            yield return StartCoroutine("MoveEnemy");
        }
        else
        {
            yield return StartCoroutine("StartEnemyMovementCycle");
        }
    }

    IEnumerator MoveEnemy()
    {
        Debug.Log("Moving enemy");
        animator.SetTrigger("Jump");
        Vector2 movementForce = transform.up * jumpHeight;
        myRigidbody2D.AddForce(movementForce, ForceMode2D.Impulse);
        timesJumped++;
        yield return new WaitForSecondsRealtime(secondsToWaitJump);
        myRigidbody2D.linearVelocity = Vector2.zero;
        myRigidbody2D.gravityScale = 0;
        PredictFallLocation();
        yield return new WaitForSecondsRealtime(secondsToWaitJump);
        myRigidbody2D.gravityScale = enemyGravityScale;
        if (timesJumped >= 3)
        {
            enemyMovementStarted = false;
            readyToBeExhausted = true;
        }
        else
        {
            yield return StartCoroutine("StartEnemyMovementCycle");
        }
    }

    IEnumerator ExhaustEnemy()
    {
        enemyExhausted = true;
        animator.SetTrigger("Exhaust");
        audioSource.clip = snoringClip;
        audioSource.Play();

        yield return new WaitForSecondsRealtime(enemyExhaustTime);
        animator.SetTrigger("StopExhaust");
        audioSource.Stop();
        audioSource.clip = wakeupClip;
        audioSource.Play();

        Debug.Log("Enemy awaking");
        yield return new WaitForSeconds(toIdleAnimationDuration);

        audioSource.Stop();

        timesJumped = 0;
        enemyExhausted = false;
        currentState = State.Attack;
    }

    void PredictFallLocation()
    {
        float playerPositionX = GameObject.Find("Player").transform.position.x;
        float enemyPositionY = transform.position.y;
        float halfEnemyScaleX = transform.localScale.x / 2;
        float newEnemyPositionLeftX = playerPositionX - halfEnemyScaleX;
        float newEnemyPositionRightX = playerPositionX + halfEnemyScaleX;

        float halfLeftBossWallScaleX = GameObject.Find("LeftBossWall").transform.localScale.x / 2;
        float leftBossWallPositionRightX = GameObject.Find("LeftBossWall").transform.position.x + halfLeftBossWallScaleX;
        float halfRightBossWallScaleX = GameObject.Find("RightBossWall").transform.localScale.x / 2;
        float rightBossWallPositionLeftX = GameObject.Find("RightBossWall").transform.position.x - halfRightBossWallScaleX;
        if (newEnemyPositionLeftX < leftBossWallPositionRightX)
        {
            playerPositionX = leftBossWallPositionRightX + halfEnemyScaleX;
        }
        else if (newEnemyPositionRightX > rightBossWallPositionLeftX)
        {
            playerPositionX = rightBossWallPositionLeftX - halfEnemyScaleX;
        }
        float bossLandingPositionY = bossLanding.transform.position.y;
        bossLanding.transform.position = new Vector3(playerPositionX, bossLandingPositionY, bossLandingPositionZ);
        bossLanding.SetActive(true);
        transform.position = new Vector3(playerPositionX, enemyPositionY, 0);
    }

    void CheckEnemyInCameraView()
    {
        Vector3 viewPosition = Camera.main.WorldToViewportPoint(transform.position);
        if ((viewPosition.x > 0 && viewPosition.x < 1 && viewPosition.y > 0 && viewPosition.y < 1) || enemyTriggered)
        {
            if (currentState == State.Idle)
            {
                currentState = State.Attack;
                enemyTriggered = true;  
            }
        }
        else
        {
            currentState = State.Idle;
        }
    }

    void MoveTowardsPlayer()
    {
        if (!enemyMovementStarted)
        {
            StartCoroutine("StartEnemyMovementCycle");
            enemyMovementStarted = true;
        }
    }

    void StopEnemyMovementCycle()
    {
        StopCoroutine("StartEnemyMovementCycle");
        StopCoroutine("MoveEnemy");
        StopCoroutine("ExhaustEnemy");

        timesJumped = 0;

        enemyMovementStarted = false;
    }

    public void AddToCounter()
    {
        if(currentStunCounter < maxStunCounter)
        {
            currentStunCounter++;
        }
        if(currentStunCounter == maxStunCounter)
        {
            StartCoroutine("StunnedState");
        }
    }

    private IEnumerator StunnedState()
    {
        currentState = State.Stunned;
        animator.SetTrigger("Stun");
        enemyTriggered = false;

        audioSource.clip = stunnedClip;
        audioSource.Play();

        yield return new WaitForSecondsRealtime(stunTimer);
        currentStunCounter = 0;
        animator.SetTrigger("StopStun");

        audioSource.Stop();
        audioSource.clip = wakeupClip;
        audioSource.Play();
        yield return new WaitForSeconds(toIdleAnimationDuration);
        audioSource.Stop();

        enemyExhausted = false;
        currentState = State.Attack;
        enemyTriggered = true;
    }

    public State GetCurrentState()
    {
        return currentState;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myCapsuleCollider = GetComponent<CapsuleCollider>();
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myRigidbody2D.gravityScale = enemyGravityScale;
        bossLanding = GameObject.Find("BossLanding");
        bossLandingPositionZ = bossLanding.transform.position.z;
        bossLanding.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        CheckEnemyInCameraView();
        switch (currentState)
        {
            case State.Idle:
                // Debug.Log("Idle");
                break;
            case State.Attack:
                // Debug.Log("Attacking");
                if (!readyToBeExhausted) {MoveTowardsPlayer();}
                if (readyToBeExhausted && isGrounded)
                {
                    readyToBeExhausted = false;
                    currentState = State.Exhausted;
                }
                break;
            case State.Exhausted:
                // Debug.Log("Exhausted");
                if (!enemyExhausted && isGrounded)
                {
                    StartCoroutine("ExhaustEnemy");
                }
                break;
            case State.Stunned:
                // Debug.Log("Stunned");
                StopEnemyMovementCycle();
                break;
        }
    }
}
