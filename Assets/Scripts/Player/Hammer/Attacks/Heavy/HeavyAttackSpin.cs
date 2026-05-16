using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeavyAttackSpin : HammerAttack
{
    [Header("General")]
    [SerializeField] private List<GameObject> spinAttacks;
    [SerializeField] private List<GameObject> charges;
    
    [Header("Timing")]
    [SerializeField] private float chargeSpeed = 1.5f;

    private GameObject camera;
    private Transform parent;
    private Rigidbody2D rb2D;
    private IEnumerator chargeCounting;
    private IEnumerator disableAnimation;
    private float oldGravity;
    private bool attacking;
    private bool animation;
    private int chargeCount = 1;

    void Awake()
    {
        parent = this.transform.parent;
        rb2D = parent.gameObject.GetComponent<Rigidbody2D>();
        camera = GameObject.Find("Main Camera");
        oldGravity = rb2D.gravityScale;
    }

    void OnEnable()
    {
        attacking = false;
        chargeCounting = ChargeCounting();
        StartCoroutine(chargeCounting);

        playerActions = FindObjectOfType<PlayerActions>();

        playerActions.SetIsInAnimation(true);
    }

    void FixedUpdate()
    {
        // Stop moving during attack
        rb2D.linearVelocity = new Vector2(rb2D.linearVelocity.x * 0.5f, rb2D.linearVelocity.y);
    }

    void Update()
    {
        if(Input.GetKeyUp(KeyCode.K) && !animation)
        {
            StartAnimation();
            StopCoroutine(chargeCounting);
            attacking = true;
            animation = true;

            for(int i = 0; i < chargeCount; i++)
            {
                StartCoroutine(ActivateHitboxAttack(0.4f*i));
                StartCoroutine(DeactivateHitboxAttack(0.2f + 0.4f*i));
            }
            disableAnimation = DisableAnimation(0.4f*chargeCount);
            StartCoroutine(disableAnimation);
            StartCoroutine(Disable(0.4f*chargeCount + 0.6f));
        }
        if(attacking)
            OffsetAnimation();
    }

    IEnumerator ChargeCounting()
    {
        chargeCount = 1;
        charges[0].SetActive(true);

        yield return new WaitForSeconds(chargeSpeed);
        chargeCount = 2;
        charges[1].SetActive(true);

        yield return new WaitForSeconds(chargeSpeed);
        chargeCount = 3;
        charges[2].SetActive(true);

        yield return new WaitForSeconds(chargeSpeed);
        chargeCount = 4;
        charges[3].SetActive(true);
    }

    IEnumerator ActivateHitboxAttack(float delayTime = 0.0f)
    {
        yield return new WaitForSeconds(delayTime);
        spinAttacks[0].SetActive(true);
        yield return new WaitForSeconds(0.2f);
        spinAttacks[1].SetActive(true);
    }

    IEnumerator DeactivateHitboxAttack(float delayTime = 0.0f)
    {
        yield return new WaitForSeconds(delayTime);
        spinAttacks[0].SetActive(false);
        yield return new WaitForSeconds(0.2f);
        spinAttacks[1].SetActive(false);
    }

    void DeactivateHitboxVoid(GameObject attack)
    {
        attack.SetActive(false);
    }    
    
    IEnumerator DisableAnimation(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        attacking = false;

        StopAnimation();
    }
    
    IEnumerator Disable(float delayTime)
    {
        rb2D.gravityScale = oldGravity;

        yield return new WaitForSeconds(delayTime);

        foreach(GameObject charge in charges)
        {
            DeactivateHitboxVoid(charge);
        }

        playerActions.SetIsAttacking(false);
        playerActions.SetIsInAnimation(false);

        StopCoroutine(disableAnimation);

        animation = false;

        gameObject.SetActive(false);
    }
}
