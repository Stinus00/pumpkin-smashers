using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LightSideGround : HammerAttack
{
    [Header("General")]
    [SerializeField] private float dashTime = 1.0f;
    [SerializeField] private float moveSpeed = 10.0f;

    private Vector3 targetPosition;
    private Transform parent;
    [SerializeField] private Rigidbody2D rb2D;

    void Update()
    {
        OffsetAnimation();
    }

    void OnDisable()
    {
        StopAnimation();
    }

    IEnumerator Dash()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        Vector2 moveVelocity = new Vector2(moveInput * moveSpeed, 0);
        rb2D.linearVelocity = moveVelocity;
        playerActions.SetIsDashing(true);

        yield return new WaitForSeconds(dashTime);

        playerActions.SetIsDashing(false);
    }

    IEnumerator ActivateHitboxAttack()
    {
        StartAnimation();
        StartAttacking();
        yield return new WaitForSeconds(startAttack);
        StartCoroutine("Dash");
        // attack.SetActive(true);
    }
}
