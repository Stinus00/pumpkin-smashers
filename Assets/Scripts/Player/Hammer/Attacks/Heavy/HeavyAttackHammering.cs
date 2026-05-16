using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeavyAttackHammering : HammerAttack
{
    [Header("General")]
    [SerializeField] private List<GameObject> charges;
    [SerializeField] private float shakeDuration = 0.1f;
    
    [Header("Timing")]
    [SerializeField] private float chargeSpeed = 1.5f;

    private GameObject camera;
    private Transform parent;
    private Rigidbody2D rb2D;
    private IEnumerator chargeCounting;
    private IEnumerator disableAnimation;
    private bool attacking;
    private int chargeCount = 1;

    [SerializeField] private float jumpForce = 0.1f;

    void Awake()
    {
        parent = this.transform.parent;
        rb2D = parent.gameObject.GetComponent<Rigidbody2D>();
        camera = GameObject.Find("Main Camera");
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
        if(Input.GetKeyUp(KeyCode.K) && !attacking)
        {
            StartAnimation();
            StopCoroutine(chargeCounting);
            attacking = true;

            for(int i = 0; i < chargeCount; i++)
            {
                StartCoroutine(ActivateHitboxAttack(attack, 0.2f + 0.4f*i));
                StartCoroutine(DeactivateHitboxAttack(attack, 0.4f + 0.4f*i));
            }
            if (chargeCount == 1)
                disableAnimation = DisableAnimation(0.3f);
            else
                disableAnimation = DisableAnimation(0.4f*chargeCount);
            StartCoroutine(disableAnimation);
            StartCoroutine(Disable(0.4f*chargeCount + 0.6f));
        }
    }

    IEnumerator ChargeCounting()
    {
        chargeCount = 1;
        charges[0].SetActive(true);

        yield return new WaitForSeconds(chargeSpeed);
        chargeCount = 3;
        charges[1].SetActive(true);

        yield return new WaitForSeconds(chargeSpeed);
        chargeCount = 5;
        charges[2].SetActive(true);
    }

    IEnumerator ActivateHitboxAttack(GameObject attack, float delayTime = 0.0f)
    {
        yield return new WaitForSeconds(delayTime);
        OffsetAnimation();
        rb2D.linearVelocity = new Vector2(rb2D.linearVelocity.x, jumpForce);
        camera.GetComponent<ShakeScreen>().TriggerShake(shakeDuration);
        attack.SetActive(true);
    }

    IEnumerator DeactivateHitboxAttack(GameObject attack, float delayTime = 0.0f)
    {
        yield return new WaitForSeconds(delayTime);
        attack.SetActive(false);
    }

    void DeactivateHitboxVoid(GameObject attack)
    {
        attack.SetActive(false);
    }

    IEnumerator DisableAnimation(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        StopAnimation();
    }
    
    IEnumerator Disable(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        foreach(GameObject charge in charges)
        {
            DeactivateHitboxVoid(charge);
        }

        playerActions.SetIsAttacking(false);
        playerActions.SetIsInAnimation(false);

        StopCoroutine(disableAnimation);

        attacking = false;

        gameObject.SetActive(false);
    }
}
